# 27: 백그라운드 작업 — Celery/ARQ 개념, InProcess 큐

기초 챕터 15의 `BackgroundTasks`는 요청이 끝난 뒤 **같은 프로세스**에서만 실행되며, 실패 처리나 재시도, 분산 실행이 어렵습니다. 이번에는 **작업 큐(job queue)** 개념으로 백그라운드 작업을 설계합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

`POST /jobs`로 작업을 넣으면 워커 2개가 순차 처리합니다. `GET /jobs/{id}`로 상태를 확인할 수 있습니다.

## 주요 개념

### InProcess 큐 (데모)

`asyncio.Queue` + 워커 태스크를 사용한 단일 프로세스 큐입니다. 워커는 시작 시 생성되고 종료 시 취소됩니다.

```python
async def worker(worker_id):
    while True:
        job_id = await job_queue.get()
        result = await asyncio.to_thread(process_job, job_id, payload)
        jobs[job_id]["status"] = "done"
        job_queue.task_done()
```

블로킹 작업은 `asyncio.to_thread()`로 별도 스레드에 맡겨 이벤트 루프가 멈추지 않게 합니다. 상태는 `queued -> running -> done/failed`로 관리합니다.

### Celery / ARQ — 분산 작업 큐

InProcess 큐는 **단일 프로세스, 재시작 시 작업 유실**이라는 한계가 있습니다. 실제 서비스에서는 메시지 브로커를 사용합니다.

- **Celery (RabbitMQ/Redis)**: 가장 널리 쓰이는 Python 작업 큐. 워커를 별도 프로세스/머신으로 분산 실행.
- **ARQ (Redis)**: FastAPI/Starlette과 잘 어울리는 가벼운 async 작업 큐.

```python
# ARQ 예시 (개념)
from arq import create_pool
from arq.connections import RedisSettings

async def enqueue_job(context, payload):
    job = await context["pool"].enqueue_job("process_job", payload)
    return {"job_id": job.job_id}
```

### 왜 상태 저장이 필요한가

`BackgroundTasks`는 실행 결과를 알 수 없습니다. 반면 큐 방식은 작업별 `id`와 상태를 저장하므로:

- 클라이언트가 폴링/웹훅으로 결과 조회
- 실패한 작업 재시도 및 재시도 횟수 관리
- 재시작 후에도 이어서 처리 (영속화 시)

가 가능합니다.

### 202 Accepted

큐에 넣은 시점이 곧 완료가 아니므로 `status_code=202`(접수됨)과 작업 ID를 즉시 반환합니다.

## 연습

1. 실패한 작업을 3회까지 재시도하는 로직을 `worker`에 추가해 보세요.
2. 재시작 시 작업이 유실되지 않도록 SQLite에 작업을 기록해 보세요.
