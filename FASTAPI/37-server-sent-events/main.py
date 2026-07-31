import asyncio
import json

from fastapi import FastAPI, Request
from fastapi.responses import StreamingResponse
from pydantic import BaseModel

app = FastAPI(title="SSE - Server-Sent Events")

# 이벤트를 받을 구독자(클라이언트별 큐) 목록
subscribers: set[asyncio.Queue] = set()


async def broadcast(event_type: str, data: dict):
    """모든 구독자에게 이벤트 전파"""
    for q in list(subscribers):
        await q.put({"type": event_type, "data": data})


async def event_stream():
    """SSE 스트림: 각 클라이언트가 전용 큐에서 이벤트를 받는다"""
    queue: asyncio.Queue = asyncio.Queue()
    subscribers.add(queue)
    try:
        while True:
            try:
                # 15초 안에 이벤트가 없으면 keep-alive 주석 전송
                event = await asyncio.wait_for(queue.get(), timeout=15)
                yield f"event: {event['type']}\n"
                yield f"data: {json.dumps(event['data'], ensure_ascii=False)}\n\n"
            except asyncio.TimeoutError:
                yield ": keep-alive\n\n"  # 주석 줄은 무시되며 연결 유지용
    finally:
        subscribers.discard(queue)  # 연결 종료 시 구독 해제


@app.get("/events")
async def events():
    """SSE 엔드포인트: text/event-stream으로 실시간 이벤트 전송"""
    return StreamingResponse(
        event_stream(),
        media_type="text/event-stream",
        headers={
            "Cache-Control": "no-cache",
            "Connection": "keep-alive",
            "X-Accel-Buffering": "no",  # Nginx 버퍼링 비활성화
        },
    )


class EventPayload(BaseModel):
    type: str = "notification"
    data: dict = {}


@app.post("/events/publish")
async def publish(payload: EventPayload):
    """REST로 이벤트 발행 -> 연결된 모든 클라이언트로 푸시"""
    await broadcast(payload.type, payload.data)
    return {"status": "published", "subscribers": len(subscribers)}


@app.get("/count")
async def count():
    return {"subscribers": len(subscribers)}
