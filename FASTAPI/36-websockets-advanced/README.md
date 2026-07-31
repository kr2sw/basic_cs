# 36: WebSocket 고급 — 룸, 인증, 재연결

기초 챕터 14에서 단순 채팅을 다뤘습니다. 이번에는 실제 서비스에서 필요한 **룸(방) 관리**, **연결 시 인증**, **연결 끊김/재연결** 처리 패턴을 구현합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

브라우저 2개에서 접속해 보세요.

```js
const ws = new WebSocket("ws://localhost:8000/ws/lobby?token=tok-alice");
ws.onmessage = (e) => console.log(JSON.parse(e.data));
ws.send(JSON.stringify({ text: "안녕하세요!" }));
```

사용 가능한 토큰: `tok-alice`, `tok-bob`, `tok-admin`.

## 주요 개념

### 룸(Room) 관리

채팅방/게임룸처럼 여러 사용자를 그룹으로 묶어 메시지를 브로드캐스트합니다. `RoomManager`가 룸별 소켓 목록과 메시지 기록을 관리합니다.

```python
class RoomManager:
    def __init__(self):
        self.rooms: dict[str, dict[str, WebSocket]] = {}
        self.history: dict[str, list[dict]] = {}

    async def broadcast(self, room, event, sender, text):
        message = {"event": event, "sender": sender, "text": text, "ts": time.time()}
        for ws in self.rooms.get(room, {}).values():
            await ws.send_json(message)
```

연결이 끊긴 소켓은 `send_json` 실패 시 목록에서 정리합니다.

### 연결 시점 인증

HTTP처럼 인증 헤더를 쓰기 어렵기 때문에, **쿼리 파라미터에 토큰**을 넣어 연결 시점에 검증합니다. 인증 실패 시 `WebSocketDisconnect`를 던지면 연결이 거부됩니다.

```python
@app.websocket("/ws/{room}")
async def chat_room(websocket: WebSocket, room: str, token: str = Query(...)):
    username = auth_user(token)      # 401이면 연결 거부
    await manager.connect(room, username, websocket)
```

보안상 토큰이 URL에 노출되므로 `wss://`(TLS) 사용이 권장됩니다. 공용 네트워크에선 쿠키 기반 검증을 검토합니다.

### 연결 끊김과 재연결

네트워크 불안정으로 연결이 끊겨도 메시지가 유실되지 않도록:

1. 서버가 메시지 기록(`history`)을 룸별로 보관합니다.
2. 클라이언트는 재연결 시 `?last_ts=`를 보내 **놓친 메시지를 다시 받습니다**.
3. `GET /rooms/{room}/history`로 언제든 복구 가능합니다.

```python
@app.get("/rooms/{room}/history")
def room_history(room: str):
    return manager.history.get(room, [])
```

### 하트비트(Heartbeat)

`ws://` 연결이 조용히 끊기는 경우가 많습니다. 서버가 주기적으로 ping을 보내거나, 클라이언트가 주기적으로 `{"type": "ping"}`을 보내 연결 상태를 확인합니다. 응답이 없으면 재연결을 시도합니다.

## 연습

1. `GET /rooms`로 활성 룸을 확인한 뒤, 두 사용자가 같은 룸에서 채팅을 주고받아 보세요.
2. `last_ts` 파라미터로 "놓친 메시지만 다시 받기"를 구현해 보세요.
