// Socket.IO 개념을 Node.js 핵심 모듈(events)로 시뮬레이션한
// 실시간 채팅 구조 예제입니다.

const { EventEmitter } = require('events');

// ---------- 룸 매니저 (io.to(room)) ----------
class RoomManager {
  constructor() {
    this.rooms = new Map(); // roomName -> Set<client>
  }

  join(roomName, client) {
    if (!this.rooms.has(roomName)) this.rooms.set(roomName, new Set());
    this.rooms.get(roomName).add(client);
    client.rooms.add(roomName);
    console.log(`  [룸] ${client.user} 님이 "${roomName}" 방에 입장 (현재 ${this.rooms.get(roomName).size}명)`);
  }

  leave(roomName, client) {
    const room = this.rooms.get(roomName);
    if (room) {
      room.delete(client);
      client.rooms.delete(roomName);
      console.log(`  [룸] ${client.user} 님이 "${roomName}" 방에서 퇴장`);
    }
  }

  // 특정 방에만 메시지 전송 (io.to(room).emit())
  to(roomName) {
    return {
      emit: (payload) => {
        const room = this.rooms.get(roomName);
        if (!room) return;
        for (const client of room) {
          client.send(payload);
        }
      },
    };
  }
}

// ---------- 채팅 서버 (Socket.IO 서버 유사) ----------
class ChatServer extends EventEmitter {
  constructor() {
    super();
    this.clients = new Map();
    this.rooms = new RoomManager();
    this.nextId = 1;
  }

  // 클라이언트 연결 (socket.io의 'connection' 이벤트)
  connect(user) {
    const client = {
      id: this.nextId++,
      user,
      rooms: new Set(),
      send: (payload) => {
        console.log(`  [${user} 받음] ${JSON.stringify(payload)}`);
      },
    };
    this.clients.set(client.id, client);
    this.emit('connection', client);
    return client;
  }

  disconnect(client) {
    this.clients.delete(client.id);
    this.emit('disconnection', client);
  }

  // 모든 클라이언트에게 브로드캐스트 (io.emit())
  broadcast(payload) {
    for (const client of this.clients.values()) {
      client.send(payload);
    }
  }
}

// ---------- 서버 로직 정의 ----------
const server = new ChatServer();

// 연결 이벤트 처리
server.on('connection', (client) => {
  console.log(`[연결] ${client.user} 님이 접속했습니다`);

  // 입장 메시지 브로드캐스트
  server.broadcast({ type: 'system', message: `${client.user} 님이 입장했습니다.` });

  // 클라이언트 메시지 수신 처리 (socket.on(...))
  client.onMessage = (payload) => {
    switch (payload.command) {
      case 'join': {
        server.rooms.join(payload.room, client);
        break;
      }
      case 'chat': {
        const message = { type: 'chat', room: payload.room, user: client.user, text: payload.text, at: new Date().toISOString() };
        server.rooms.to(payload.room).emit(message);
        break;
      }
      default:
        client.send({ type: 'error', message: '알 수 없는 명령입니다' });
    }
  };
});

// ---------- 클라이언트 시뮬레이션 ----------
const alice = server.connect('앨리스');
const bob = server.connect('밥');
const carol = server.connect('캐롤');

// 밥과 캐롤은 "korean" 방에 입장
bob.onMessage({ command: 'join', room: 'korean' });
carol.onMessage({ command: 'join', room: 'korean' });

// 앨리스가 korean 방에서 메시지 전송 (룸 브로드캐스트)
console.log('\n[앨리스 -> korean 방 채팅 전송]');
alice.onMessage({ command: 'chat', room: 'korean', text: '안녕하세요! 한국어 방입니다.' });

// 전체 브로드캐스트 데모
console.log('\n[서버 -> 전체 브로드캐스트]');
server.broadcast({ type: 'system', message: '잠시 후 서버 점검이 있습니다.' });

// 연결 해제 시뮬레이션
console.log('\n[연결 해제]');
server.disconnect(alice);
server.broadcast({ type: 'system', message: '앨리스 님이 퇴장했습니다.' });

console.log('\n(채팅 구조 시뮬레이션 완료)');
