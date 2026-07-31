import hashlib
import re
import uuid
from pathlib import Path

from fastapi import FastAPI, File, HTTPException, UploadFile
from fastapi.responses import StreamingResponse
from pydantic import BaseModel

app = FastAPI(title="파일 처리 - 업로드 / 청크 / 스트리밍")

UPLOAD_DIR = Path("uploads")
PART_DIR = Path("uploads/.parts")
UPLOAD_DIR.mkdir(exist_ok=True)
PART_DIR.mkdir(parents=True, exist_ok=True)

MAX_FILE_SIZE = 10 * 1024 * 1024  # 10MB 제한
ALLOWED_EXTENSIONS = {".txt", ".jpg", ".jpeg", ".png", ".pdf", ".csv"}
CHUNK_SIZE = 1024 * 1024  # 1MB


def validate_file(filename: str) -> Path:
    """확장자 화이트리스트 + 경로 탈출 방지"""
    name = Path(filename).name  # ../etc/passwd 같은 경로 조작 차단
    if Path(name).suffix.lower() not in ALLOWED_EXTENSIONS:
        raise HTTPException(status_code=415, detail=f"허용 확장자: {ALLOWED_EXTENSIONS}")
    return UPLOAD_DIR / name


@app.post("/upload", summary="일반 업로드 (검증 포함)")
async def upload(file: UploadFile = File(...)):
    """업로드 파일을 청크 단위로 읽으며 크기 제한과 해시를 검증"""
    path = validate_file(file.filename or "file.bin")

    total = 0
    hasher = hashlib.sha256()
    with open(path, "wb") as f:
        while chunk := await file.read(CHUNK_SIZE):
            total += len(chunk)
            if total > MAX_FILE_SIZE:
                path.unlink(missing_ok=True)
                raise HTTPException(status_code=413, detail=f"파일이 {MAX_FILE_SIZE // 1024 // 1024}MB를 초과합니다")
            hasher.update(chunk)
            f.write(chunk)

    return {
        "filename": path.name,
        "size": total,
        "sha256": hasher.hexdigest(),
        "saved_at": str(path),
    }


# ---- 청크(조각) 업로드: 대용량 파일을 조각으로 나눠 전송 ----
class ChunkStart(BaseModel):
    filename: str
    total_chunks: int


class ChunkRef(BaseModel):
    upload_id: str
    chunk_index: int


class CompleteIn(BaseModel):
    upload_id: str


@app.post("/chunked/start")
def chunk_start(data: ChunkStart):
    """업로드 세션 생성 -> upload_id 반환 (원본 파일명도 함께 보관)"""
    upload_id = uuid.uuid4().hex
    session = PART_DIR / upload_id
    session.mkdir(exist_ok=True)
    (session / "_filename").write_text(data.filename, encoding="utf-8")
    return {"upload_id": upload_id, "total_chunks": data.total_chunks, "filename": data.filename}


@app.post("/chunked/{upload_id}/parts/{chunk_index}", summary="조각 전송")
async def chunk_upload(upload_id: str, chunk_index: int, file: UploadFile = File(...)):
    """각 조각을 별도 파일로 저장"""
    session = PART_DIR / upload_id
    if not session.is_dir():
        raise HTTPException(status_code=404, detail="업로드 세션을 찾을 수 없습니다")
    with open(session / f"part_{chunk_index:06d}", "wb") as f:
        while chunk := await file.read(CHUNK_SIZE):
            f.write(chunk)
    return {"upload_id": upload_id, "chunk_index": chunk_index, "received": True}


@app.post("/chunked/{upload_id}/complete", summary="조각 병합")
async def chunk_complete(upload_id: str, total_chunks: int):
    """모든 조각을 순서대로 병합해 최종 파일 생성"""
    session = PART_DIR / upload_id
    parts = sorted(session.glob("part_*"))
    if len(parts) != total_chunks:
        raise HTTPException(status_code=409, detail=f"조각 불일치: {len(parts)}/{total_chunks}")

    # 세션 시작 때 보관한 원본 파일명을 복원해 검증
    filename = (session / "_filename").read_text(encoding="utf-8")
    final_path = validate_file(filename)

    with open(final_path, "wb") as out:
        for p in parts:
            out.write(p.read_bytes())

    # 세션 정리
    for p in parts:
        p.unlink()
    (session / "_filename").unlink()
    session.rmdir()

    return {"filename": final_path.name, "size": final_path.stat().st_size}


@app.get("/download/{filename}", summary="청크 단위 스트리밍 다운로드")
def download(filename: str):
    """대용량 파일을 메모리에 모두 올리지 않고 조각 단위로 전송"""
    path = validate_file(filename)
    if not path.is_file():
        raise HTTPException(status_code=404, detail="파일을 찾을 수 없습니다")

    def iter_chunks():
        with open(path, "rb") as f:
            while chunk := f.read(CHUNK_SIZE):
                yield chunk

    return StreamingResponse(
        iter_chunks(),
        media_type="application/octet-stream",
        headers={"Content-Disposition": f'attachment; filename="{path.name}"'},
    )


@app.get("/files")
def list_files():
    """업로드된 파일 목록"""
    return [{"name": p.name, "size": p.stat().st_size} for p in UPLOAD_DIR.iterdir() if p.is_file()]
