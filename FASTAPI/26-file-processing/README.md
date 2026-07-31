# 26: 파일 처리 — 청크 업로드, 스트리밍, 검증

기초 챕터 12에서 단일 파일 업로드를 다뤘습니다. 이번에는 대용량 파일을 다룰 때 필요한 **청크 업로드**, **스트리밍 다운로드**, 그리고 **파일 검증**을 구현합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

## 주요 개념

### 파일 검증 — 확장자 화이트리스트

사용자가 보낸 파일명은 경로 탈출에 악용될 수 있습니다. `Path(filename).name`으로 베이스 이름만 추출하고, 허용 확장자 목록을 검사합니다.

```python
name = Path(filename).name          # ../etc/passwd 차단
if Path(name).suffix.lower() not in ALLOWED_EXTENSIONS:
    raise HTTPException(415, "허용되지 않은 확장자")
```

업로드와 함께 **콘텐츠 타입 검사**와 **크기 제한**도 추가합니다.

### 청크 단위 읽기 (메모리 보호)

`await file.read(CHUNK_SIZE)`로 조금씩 읽으면서 쓰면 큰 파일도 메모리에 모두 올리지 않습니다.

```python
while chunk := await file.read(1024 * 1024):
    total += len(chunk)
    if total > MAX_FILE_SIZE:
        raise HTTPException(413, "파일이 너무 큽니다")
    f.write(chunk)
```

동시에 `sha256`을 갱신해 업로드된 내용의 무결성을 검증할 수 있습니다.

### 청크(조각) 업로드

대용량 또는 불안정한 네트워크에서는 파일을 **조각으로 나눠 전송**하고 마지막에 병합합니다.

1. `POST /chunked/start` → `upload_id` 발급
2. `POST /chunked/{id}/parts/{index}` → 조각별 저장
3. `POST /chunked/{id}/complete` → 조각을 정렬해 병합

```python
parts = sorted(session.glob("part_*"))   # part_000001, part_000002 ...
if len(parts) != total_chunks:
    raise HTTPException(409, "조각 불일치")
```

### 스트리밍 다운로드

`StreamingResponse`에 제너레이터를 넘기면 파일 전체를 버퍼에 올리지 않고 클라이언트로 흘려보냅니다. 웹서버(Nginx 등)도 뒤에서 버퍼링을 담당합니다.

```python
def iter_chunks():
    with open(path, "rb") as f:
        while chunk := f.read(CHUNK_SIZE):
            yield chunk

return StreamingResponse(iter_chunks(), media_type="application/octet-stream")
```

## 연습

1. 업로드 시 MIME 타입(`file.content_type`)이 화이트리스트인지도 검사해 보세요.
2. 재개(resume)를 지원하려면 조각 전송 API에 어떤 정보가 추가로 필요할지 설계해 보세요.
