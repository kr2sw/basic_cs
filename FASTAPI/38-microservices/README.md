# 38: 마이크로서비스 — httpx 호출 패턴, 디스커버리

하나의 커다란 앱 대신 **작은 서비스들로 분리**하고 서로 HTTP로 통신하는 아키텍처가 마이크로서비스입니다. 이번 챕터에서는 서비스 간 호출의 핵심 패턴(**httpx**, 재시도/타임아웃, 디스커버리)을 데모로 배웁니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

이번 챕터는 단일 프로세스 안에 내부 서비스(`/internal/...`)와 게이트웨이(`/api/...`)를 함께 두어 호출 패턴을 보여줍니다. 실제로는 각 서비스가 별도 컨테이너로 실행됩니다.

- `GET /api/users/1` — user-service 단일 호출
- `GET /api/users/1/orders` — order-service 호출 (쿼리 파라미터)
- `GET /api/aggregate/1` — 여러 서비스 병합 (fan-out)
- `GET /api/services` — 레지스트리/상태 확인

## 주요 개념

### httpx.AsyncClient — 비동기 HTTP 클라이언트

`httpx`는 FastAPI의 테스트 클라이언트이자 서비스 호출용 HTTP 클라이언트입니다. `async with`로 세션을 재사용합니다.

```python
async with httpx.AsyncClient(timeout=3.0) as client:
    resp = await client.get(url, params={"user_id": user_id})
    resp.raise_for_status()
    return resp.json()
```

### 타임아웃과 재시도 (일시적 장애 대응)

마이크로서비스 환경은 장애가 흔합니다. 요청마다 타임아웃을 지정하고, 일시적 장애(연결 실패)에는 **지수 백오프**로 재시도합니다. `ConnectError`는 서비스 다운, `TimeoutException`은 과부하를 의미하므로 다르게 대처합니다.

```python
for attempt in range(retries):
    try:
        resp = await client.get(url)
        resp.raise_for_status()
        return resp.json()
    except httpx.HTTPError:
        if attempt == retries - 1:
            raise HTTPException(503, "서비스 호출 실패")
        await asyncio.sleep(0.2 * (attempt + 1))
```

### Fan-out 집계 패턴

여러 서비스를 호출할 때 순차 호출이 아니라 `asyncio.gather`로 **동시에** 호출해 전체 지연을 줄입니다. 핵심 서비스(사용자) 실패는 전체 실패로, 부가 서비스(주문) 실패는 빈 값으로 처리하는 **회복 탄력성(fail-open)** 설계를 적용합니다.

```python
user, orders = await asyncio.gather(
    fetch(user_url), fetch(orders_url), return_exceptions=True
)
```

### 서비스 디스커버리

서로의 IP/포트를 코드에 하드코딩하면 유지보수가 불가능합니다. **레지스트리**(Kubernetes Service DNS, Consul, etcd)에서 동적으로 주소를 찾습니다. 데모에서는 `SERVICES` 딕셔너리를 레지스트리로 사용합니다.

```python
SERVICES = {"user": {"base_url": "http://user-service:8000", "healthy": True}}
```

헬스체크(`/health`)로 레지스트리의 상태를 주기적으로 갱신하고, 죽은 인스턴스로는 라우팅하지 않습니다.

## 연습

1. `service_registry` 응답을 캐시해 헬스체크 호출을 줄여 보세요.
2. `timeout=3.0`을 짧게 바꾸고 내부 서비스를 일부러 실패시켜 재시도/오류 처리가 동작하는지 확인해 보세요.
