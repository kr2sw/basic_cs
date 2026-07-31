import io
import time
from pathlib import Path

from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import StreamingResponse

app = FastAPI(title="대용량 스트리밍 - StreamingResponse / 청크 전송")

CHUNK_SIZE = 1024 * 1024  # 1MB

# 데모용 대용량 파일
BIG_FILE = Path("big_sample.txt")
if not BIG_FILE.exists():
    with open(BIG_FILE, "w", encoding="utf-8") as f:
        for i in range(200_000):
            f.write(f"샘플 데이터 라인 {i}\n")


# 1) 제너레이터로 큰 응답 생성: 메모리에 모두 올리지 않고 조각 단위로 생성
def csv_generator(rows: int = 100_000):
    yield "id,name,price\n"
    batch = []
    for i in range(rows):
        batch.append(f"{i},상품{i},{i * 137}\n")
        if len(batch) >= 1000:  # 1000행씩 묶어 생성 -> 반환 -> 메모리 해제
            yield "".join(batch)
            batch.clear()
    if batch:
        yield "".join(batch)


@app.get("/big.csv")
def big_csv():
    """1,000행 단위 배치로 생성되는 대용량 CSV (1 ~ 100만행 조정 가능)"""
    return StreamingResponse(
        csv_generator(rows=100_000),
        media_type="text/csv",
        headers={"Content-Disposition": 'attachment; filename="big.csv"'},
    )


# 2) 파일을 청크 단위로 읽어 스트리밍 다운로드
def iter_file(path: Path):
    with open(path, "rb") as f:
        while chunk := f.read(CHUNK_SIZE):
            yield chunk


@app.get("/download/{filename}")
def download(filename: str):
    """디스크의 파일을 1MB 청크로 나눠 전송"""
    path = (Path(".") / filename).resolve()
    base = Path(".").resolve()
    if not str(path).startswith(str(base)) or not path.is_file():
        raise HTTPException(status_code=404, detail="파일을 찾을 수 없습니다")

    return StreamingResponse(
        iter_file(path),
        media_type="application/octet-stream",
        headers={"Content-Disposition": f'attachment; filename="{path.name}"'},
    )


# 3) 요청 본문 스트리밍 수신: 큰 업로드를 메모리에 올리지 않는다
@app.post("/stream-upload")
async def stream_upload(request: Request):
    """전송 계층에서 청크 단위로 들어오는 본문을 누적 크기만 계산"""
    total = 0
    async for chunk in request.stream():
        total += len(chunk)
    return {"received_bytes": total}


# 4) 지연 생성 응답: 처음은 즉시, 이후는 천천히 (progress/시간순 데이터 예시)
def slow_generator():
    for i in range(10):
        yield f"{i}: {time.time():.3f}\n"
        time.sleep(0.5)


@app.get("/slow")
def slow_stream():
    """0.5초 간격으로 흐르는 데이터 (SSE가 아닌 일반 스트리밍 데모)"""
    return StreamingResponse(slow_generator(), media_type="text/plain")
