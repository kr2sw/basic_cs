# 23: 디자인 패턴 — Singleton, Factory, Dependency Injection Container

Node.js에서 자주 사용하는 디자인 패턴을 학습합니다.

## 싱글턴 (Singleton)

전체 프로세스에서 인스턴스가 하나만 존재하도록 보장합니다.

```js
class Database {
  constructor() {
    if (Database.instance) return Database.instance;
    Database.instance = this;
  }
  static getInstance() {
    if (!Database.instance) Database.instance = new Database();
    return Database.instance;
  }
}

const db1 = Database.getInstance();
const db2 = Database.getInstance();
console.log(db1 === db2); // true
```

Node.js에서는 모듈 캐싱 덕분에 `module.exports = new Config()` 형태도 싱글턴이 됩니다.

## 팩토리 (Factory)

객체 생성 로직을 캡슐화하여 타입에 따라 다른 인스턴스를 반환합니다.

```js
class NotifierFactory {
  static create(type) {
    switch (type) {
      case 'email': return new EmailNotifier();
      case 'sms': return new SmsNotifier();
      default: throw new Error('알 수 없는 타입');
    }
  }
}
```

## 의존성 주입 컨테이너 (DI Container)

객체가 필요한 의존성을 스스로 만들지 않고 컨테이너가 주입해 줍니다. 테스트와 유지보수가 쉬워집니다.

```js
const container = new Container();
container.register('logger', () => new Logger());
container.register('userService', (c) =>
  new UserService(c.resolve('logger')));

const service = container.resolve('userService');
```

## 패턴 선택 기준

| 패턴 | 사용 시점 |
|------|-----------|
| 싱글턴 | DB 커넥션, 설정(config)처럼 전역 상태가 하나여야 할 때 |
| 팩토리 | 생성 로직이 복잡하거나 타입별로 다른 객체가 필요할 때 |
| DI 컨테이너 | 규모가 큰 앱에서 의존성을 중앙 관리해야 할 때 |

## 예제 실행

```bash
node index.js
```
