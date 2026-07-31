import os
import time
from collections import defaultdict, deque

from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

# 환경 변수로 설정 (기본: IP당 1분에 10회)
RATE_LIMIT = int(os.getenv("RATE_LIMIT", "10"))
RATE_PERIOD = int(os.getenv("RATE_PERIOD", "60"))


class SlidingWindowLimiter:
    """슬라이딩 윈도우 방식 속도 제한: 시간당 요청 수를 윈도우로 관리"""

    def __init__(self, limit: int, period: int):
        self.limit = limit
        self.period = period
        self.hits: dict[str, deque] = defaultdict(deque)  # key -> 타임스탬프 목록

    def allow(self, key: str) -> tuple[bool, int]:
        """요청 허용 여부와 Retry-After(초) 반환"""
        now = time.monotonic()
        dq = self.hits[key]
        # 윈도우 밖의 오래된 기록은 제거
        while dq and now - dq[0] > self.period:
            dq.popleft()
        if len(dq) >= self.limit:
            retry_after = int(self.period - (now - dq[0])) + 1
            return False, max(retry_after, 1)
        dq.append(now)
        return True, 0


class RateLimitMiddleware:
    """ASGI 미들웨어로 모든 요청에 속도 제한 적용"""

    def __init__(self, app, limiter: SlidingWindowLimiter, exempt_paths=()):
        self.app = app
        self.limiter = limiter
        self.exempt_paths = set(exempt_paths)

    async def __call__(self, scope, receive, send):
        if scope["type"] != "http":
            await self.app(scope, receive, send)
            return

        request = Request(scope)
        # 문서/메타데이터는 제한에서 제외
        if request.url.path in self.exempt_paths:
            await self.app(scope, receive, send)
            return

        client_ip = request.client.host if request.client else "unknown"
        allowed, retry_after = self.limiter.allow(client_ip)
        if not allowed:
            response = JSONResponse(
                status_code=429,
                content={"detail": "요청이 너무 많습니다. 잠시 후 다시 시도하세요."},
                headers={"Retry-After": str(retry_after)},
            )
            await response(scope, receive, send)
            return

        await self.app(scope, receive, send)


limiter = SlidingWindowLimiter(limit=RATE_LIMIT, period=RATE_PERIOD)
app = FastAPI(title="속도 제한 - RateLimiter 미들웨어")
app.add_middleware(RateLimitMiddleware, limiter=limiter, exempt_paths={"/health", "/docs", "/redoc", "/openapi.json"})


@app.get("/health", summary="제한에서 제외되는 경로")
def health():
    return {"status": "ok"}


@app.get("/", summary="일반 요청 (제한 대상)")
def home():
    return {"message": "hello"}


@app.get("/limited")
def limited_endpoint():
    """제한에 걸리면 429 응답 + Retry-After 헤더 확인"""
    return {"message": "제한된 엔드포인트입니다"}
