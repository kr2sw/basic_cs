# 37: SSE — 이벤트 스트리밍, 실시간 푸시

서버 -> 클라이언트 방향으로 **지속적으로 이벤트를 푸시**해야 하는 기능(알림, 주문 상태, 주가, 로그)에 적합한 기술이 **SSE(Server-Sent Events)**입니다. WebSocket과 달리 HTTP 위에서 동작하며 서버 단방향 푸시에 특화되어 있습니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

SSE 연결 (curl):

```bash
curl -N http://localhost:8000/events
```

다른 터미널에서 이벤트 발행:

```bash
curl -X POST http://localhost:8000/events/publish \
  -H "Content-Type: application/json" \
  -d '{"type":"notification","data":{"message":"새 주문이 들어왔습니다"}}'
```

## 주요 개념

### SSE 프로토콜

`text/event-stream` 미디어 타입으로, 이벤트를 `data:` 필드에 줄 단위로 보냅니다. `event:`로 이벤트 이름, `id:`로 식별자를 지정할 수 있습니다.

```
event: notification
data: {"message": "새 주문이 들어왔습니다"}

```

### 구현: StreamingResponse + 제너레이터

`StreamingResponse`에 **비동기 제너레이터**를 넘기면 연결이 열린 상태로 계속 데이터를 보냅니다. 클라이언트별 전용 큐로 구독을 관리합니다.

```python
async def event_stream():
    queue = asyncio.Queue()
    subscribers.add(queue)
    try:
        while True:
            event = await asyncio.wait_for(queue.get(), timeout=15)
            yield f"event: {event['type']}\n"
            yield f"data: {json.dumps(event['data']) }\n\n"
    finally:
        subscribers.discard(queue)
```

- **keep-alive**: 이벤트가 없을 때 `: keep-alive\n\n` 주석 줄을 보내 연결이 타임아웃되지 않게 합니다.
- **연결 종료**: 클라이언트가 끊으면 제너레이터의 `finally`에서 구독을 해제합니다.

### SSE vs WebSocket

| 구분 | SSE | WebSocket |
|------|-----|-----------|
| 방향 | 서버 -> 클라이언트 단방향 | 양방향 |
| 프로토콜 | HTTP (간단) | WS 핸드셰이크 |
| 자동 재연결 | 브라우저가 `EventSource`로 자동 처리 | 직접 구현 |
| 적합한 용도 | 알림/피드/상태 푸시 | 채팅/게임/협업 |

단방향 푸시가 목적이면 SSE가 훨씬 단순하고 안정적입니다. Nginx 뒤에서는 `X-Accel-Buffering: no` 헤더로 버퍼링을 꺼야 실시간으로 전달됩니다.

### 클라이언트 (브라우저)

```js
const es = new EventSource("/events");
es.addEventListener("notification", (e) => console.log(JSON.parse(e.data)));
```

연결이 끊기면 `EventSource`가 자동으로 재연결하며, 서버가 보낸 `id:`를 함께 보내 유실 이벤트 복구를 지원합니다.

## 연습

1. `data:` 대신 `event:`와 `id:` 필드도 함께 내려보내고 브라우저에서 확인해 보세요.
2. `GET /count`로 현재 구독자 수가 발행/해제에 따라 변하는지 확인해 보세요.
