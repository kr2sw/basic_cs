# 28: 속도 제한 — RateLimiter 미들웨어 구현

무제한 요청은 비용 증가, 리소스 고갈, 크롤링/공격 등으로 이어집니다. 이번 챕터에서는 **ASGI 미들웨어**로 동작하는 자체 RateLimiter를 구현합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

`RATE_LIMIT=10`이 기본이므로, `/limited`를 10번 이상 연속 호출하면 429 응답이 반환됩니다. 환경 변수로 조정할 수 있습니다.

## 주요 개념

### 슬라이딩 윈도우 알고리즘

IP별로 요청 시각을 저장해 **현재 시각 기준 최근 N초 동안의 요청 수**를 셉니다. 오래된 기록은 버리고, 임계값을 넘으면 거부합니다.

```python
while dq and now - dq[0] > self.period:
    dq.popleft()          # 윈도우 밖 기록 제거
if len(dq) >= self.limit:
    return False, retry_after
dq.append(now)
```

고정 윈도우(고정 시간 단위 초기화)보다 경계 시점의 폭주에 강합니다. 단일 프로세스 메모리 저장이므로, 다중 워커에서는 Redis(Lua 스크립트) 등을 사용해야 합니다.

### ASGI 미들웨어

FastAPI의 `add_middleware`로 전체 요청 흐름에 끼워 넣습니다. HTTP 요청이면 IP를 키로 검사하고, 거부 시 429를 바로 응답합니다.

```python
class RateLimitMiddleware:
    async def __call__(self, scope, receive, send):
        client_ip = request.client.host
        allowed, retry_after = self.limiter.allow(client_ip)
        if not allowed:
            await JSONResponse(429, headers={"Retry-After": ...})(scope, receive, send)
```

### 제외 경로

Swagger 문서(`/docs`, `/openapi.json`)와 헬스체크는 제한에서 제외합니다. 운영에서는 사내 IP 화이트리스트도 적용합니다.

### Retry-After 헤더

429 응답에는 표준 헤더 `Retry-After`(초)를 포함해 클라이언트가 재시도 시점을 알 수 있게 합니다.

```bash
curl -i http://localhost:8000/limited   # 429 ... Retry-After: 32
```

## 프로덕션 대안

- `slowapi`(Flask-Limiter 계열), `limits` 라이브러리
- Redis 기반 **token bucket / sliding window** (다중 워커 공유)
- Nginx `limit_req` 모듈 (엣지에서 1차 방어)

## 연습

1. `RATE_LIMIT=3 RATE_PERIOD=5`로 실행해 3회 호출 후 429가 오는지 확인해 보세요.
2. `RateLimitMiddleware`에 `X-API-Key` 기반 제한(클라이언트별)을 추가해 보세요.
