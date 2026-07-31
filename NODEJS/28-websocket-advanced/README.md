# 28: WebSocket 고급 — Socket.IO Concepts and Realtime Chat

Socket.IO 개념을 이해하고 실시간 채팅 구조를 학습합니다.

## Socket.IO 특징

WebSocket을 기반으로 폴링까지 지원하여 모든 환경에서 실시간 통신이 가능합니다.

```js
// 서버
const io = require('socket.io')(server);

io.on('connection', (socket) => {
  socket.emit('welcome', '환영합니다');
  socket.on('chat message', (msg) => io.emit('chat message', msg));
});

// 클라이언트
const socket = io('http://localhost:3000');
socket.on('chat message', (msg) => console.log(msg));
socket.emit('chat message', '안녕하세요');
```

## 핵심 개념

| 개념 | 설명 |
|------|------|
| **이벤트** | `socket.on()`으로 수신, `socket.emit()`으로 송신 |
| **브로드캐스트** | 연결된 모든 클라이언트에게 전송 |
| **룸 (Room)** | 특정 그룹에게만 메시지 전송 (`socket.join('room')`) |
| **재연결** | 연결이 끊겨도 자동으로 재연결 시도 |

## 룸과 브로드캐스트

```js
socket.join('korean-room');
io.to('korean-room').emit('message', '한국어 방 메시지');
```

## 예제 실행

예제는 Socket.IO 설치 없이 Node 핵심 모듈(events)로 채팅 서버 구조를 시뮬레이션합니다.

```bash
node index.js
```
