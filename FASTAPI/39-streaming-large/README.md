# 39: 대용량 스트리밍 — StreamingResponse, 청크 전송

수십만 행의 CSV, 수 GB의 파일, 실시간 로그를 다룰 때 **전체를 메모리에 올리면 서버가 죽을 수 있습니다.** 이번 챕터는 **조각 단위(chunk)로 생성/전송/수신**하는 스트리밍 패턴을 다룹니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

- `GET /big.csv` — 100,000행 CSV 스트리밍 생성
- `GET /download/big_sample.txt` — 파일 청크 전송
- `POST /stream-upload` — 요청 본문 스트리밍 수신
- `GET /slow` — 0.5초 간격 데이터 흐름

## 주요 개념

### 응답: 제너레이터로 생성

`StreamingResponse`에 제너레이터를 넘기면, 클라이언트가 요청한 만큼만 생성해 전달합니다. 데이터를 배치로 묶어 yield하면 생성-전송-해제가 반복되어 **메모리가 상수**로 유지됩니다.

```python
def csv_generator(rows):
    batch = []
    for i in range(rows):
        batch.append(f"{i},상품{i}\n")
        if len(batch) >= 1000:
            yield "".join(batch)   # 1000행 생성 -> 전송 -> 버퍼 해제
            batch.clear()
```

`json.dumps`를 쓰는 경우도 전체를 한 번에 만들지 말고 `ndjson`(줄 단위 JSON)으로 스트리밍하는 것이 일반적입니다.

### 파일 스트리밍

`open(path, "rb")`에서 `read(CHUNK_SIZE)`만큼 읽어 전송합니다. 전용 버퍼를 쓰지 않으면 파일 전체가 메모리에 들어가므로 반드시 청크 단위로 읽습니다.

```python
def iter_file(path):
    with open(path, "rb") as f:
        while chunk := f.read(1024 * 1024):
            yield chunk
```

### 요청 스트리밍 수신

큰 업로드도 `request.stream()`으로 조각 단위로 받아 처리합니다.

```python
async for chunk in request.stream():
    total += len(chunk)
```

### 스트리밍 시 주의점

- `StreamingResponse`는 **응답 헤더를 먼저 보낸 뒤** 본문을 흘려보내므로, 상태 코드/헤더를 나중에 바꿀 수 없습니다.
- 예외가 발생하면 이미 전송된 내용 뒤에 잘린 응답이 갈 수 있으므로, 헤더 보내기 전 검증을 끝내는 것이 좋습니다.
- Nginx 뒤에서는 `X-Accel-Buffering: no` 또는 버퍼 크기 조정이 필요할 수 있습니다.
- **Backpressure**: `StreamingResponse`는 클라이언트 전송 속도에 맞춰 제너레이터를 천천히 진행하므로 메모리 폭증을 방지합니다.

## 연습

1. `/big.csv`의 `rows`를 1,000,000으로 바꾸고 메모리 사용량이 늘지 않는지 확인해 보세요.
2. 제너레이터에서 예외를 던졌을 때 클라이언트에 어떤 응답이 가는지 확인해 보세요.
