import asyncio
import time
import uuid
from contextlib import asynccontextmanager

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

# 프로세스 내 작업 큐 (프로덕션에서는 Celery/ARQ + Redis 사용)
job_queue: asyncio.Queue[str] = asyncio.Queue()
jobs: dict[str, dict] = {}
WORKERS = 2


def process_job(job_id: str, payload: str) -> str:
    """실제 작업 예시: 이메일 발송, 이미지 리사이즈 등 (여기서는 sleep으로 대체)"""
    time.sleep(1)
    return f"처리 완료: {payload}"


async def worker(worker_id: int):
    """큐에서 작업을 꺼내 실행하고 상태를 갱신"""
    while True:
        job_id = await job_queue.get()
        try:
            job = jobs.get(job_id)
            if not job or job["status"] == "cancelled":
                continue
            job["status"] = "running"
            job["started_at"] = time.time()
            # 블로킹 작업은 별도 스레드로 실행해 이벤트 루프를 보호
            result = await asyncio.to_thread(process_job, job_id, job["payload"])
            job["status"] = "done"
            job["result"] = result
        except Exception as e:
            if job_id in jobs:
                jobs[job_id]["status"] = "failed"
                jobs[job_id]["error"] = str(e)
        finally:
            job_queue.task_done()


@asynccontextmanager
async def lifespan(app: FastAPI):
    # 시작 시 워커 생성
    workers = [asyncio.create_task(worker(i)) for i in range(WORKERS)]
    yield
    # 종료 시 작업 취소 (정상 종료를 위해서는 graceful shutdown 처리 필요)
    for w in workers:
        w.cancel()


app = FastAPI(title="백그라운드 작업 - InProcess 큐", lifespan=lifespan)


class JobCreate(BaseModel):
    payload: str


@app.post("/jobs", status_code=202)
async def enqueue(data: JobCreate):
    """작업을 큐에 등록하고 즉시 202 응답"""
    job_id = uuid.uuid4().hex
    jobs[job_id] = {
        "id": job_id,
        "payload": data.payload,
        "status": "queued",
        "created_at": time.time(),
        "result": None,
        "error": None,
    }
    await job_queue.put(job_id)
    return {"id": job_id, "status": "queued", "queue_size": job_queue.qsize()}


@app.get("/jobs")
def list_jobs():
    return [
        {
            "id": j["id"],
            "status": j["status"],
            "payload": j["payload"],
            "result": j["result"],
            "error": j["error"],
        }
        for j in jobs.values()
    ]


@app.get("/jobs/{job_id}")
def get_job(job_id: str):
    job = jobs.get(job_id)
    if job is None:
        raise HTTPException(status_code=404, detail="작업을 찾을 수 없습니다")
    return job


@app.post("/jobs/{job_id}/cancel")
async def cancel_job(job_id: str):
    job = jobs.get(job_id)
    if job is None:
        raise HTTPException(status_code=404, detail="작업을 찾을 수 없습니다")
    if job["status"] == "queued":
        job["status"] = "cancelled"
        return {"message": "작업이 취소되었습니다"}
    raise HTTPException(status_code=409, detail="실행 중이거나 완료된 작업은 취소할 수 없습니다")
