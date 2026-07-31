# 22: 이벤트 기반 아키텍처 — EventEmitter and Custom Events

EventEmitter를 활용한 이벤트 기반 아키텍처를 학습합니다.

## EventEmitter 기본

Node.js의 `events` 모듈은 이벤트를 등록(`on`)하고 발생(`emit`)시킬 수 있습니다.

```js
const { EventEmitter } = require('events');
const emitter = new EventEmitter();

emitter.on('greet', (name) => {
  console.log(`안녕하세요, ${name}님!`);
});

emitter.emit('greet', '홍길동');
```

## 커스텀 이벤트 클래스

EventEmitter를 상속하면 도메인 이벤트를 정의할 수 있습니다.

```js
class OrderService extends EventEmitter {
  createOrder(items) {
    const order = { id: 1, items };
    this.emit('order:created', order);
    return order;
  }
}

const orders = new OrderService();
orders.on('order:created', (order) => {
  console.log(`주문 #${order.id} 생성됨`);
});
```

이벤트 이름은 `order:created`처럼 콜론으로 계층을 표현하는 것이 관례입니다.

## once

`once`로 등록하면 이벤트가 한 번만 실행되고 자동으로 제거됩니다.

```js
emitter.once('tick', () => {
  console.log('한 번만 실행됩니다');
});

emitter.emit('tick');
emitter.emit('tick'); // 무시됨
```

## error 이벤트

`error` 이벤트에 리스너가 없으면 예외가 발생하므로 반드시 처리합니다.

```js
emitter.on('error', (err) => {
  console.error('오류 발생:', err.message);
});
```

## 리스너 관리 유틸리티

- `emitter.listenerCount('event')`: 리스너 개수
- `emitter.eventNames()`: 등록된 이벤트 이름
- `emitter.removeAllListeners('event')`: 리스너 제거
- `emitter.setMaxListeners(n)`: 리스너 개수 제한 설정

## 예제 실행

```bash
node index.js
```
