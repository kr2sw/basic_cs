# 14: WebSocket — 실시간 통신

## 실행

```bash
uvicorn main:app --reload
```

http://127.0.0.1:8000 - WebSocket 채팅 클라이언트

## 주요 개념

- **@app.websocket()**: WebSocket 엔드포인트
- **WebSocket.accept()**: 연결 수락
- **WebSocket.receive_text()**: 메시지 수신
- **WebSocket.send_text()**: 메시지 전송
- **연결 관리**: 여러 클라이언트 동시 처리
