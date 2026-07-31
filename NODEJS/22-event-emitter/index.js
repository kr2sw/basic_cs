// 이벤트 기반 아키텍처: EventEmitter, 커스텀 이벤트, once, error 이벤트

const { EventEmitter } = require('events');

// ---------- 1. 기본 EventEmitter ----------
const emitter = new EventEmitter();

emitter.on('greet', (name) => {
  console.log(`[기본] 안녕하세요, ${name}님!`);
});

emitter.emit('greet', '홍길동');

// ---------- 2. 커스텀 이벤트 클래스 ----------
class OrderService extends EventEmitter {
  constructor() {
    super();
    this.orders = [];
  }

  createOrder(items) {
    const order = {
      id: this.orders.length + 1,
      items,
      status: 'created',
      createdAt: new Date().toISOString(),
    };
    this.orders.push(order);

    this.emit('order:created', order);
    this.emit('notification:send', order);

    // 결제는 잠시 후 처리된다고 가정
    setTimeout(() => {
      order.status = 'paid';
      this.emit('order:paid', order);
    }, 300);

    return order;
  }
}

console.log('\n--- 주문 이벤트 ---');
const orders = new OrderService();

orders.on('order:created', (order) => {
  console.log(`[주문 생성] 주문 #${order.id} 생성됨 (${order.items.join(', ')})`);
});

orders.on('notification:send', (order) => {
  console.log(`[알림] 주문 #${order.id} 알림을 전송합니다`);
});

orders.on('order:paid', (order) => {
  console.log(`[결제 완료] 주문 #${order.id} 결제 완료`);
});

orders.createOrder(['노트북', '무선 마우스']);

// ---------- 3. once: 한 번만 실행 ----------
console.log('\n--- once ---');
const onceEmitter = new EventEmitter();
let count = 0;

onceEmitter.once('tick', () => {
  count += 1;
  console.log(`tick 이벤트 실행 횟수: ${count}`);
});

onceEmitter.emit('tick'); // 실행됨
onceEmitter.emit('tick'); // 무시됨
console.log('tick 리스너 수:', onceEmitter.listenerCount('tick'));

// ---------- 4. error 이벤트 ----------
console.log('\n--- error 이벤트 ---');
const risky = new EventEmitter();
risky.on('error', (err) => {
  console.error(`[error 이벤트 처리] ${err.message}`);
});
risky.emit('error', new Error('의도적으로 발생시킨 오류'));

// ---------- 5. 리스너 관리 유틸리티 ----------
console.log('\n--- 리스너 관리 ---');
console.log('greet 리스너 수:', emitter.listenerCount('greet'));
console.log('order:created 리스너 수:', orders.listenerCount('order:created'));
console.log('등록된 이벤트:', orders.eventNames().join(', '));

emitter.removeAllListeners('greet');
console.log('removeAllListeners 후 greet 리스너 수:', emitter.listenerCount('greet'));

// ---------- 6. 이벤트 기반 아키텍처 데모 ----------
console.log('\n--- 이벤트 기반 알림 시스템 ---');

class NotificationHub extends EventEmitter {
  send(type, payload) {
    this.emit(`notification:${type}`, payload);
  }
}

const hub = new NotificationHub();
hub.on('notification:email', (payload) => {
  console.log(`[이메일 발송] to=${payload.to} 제목="${payload.subject}"`);
});
hub.on('notification:sms', (payload) => {
  console.log(`[문자 발송] to=${payload.phone} 내용="${payload.message}"`);
});

hub.send('email', { to: 'hong@example.com', subject: '환영합니다' });
hub.send('sms', { phone: '010-1234-5678', message: '주문이 접수되었습니다' });

// 1초 후 주문 이벤트 결과가 표시되도록 대기
setTimeout(() => {
  console.log('\n(모든 이벤트 처리 완료)');
}, 500);
