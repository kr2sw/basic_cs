# 37: 실시간 통신 — WebSocket Chat UI & Socket.IO

WebSocket으로 서버와 양방향 통신하는 채팅 UI를 만들고, Socket.IO 기초를 배웁니다.

## WebSocket 개요

HTTP는 요청/응답 단방향이지만 WebSocket은 **한 번 연결된 뒤 양방향**으로 데이터를 주고받습니다. 채팅, 알림, 협업 편집기 등 실시간 기능의 기반입니다.

```js
const ws = new WebSocket('ws://localhost:3000')

ws.onopen = () => ws.send(JSON.stringify({ type: 'message', text: '안녕' }))
ws.onmessage = event => console.log(JSON.parse(event.data))
ws.onclose = () => console.log('연결 종료')
```

## React에서 WebSocket 관리

컴포넌트가 언마운트될 때 연결을 닫도록 `useEffect` cleanup에서 처리합니다. 메시지 수신은 상태로 쌓아 렌더링합니다.

```jsx
useEffect(() => {
  const ws = new WebSocket(url)
  ws.onmessage = e => setMessages(m => [...m, JSON.parse(e.data)])
  return () => ws.close()   // cleanup: 연결 정리
}, [url])
```

## Socket.IO 기초

Socket.IO는 WebSocket 위의 라이브러리로, 재연결·룸·브로드캐스트·이벤트 이름을 제공합니다. 채팅에서는 서버가 클라이언트로 메시지를 브로드캐스트합니다.

```js
// 서버 (server.js)
const { Server } = require('socket.io')
const io = new Server(3000)
io.on('connection', socket => {
  socket.on('chat', msg => io.emit('chat', msg))   // 모든 클라이언트에게 전달
})

// 클라이언트
import { io } from 'socket.io-client'
const socket = io('http://localhost:3000')
socket.emit('chat', '안녕!')
socket.on('chat', msg => console.log(msg))
```

## 실행

```bash
# 이 예제 App.jsx는 기본적으로 "로컬 데모 모드"로 동작합니다.
# 진짜 서버와 통신하려면 URL을 입력하세요. Socket.IO 사용 시:
npm install socket.io-client && npm run dev
```
