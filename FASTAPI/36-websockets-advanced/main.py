import time

from fastapi import FastAPI, HTTPException, Query, WebSocket, WebSocketDisconnect
from pydantic import BaseModel

app = FastAPI(title="WebSocket 고급 - 룸/인증/재연결")

# 데모용 토큰 -> 사용자 매핑 (실제로는 JWT 검증)
TOKENS = {"tok-alice": "alice", "tok-bob": "bob", "tok-admin": "admin"}


class RoomManager:
    """룸 단위로 소켓과 메시지를 관리"""

    def __init__(self):
        self.rooms: dict[str, dict[str, WebSocket]] = {}
        self.history: dict[str, list[dict]] = {}

    async def connect(self, room: str, username: str, ws: WebSocket):
        await ws.accept()
        self.rooms.setdefault(room, {})[username] = ws
        await self.broadcast(room, "join", username, f"{username}님이 입장했습니다")

    def disconnect(self, room: str, username: str):
        self.rooms.get(room, {}).pop(username, None)
        if room in self.rooms and not self.rooms[room]:
            self.rooms.pop(room, None)

    def add_history(self, room: str, message: dict):
        self.history.setdefault(room, []).append(message)
        if len(self.history[room]) > 100:
            self.history[room] = self.history[room][-100:]

    async def broadcast(self, room: str, event: str, sender: str, text: str):
        message = {"event": event, "sender": sender, "text": text, "ts": time.time()}
        self.add_history(room, message)
        for name, ws in list(self.rooms.get(room, {}).items()):
            try:
                await ws.send_json(message)
            except Exception:
                # 전송 실패(연결 끊김) 소켓은 정리
                self.rooms.get(room, {}).pop(name, None)


manager = RoomManager()


class MessageIn(BaseModel):
    room: str
    text: str


def auth_user(token: str) -> str:
    """쿼리 파라미터의 토큰으로 사용자 인증"""
    username = TOKENS.get(token)
    if username is None:
        raise HTTPException(status_code=401, detail="유효하지 않은 토큰입니다")
    return username


@app.websocket("/ws/{room}")
async def chat_room(websocket: WebSocket, room: str, token: str = Query(...)):
    """룸 채팅 소켓: ?token=... 로 연결 시점에 인증"""
    username = auth_user(token)  # 인증 실패 시 401로 연결 거부
    await manager.connect(room, username, websocket)
    try:
        while True:
            data = await websocket.receive_json()
            text = str(data.get("text", ""))
            if text:
                await manager.broadcast(room, "message", username, text)
    except WebSocketDisconnect:
        # 연결이 끊기면 룸에서 제거하고 나간 사실을 브로드캐스트
        manager.disconnect(room, username)
        await manager.broadcast(room, "leave", username, f"{username}님이 나갔습니다")


# ---- REST 보조 엔드포인트 ----

@app.get("/rooms")
def list_rooms():
    """활성 룸과 참여자 목록"""
    return {room: list(members) for room, members in manager.rooms.items()}


@app.get("/rooms/{room}/history")
def room_history(room: str):
    """룸 메시지 기록 (재연결 시 유실분 복구용)"""
    return manager.history.get(room, [])


@app.post("/rooms/{room}/messages")
async def push_message(room: str, data: MessageIn):
    """REST로 룸에 메시지 푸시 (재연결 클라이언트가 마지막 메시지를 보내는 패턴)"""
    await manager.broadcast(room, "rest", "system", data.text)
    return {"room": room, "sent": True}
