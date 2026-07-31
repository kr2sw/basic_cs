import logging
import time

import structlog
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

# ---- structlog 설정: 구조화된 로그(JSON) 출력 ----
structlog.configure(
    processors=[
        structlog.contextvars.merge_contextvars,
        structlog.processors.add_log_level,
        structlog.processors.TimeStamper(fmt="iso"),
        structlog.processors.StackInfoRenderer(),
        structlog.processors.JSONRenderer(),
    ],
    wrapper_class=structlog.make_filtering_bound_logger(logging.INFO),
    logger_factory=structlog.PrintLoggerFactory(),
)
log = structlog.get_logger()


class Metrics:
    """인메모리 메트릭 수집기 (프로덕션에서는 Prometheus 클라이언트 사용)"""

    def __init__(self):
        self.requests = 0
        self.errors = 0
        self.total_latency_ms = 0.0
        self.path_counts: dict[str, int] = {}
        self.path_errors: dict[str, int] = {}
        self.started_at = time.time()

    def observe(self, path: str, status_code: int, latency_ms: float):
        self.requests += 1
        self.total_latency_ms += latency_ms
        self.path_counts[path] = self.path_counts.get(path, 0) + 1
        if status_code >= 500:
            self.errors += 1
            self.path_errors[path] = self.path_errors.get(path, 0) + 1

    def render_prometheus(self) -> str:
        """Prometheus 텍스트 포맷으로 메트릭 노출"""
        lines = [
            "# HELP http_requests_total HTTP 요청 수",
            "# TYPE http_requests_total counter",
            f"http_requests_total {self.requests}",
            f"http_request_errors_total {self.errors}",
            "# HELP http_request_duration_ms_total 누적 처리 시간",
            "# TYPE http_request_duration_ms_total counter",
            f"http_request_duration_ms_total {self.total_latency_ms:.1f}",
            "# HELP http_requests_by_path 경로별 요청 수",
            "# TYPE http_requests_by_path counter",
        ]
        for path, count in sorted(self.path_counts.items()):
            lines.append(f'http_requests_by_path{{path="{path}"}} {count}')
        lines.append("# HELP process_uptime_seconds 프로세스 실행 시간")
        lines.append("# TYPE process_uptime_seconds gauge")
        lines.append(f"process_uptime_seconds {time.time() - self.started_at:.0f}")
        return "\n".join(lines) + "\n"


metrics = Metrics()


@app.middleware("http")
async def monitor(request: Request, call_next):
    """모든 요청에 대해 로깅과 메트릭 수집"""
    start = time.perf_counter()
    try:
        response = await call_next(request)
    except Exception:
        # 오류도 메트릭에 기록하고 재발생
        metrics.observe(request.url.path, 500, (time.perf_counter() - start) * 1000)
        raise
    latency_ms = (time.perf_counter() - start) * 1000
    metrics.observe(request.url.path, response.status_code, latency_ms)

    # 구조화된 로그: JSON으로 한 줄씩 출력
    log.info(
        "request",
        method=request.method,
        path=request.url.path,
        status=response.status_code,
        latency_ms=round(latency_ms, 2),
        client=request.client.host if request.client else None,
    )
    return response


app = FastAPI(title="모니터링 - structlog + Prometheus 메트릭")


@app.get("/health")
def health():
    return {"status": "ok", "uptime": round(time.time() - metrics.started_at, 1)}


@app.get("/metrics", include_in_schema=False)
def metrics_endpoint():
    """Prometheus 스크레이퍼가 수집하는 엔드포인트"""
    return JSONResponse(content=metrics.render_prometheus(), media_type="text/plain; version=0.0.4")


@app.get("/slow")
async def slow_endpoint():
    """지연이 큰 요청 (메트릭/로그에 반영)"""
    time.sleep(0.5)
    return {"message": "느린 응답"}


@app.get("/error")
def error_endpoint():
    raise RuntimeError("의도적인 오류 (메트릭/로그에 반영)")
