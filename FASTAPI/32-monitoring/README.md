# 32: 모니터링 — structlog, Prometheus 메트릭 개념

운영 환경에서 가장 중요한 것 중 하나는 **문제를 빨리 발견**하는 것입니다. 이번 챕터에서는 **구조화된 로깅(structlog)**과 **Prometheus 메트릭** 개념을 구현합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

- `/health`: 상태 확인
- `/metrics`: Prometheus 텍스트 포맷 메트릭 (http://127.0.0.1:8000/metrics)
- `/slow`: 0.5초 지연 응답
- `/error`: 의도적인 오류 발생

## 주요 개념

### structlog — 구조화된 로깅

일반 텍스트 로그는 검색/집계가 어렵습니다. structlog는 로그를 **키-값 쌍**으로 남기고 JSON으로 직렬화합니다.

```python
structlog.configure(processors=[TimeStamper(fmt="iso"), JSONRenderer()])
log = structlog.get_logger()
log.info("request", method="GET", path="/health", status=200, latency_ms=3.2)
```

```
{"method": "GET", "path": "/health", "status": 200, "latency_ms": 3.2, ...}
```

`contextvars` 프로세서를 추가하면 요청 간 컨텍스트(request_id, tenant_id)를 자동으로 부여할 수 있습니다.

### 메트릭과 Prometheus

메트릭은 **수치로 표현되는 지표**입니다. Prometheus가 주기적으로 `/metrics`를 스크레이핑해 시계열로 저장하고, Grafana로 대시보드를 만듭니다.

대표적인 타입:

| 타입 | 의미 | 예시 |
|------|------|------|
| counter | 단조 증가 카운터 | 요청 수, 오류 수 |
| gauge | 증감 가능한 값 | 현재 커넥션 수, uptime |
| histogram | 값의 분포 | 응답 지연 분포 |

텍스트 포맷 예시:

```
http_requests_total 1200
http_requests_by_path{path="/health"} 300
```

이번 챕터는 의존성 없이 직접 구현했지만, 실제로는 `prometheus_client` 라이브러리를 사용합니다.

```python
from prometheus_client import Counter, Histogram
REQUESTS = Counter("http_requests_total", "HTTP 요청 수", ["path"])
LATENCY = Histogram("http_request_duration_seconds", "응답 지연", ["path"])
```

### 미들웨어에서 수집

요청의 시작/끝 시각과 상태 코드를 미들웨어에서 수집하고, `Response`를 다음 레이어로 그대로 넘깁니다.

```python
@app.middleware("http")
async def monitor(request, call_next):
    start = time.perf_counter()
    response = await call_next(request)
    metrics.observe(request.url.path, response.status_code, ...)
    return response
```

## SLO/알림 예시

- 응답 지연 p99 < 300ms
- 5xx 비율 < 0.1%
- 오류 대시보드 + Slack 알림 연동

## 연습

1. `/slow` 호출 후 `/metrics`에서 latency 값이 누적되는지 확인해 보세요.
2. `contextvars`로 `request_id`를 로그에 자동 포함시켜 보세요.
