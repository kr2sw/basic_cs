// Redis 개념을 in-memory로 시뮬레이션한 예제입니다.
// 실제 Redis 사용 시: npm install redis
//   const { createClient } = require('redis');
//   const redis = createClient(); await redis.connect();

const { EventEmitter } = require('events');

class RedisSimulator {
  constructor() {
    this.store = new Map(); // key -> { value, expiresAt }
    this.pubsub = new EventEmitter();
  }

  // SET key value [EX seconds]
  set(key, value, ttlSeconds) {
    const expiresAt = ttlSeconds ? Date.now() + ttlSeconds * 1000 : null;
    this.store.set(key, { value, expiresAt });
    return 'OK';
  }

  // GET key (만료 시 null 반환)
  get(key) {
    const entry = this.store.get(key);
    if (!entry) return null;
    if (entry.expiresAt && entry.expiresAt < Date.now()) {
      this.store.delete(key);
      return null;
    }
    return entry.value;
  }

  del(...keys) {
    let count = 0;
    for (const key of keys) {
      if (this.store.delete(key)) count += 1;
    }
    return count;
  }

  exists(key) {
    return this.get(key) !== null;
  }

  // TTL key (남은 초, 없으면 -2, 만료 없으면 -1)
  ttl(key) {
    const entry = this.store.get(key);
    if (!entry) return -2;
    if (!entry.expiresAt) return -1;
    return Math.max(0, Math.round((entry.expiresAt - Date.now()) / 1000));
  }

  keys(pattern) {
    const re = new RegExp('^' + pattern.replace(/\*/g, '.*') + '$');
    return [...this.store.keys()].filter((k) => re.test(k));
  }

  // pub/sub
  publish(channel, message) {
    const msg = typeof message === 'string' ? message : JSON.stringify(message);
    this.pubsub.emit(channel, msg);
    return this.pubsub.listenerCount(channel); // 수신자 수 반환
  }

  subscribe(channel, handler) {
    this.pubsub.on(channel, handler);
  }
}

const redis = new RedisSimulator();

// ---------- 1. 기본 명령어 ----------
console.log('=== 1. 기본 명령어 ===');
redis.set('user:1', '홍길동', 5); // 5초 만료
console.log('SET user:1 ->', redis.get('user:1'));
console.log('TTL user:1 ->', redis.ttl('user:1'), '초');
console.log('EXISTS user:1 ->', redis.exists('user:1'));
console.log('DEL user:1 ->', redis.del('user:1'), '개 삭제');
console.log('DEL 이후 GET ->', redis.get('user:1'));

// ---------- 2. 만료 처리 ----------
console.log('\n=== 2. 만료 처리 ===');
redis.set('session:abc', '토큰 데이터', 1);
console.log('1초 후 값:', redis.get('session:abc'));
setTimeout(() => {
  console.log('2초 뒤 값:', redis.get('session:abc'), '(만료되어 null)');
  console.log('TTL(만료 키):', redis.ttl('session:abc'), '(없으면 -2)');
}, 1200);

// ---------- 3. Cache-Aside 패턴 ----------
console.log('\n=== 3. Cache-Aside 패턴 ===');

// 실제 DB 대신 느린 조회 함수 시뮬레이션
const database = {
  users: [
    { id: 1, name: '홍길동', age: 30 },
    { id: 2, name: '김철수', age: 25 },
  ],
};

function slowDbFindUser(id) {
  // DB 조회는 느리다고 가정 (300ms)
  const start = Date.now();
  while (Date.now() - start < 300) {}
  return database.users.find((u) => u.id === id);
}

async function getUser(id) {
  const cacheKey = `user:${id}`;
  const cached = redis.get(cacheKey);
  if (cached) {
    console.log(`[캐시 히트] user:${id}`);
    return JSON.parse(cached);
  }
  console.log(`[캐시 미스] user:${id} -> DB 조회`);
  const user = slowDbFindUser(id);
  if (user) redis.set(cacheKey, JSON.stringify(user), 10);
  return user;
}

(async () => {
  const start = Date.now();
  await getUser(1); // 미스
  console.log(`첫 조회 소요: ${Date.now() - start}ms\n`);

  const start2 = Date.now();
  const cached = await getUser(1); // 히트 (즉시)
  console.log(`캐시 조회 소요: ${Date.now() - start2}ms`);
  console.log('캐시 데이터:', cached);

  // ---------- 4. Pub/Sub ----------
  console.log('\n=== 4. Pub/Sub ===');
  redis.subscribe('order:created', (message) => {
    console.log(`[구독자 A 수신] ${message}`);
  });
  redis.subscribe('order:created', (message) => {
    console.log(`[구독자 B 수신] ${message}`);
  });

  const receivers = redis.publish('order:created', { id: 100, product: '노트북' });
  console.log('수신자 수:', receivers);

  // ---------- 5. 키 패턴 조회 ----------
  redis.set('user:1', 'a');
  redis.set('user:2', 'b');
  redis.set('order:5', 'c');
  console.log('\n=== 5. 키 패턴 조회 ===');
  console.log('user:* ->', redis.keys('user:*'));

  setTimeout(() => console.log('\n(테스트 완료)'), 1500);
})();
