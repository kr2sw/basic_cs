// TypeScript + Node 타입 안전 서버 구조 예제
// 예제는 실행 가능하도록 JSDoc 타입 주석으로 구현했습니다.
// 같은 코드를 index.ts로 바꾸고 strict 모드로 컴파일하면 타입 오류가 실행 전에 잡힙니다.

/**
 * 사용자 타입 (TypeScript interface 대체)
 * @typedef {Object} User
 * @property {number} id
 * @property {string} name
 * @property {string} email
 * @property {'admin' | 'editor' | 'viewer'} role
 */

/**
 * 게시글 타입
 * @typedef {Object} Post
 * @property {number} id
 * @property {string} title
 * @property {number} authorId
 */

// ---------- 1. 제네릭 Repository (Repository<T> 유사) ----------
/**
 * @template T
 */
class Repository {
  constructor() {
    /** @type {Map<number, T>} */
    this.items = new Map();
    this.nextId = 1;
  }

  /**
   * @param {T} entity
   * @returns {T & { id: number }}
   */
  create(entity) {
    /** @type {T & { id: number }} */
    const saved = { ...entity, id: this.nextId++ };
    this.items.set(saved.id, saved);
    return saved;
  }

  /**
   * @param {number} id
   * @returns {(T & { id: number }) | null}
   */
  findById(id) {
    return this.items.get(id) ?? null;
  }

  /**
   * @returns {(T & { id: number })[]}
   */
  findAll() {
    return [...this.items.values()];
  }

  /**
   * @param {number} id
   * @returns {boolean}
   */
  remove(id) {
    return this.items.delete(id);
  }
}

// ---------- 2. 타입 가드 (외부 데이터 검증) ----------
/**
 * @param {unknown} value
 * @returns {value is User}
 */
function isUser(value) {
  if (!value || typeof value !== 'object') return false;
  /** @type {Record<string, unknown>} */
  const u = value;
  return (
    typeof u.id === 'number' &&
    typeof u.name === 'string' &&
    typeof u.email === 'string' &&
    ['admin', 'editor', 'viewer'].includes(u.role)
  );
}

// ---------- 3. 타입 안전 서비스 레이어 ----------
class UserService {
  constructor() {
    /** @type {Repository<User>} */
    this.users = new Repository();
  }

  /**
   * @param {string} name
   * @param {string} email
   * @returns {User & { id: number }}
   */
  register(name, email) {
    const user = this.users.create({ name, email, role: 'viewer' });
    console.log(`사용자 등록: #${user.id} ${user.name} (${user.email})`);
    return user;
  }

  /**
   * @param {number} id
   * @returns {(User & { id: number }) | null}
   */
  find(id) {
    return this.users.findById(id);
  }
}

// ---------- 4. 외부 데이터(JSON)를 타입 안전하게 파싱 ----------
/**
 * @param {string} json
 * @returns {User & { id: number }}
 */
function parseUserJson(json) {
  /** @type {unknown} */
  const parsed = JSON.parse(json);
  if (!isUser(parsed)) {
    throw new Error('유효하지 않은 사용자 데이터입니다');
  }
  return parsed; // 타입 가드를 통과했으므로 User로 안전하게 사용 가능
}

// ---------- 데모 ----------
console.log('=== 제네릭 Repository ===');
const userRepo = new Repository();
userRepo.create({ name: '홍길동', email: 'hong@example.com', role: 'admin' });
userRepo.create({ name: '김철수', email: 'kim@example.com', role: 'editor' });
console.log(userRepo.findAll());

const postRepo = new Repository();
postRepo.create({ title: 'TypeScript 입문', authorId: 1 });
postRepo.create({ title: '제네릭 활용', authorId: 1 });
console.log('작성자의 첫 게시글:', postRepo.findById(1));

console.log('\n=== 서비스 레이어 ===');
const userService = new UserService();
const newUser = userService.register('이영희', 'lee@example.com');
const found = userService.find(newUser.id);
console.log('조회된 사용자:', found);

console.log('\n=== 타입 가드 ===');
try {
  const valid = parseUserJson('{"id":9,"name":"박철수","email":"park@example.com","role":"viewer"}');
  console.log('유효한 데이터 통과:', valid);
} catch (err) {
  console.log('오류:', err.message);
}

try {
  parseUserJson('{"id":10,"name":"해커","email":"hack@example.com","role":"superuser"}');
} catch (err) {
  console.log('잘못된 role 데이터 거부:', err.message);
}

try {
  parseUserJson('"그냥 문자열"');
} catch (err) {
  console.log('객체가 아닌 데이터 거부:', err.message);
}
