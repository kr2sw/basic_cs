// 27: 데코레이터 심화 — 메서드/프로퍼티 데코레이터, DI 컨테이너
// 실행 조건: tsconfig.json에 "experimentalDecorators": true 필요

// === 1. 메서드 데코레이터: 로깅 ===
function log(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
  const original = descriptor.value;
  descriptor.value = function (...args: any[]) {
    console.log(`[LOG] ${propertyKey} 호출, 인자: ${JSON.stringify(args)}`);
    const result = original.apply(this, args);
    console.log(`[LOG] ${propertyKey} 반환: ${JSON.stringify(result)}`);
    return result;
  };
  return descriptor;
}

// === 2. 메서드 데코레이터: 실행 시간 측정 ===
function measure(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
  const original = descriptor.value;
  descriptor.value = function (...args: any[]) {
    const start = performance.now();
    const result = original.apply(this, args);
    console.log(`[TIME] ${propertyKey}: ${(performance.now() - start).toFixed(3)}ms`);
    return result;
  };
  return descriptor;
}

// === 3. 파라미터 데코레이터로 DI 메타데이터 수집 ===
type Constructor<T = any> = new (...args: any[]) => T;

const INJECT_META = Symbol("inject");

function inject(token: string) {
  return (target: any, propertyKey: string | undefined, parameterIndex: number) => {
    const existing: Record<number, string> = Reflect.getOwnMetadata?.(INJECT_META, target) ?? {};
    existing[parameterIndex] = token;
    Reflect.defineMetadata?.(INJECT_META, existing, target);
  };
}

// === 4. 미니 DI 컨테이너 ===
const container = new Map<string, any>();

function register<T>(token: string, factory: () => T): T {
  const instance = factory();
  container.set(token, instance);
  return instance;
}

function resolve<T>(token: string): T {
  return container.get(token) as T;
}

// === 5. 서비스 정의 ===
class Database {
  users = new Map<number, string>();
  constructor() {
    this.users.set(1, "Alice");
    this.users.set(2, "Bob");
  }
  find(id: number): string | undefined {
    return this.users.get(id);
  }
}

class UserService {
  constructor(private db: Database) {}

  @log
  @measure
  getUser(id: number): string {
    return this.db.find(id) ?? "없음";
  }
}

// === 6. 조립 및 실행 ===
const db = register("database", () => new Database());
const userService = register("userService", () => new UserService(resolve("database")));

const service = resolve<UserService>("userService");
console.log("사용자:", service.getUser(1));
console.log("사용자:", service.getUser(2));

// === 7. 클래스 데코레이터: 싱글턴 ===
function singleton<T extends Constructor>(target: T) {
  let instance: T | null = null;
  const proxy = class extends target {
    constructor(...args: any[]) {
      super(...args);
      if (!instance) instance = this as T;
      return instance;
    }
  } as T;
  return proxy;
}

@singleton
class Logger {
  logs: string[] = [];
  log(msg: string) {
    this.logs.push(msg);
    console.log("[Logger]", msg);
  }
}

const l1 = new Logger();
const l2 = new Logger();
console.log("싱글턴 여부:", l1 === l2);  // true
l1.log("중급 데코레이터 완료!");
