# 14. WebSocket

실시간 양방향 통신을 위한 WebSocket을 학습합니다.

## WebSocket vs HTTP

- **HTTP**: 클라이언트가 요청해야 서버가 응답 (단방향)
- **WebSocket**: 연결이 유지되며 서버가 먼저 데이터를 보낼 수 있음 (양방향)

## ws 라이브러리

Node.js에서 가장 널리 사용되는 WebSocket 라이브러리입니다.

### 설치

```bash
npm install ws
```

## WebSocket 서버

```js
const { WebSocketServer } = require('ws');
const wss = new WebSocketServer({ port: 8080 });

wss.on('connection', (ws) => {
  ws.on('message', (data) => {
    // 모든 클라이언트에게 브로드캐스트
    wss.clients.forEach(client => {
      if (client.readyState === WebSocket.OPEN) {
        client.send(data.toString());
      }
    });
  });
});
```

## WebSocket 클라이언트 (브라우저)

```js
const ws = new WebSocket('ws://localhost:8080');
ws.onmessage = (event) => console.log(event.data);
ws.send('Hello!');
```

## 채팅 예제

서버는 메시지를 받아 모든 연결된 클라이언트에게 브로드캐스트합니다.

## 예제 실행

서버와 클라이언트를 각각 실행합니다.

```bash
node server.js
node client.js
```
