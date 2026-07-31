import asyncio
import os

import httpx
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

app = FastAPI(title="마이크로서비스 - httpx 호출 패턴")

# 서비스 레지스트리 (프로덕션에서는 Consul/etcd/Kubernetes DNS 사용)
GATEWAY_URL = os.getenv("GATEWAY_URL", "http://localhost:8000")
SERVICES = {
    "user": {"base_url": f"{GATEWAY_URL}/internal/users", "healthy": True},
    "order": {"base_url": f"{GATEWAY_URL}/internal/orders", "healthy": True},
}

# ---- 내부 서비스 (user-service, order-service 역할) ----
FAKE_USERS = {1: {"id": 1, "name": "alice"}, 2: {"id": 2, "name": "bob"}}
FAKE_ORDERS = [
    {"id": 101, "user_id": 1, "item": "노트북", "amount": 1200000},
    {"id": 102, "user_id": 1, "item": "마우스", "amount": 29000},
    {"id": 103, "user_id": 2, "item": "모니터", "amount": 350000},
]


@app.get("/internal/health")
def internal_health():
    """내부 서비스 상태 확인용"""
    return {"status": "ok"}


@app.get("/internal/users/{user_id}")
def internal_get_user(user_id: int):
    user = FAKE_USERS.get(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="사용자를 찾을 수 없습니다")
    return user


@app.get("/internal/orders")
def internal_list_orders(user_id: int | None = None):
    if user_id is None:
        return FAKE_ORDERS
    return [o for o in FAKE_ORDERS if o["user_id"] == user_id]


# ---- 게이트웨이/집계 서비스: httpx로 다른 서비스 호출 ----

async def fetch_with_retry(client: httpx.AsyncClient, url: str, retries: int = 3) -> dict:
    """재시도가 포함된 안전한 HTTP 호출 (일시적 장애 대응)"""
    for attempt in range(retries):
        try:
            resp = await client.get(url)
            resp.raise_for_status()
            return resp.json()
        except httpx.HTTPError:
            if attempt == retries - 1:
                raise HTTPException(status_code=503, detail=f"서비스 호출 실패: {url}")
            await asyncio.sleep(0.2 * (attempt + 1))  # 지수 백오프


@app.get("/api/services")
async def service_registry():
    """레지스트리 목록 + 상태 확인 (디스커버리 데모)"""
    result = {}
    async with httpx.AsyncClient(timeout=2.0) as client:
        for name, info in SERVICES.items():
            try:
                resp = await client.get(f"{GATEWAY_URL}/internal/health")
                result[name] = {"url": info["base_url"], "healthy": resp.status_code == 200}
            except httpx.HTTPError:
                result[name] = {"url": info["base_url"], "healthy": False}
    return result


@app.get("/api/users/{user_id}")
async def get_user(user_id: int):
    """user-service 단일 호출"""
    async with httpx.AsyncClient(timeout=3.0) as client:
        url = f"{SERVICES['user']['base_url']}/{user_id}"
        try:
            resp = await client.get(url)
            if resp.status_code == 404:
                raise HTTPException(status_code=404, detail="사용자를 찾을 수 없습니다")
            resp.raise_for_status()
            return resp.json()
        except httpx.ConnectError:
            raise HTTPException(status_code=503, detail="user-service에 연결할 수 없습니다")


@app.get("/api/users/{user_id}/orders")
async def user_orders(user_id: int):
    """order-service 호출 (쿼리 파라미터 전달)"""
    async with httpx.AsyncClient(timeout=3.0) as client:
        resp = await client.get(SERVICES["order"]["base_url"], params={"user_id": user_id})
        resp.raise_for_status()
        return resp.json()


@app.get("/api/aggregate/{user_id}")
async def aggregate(user_id: int):
    """fan-out 패턴: 여러 서비스 응답을 동시에 수집해 병합"""
    async with httpx.AsyncClient(timeout=5.0) as client:
        user_task = fetch_with_retry(client, f"{SERVICES['user']['base_url']}/{user_id}")
        orders_task = fetch_with_retry(client, f"{SERVICES['order']['base_url']}", retries=2)
        user, orders = await asyncio.gather(user_task, orders_task, return_exceptions=True)

        if isinstance(user, HTTPException):
            raise user
        if isinstance(orders, HTTPException):
            orders = []  # 부가 서비스 실패는 회복 탄력적으로 처리

    return {"user": user, "orders": [o for o in orders if o["user_id"] == user_id]}
