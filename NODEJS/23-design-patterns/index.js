// 디자인 패턴: 싱글턴, 팩토리, 의존성 주입 컨테이너

// ---------- 1. 싱글턴 패턴 ----------
class Database {
  constructor() {
    if (Database.instance) return Database.instance;
    this.tables = new Map();
    Database.instance = this;
  }

  static getInstance() {
    if (!Database.instance) Database.instance = new Database();
    return Database.instance;
  }

  createTable(name) {
    if (!this.tables.has(name)) this.tables.set(name, []);
  }

  insert(name, row) {
    const table = this.tables.get(name);
    if (!table) throw new Error(`테이블이 없습니다: ${name}`);
    table.push(row);
  }

  all(name) {
    return this.tables.get(name) || [];
  }
}

console.log('--- 1. 싱글턴 패턴 ---');
const db1 = Database.getInstance();
const db2 = Database.getInstance();
console.log('동일 인스턴스인가?', db1 === db2);

db1.createTable('users');
db2.insert('users', { id: 1, name: '홍길동' });
console.log('db1에서도 같은 데이터가 보임:', db1.all('users'));

// ---------- 2. 팩토리 패턴 ----------
class EmailNotifier {
  send(message) {
    console.log(`[이메일 발송] ${message}`);
  }
}
class SmsNotifier {
  send(message) {
    console.log(`[문자 발송] ${message}`);
  }
}
class PushNotifier {
  send(message) {
    console.log(`[푸시 발송] ${message}`);
  }
}

class NotifierFactory {
  static create(type) {
    switch (type) {
      case 'email':
        return new EmailNotifier();
      case 'sms':
        return new SmsNotifier();
      case 'push':
        return new PushNotifier();
      default:
        throw new Error(`알 수 없는 알림 타입: ${type}`);
    }
  }
}

console.log('\n--- 2. 팩토리 패턴 ---');
const notifiers = ['email', 'sms', 'push'].map((t) => {
  const notifier = NotifierFactory.create(t);
  notifier.send('주문이 완료되었습니다');
  return notifier;
});

// ---------- 3. 의존성 주입 컨테이너 ----------
class Container {
  constructor() {
    this.services = new Map();
  }

  register(name, factory) {
    this.services.set(name, factory);
  }

  resolve(name) {
    const factory = this.services.get(name);
    if (!factory) throw new Error(`등록된 서비스가 없습니다: ${name}`);
    return factory(this);
  }
}

class Logger {
  log(message) {
    console.log(`[LOG] ${message}`);
  }
}

class UserRepository {
  constructor(db) {
    this.db = db;
  }
  save(user) {
    this.db.insert('users', user);
    return user;
  }
}

class UserService {
  constructor(logger, userRepository) {
    this.logger = logger;
    this.userRepository = userRepository;
  }

  register(name) {
    const user = this.userRepository.save({ name, at: new Date().toISOString() });
    this.logger.log(`사용자 등록: ${name}`);
    return user;
  }
}

console.log('\n--- 3. 의존성 주입 컨테이너 ---');
const container = new Container();

// 컨테이너에 서비스 등록 (의존성은 팩토리에서 해결)
container.register('db', () => Database.getInstance());
container.register('logger', () => new Logger());
container.register('userRepository', (c) => new UserRepository(c.resolve('db')));
container.register('userService', (c) =>
  new UserService(c.resolve('logger'), c.resolve('userRepository'))
);

// 컨테이너에서 서비스 해결 (의존성 자동 주입)
const userService = container.resolve('userService');
userService.register('김철수');
userService.register('이영희');

console.log('등록된 사용자 목록:', db1.all('users'));
