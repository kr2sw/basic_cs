// 마이크로서비스: 서비스 분리, HTTP 통신, 헬스 체크 예제
// user-service(3001) + order-service(3002)

const http = require('http');

// ---------- 유틸리티 ----------
function json(res, status, body) {
  res.writeHead(status, { 'Content-Type': 'application/json; charset=utf-8' });
  res.end(JSON.stringify(body, null, 2));
}

function requestJson(url) {
  return new Promise((resolve, reject) => {
    http
      .get(url, (res) => {
        let data = '';
        res.on('data', (c) => (data += c));
        res.on('end', () => {
          try {
            resolve({ status: res.statusCode, body: JSON.parse(data) });
          } catch (err) {
            reject(err);
          }
        });
      })
      .on('error', reject);
  });
}

function delay(ms) {
  return new Promise((r) => setTimeout(r, ms));
}

// ---------- 서비스 레지스트리 (서비스 디스커버리) ----------
class ServiceRegistry {
  constructor() {
    this.services = new Map();
  }
  register(name, url, port) {
    this.services.set(name, { name, url, port, registeredAt: new Date().toISOString() });
  }
  lookup(name) {
    return this.services.get(name)?.url;
  }
  list() {
    return [...this.services.values()];
  }
}

const registry = new ServiceRegistry();

// 경로 매칭: /users/:id 패턴 지원
function matchPath(path, pathname) {
  if (!path.includes(':')) return path === pathname;
  const re = new RegExp('^' + path.replace(/:[a-zA-Z]+/g, '[^/]+') + '$');
  return re.test(pathname);
}

// ---------- 서비스 생성 헬퍼 ----------
function createService(name, port, routes) {
  const server = http.createServer((req, res) => {
    const url = new URL(req.url, `http://localhost:${port}`);
    const route = routes.find(
      (r) => r.method === req.method && matchPath(r.path, url.pathname)
    );
    if (!route) return json(res, 404, { service: name, error: 'Not Found' });
    route.handler(req, res, url);
  });

  server.listen(port, () => {
    console.log(`[${name}] http://localhost:${port} 에서 실행 중`);
  });
  return server;
}

// ---------- user-service ----------
const users = [
  { id: 1, name: '홍길동', email: 'hong@example.com' },
  { id: 2, name: '김철수', email: 'kim@example.com' },
];

const userService = createService('user-service', 3001, [
  {
    method: 'GET',
    path: '/health',
    handler: (req, res) =>
      json(res, 200, { status: 'ok', service: 'user-service', uptime: process.uptime() }),
  },
  {
    method: 'GET',
    path: '/users',
    handler: (req, res) => json(res, 200, { service: 'user-service', count: users.length, users }),
  },
  {
    method: 'GET',
    path: '/users/:id',
    handler: (req, res, url) => {
      const id = Number(url.pathname.split('/').pop());
      const user = users.find((u) => u.id === id);
      if (!user) return json(res, 404, { service: 'user-service', error: '사용자 없음' });
      json(res, 200, user);
    },
  },
]);

// ---------- order-service ----------
const orders = [
  { id: 101, userId: 1, product: '노트북', amount: 1500000 },
  { id: 102, userId: 1, product: '마우스', amount: 35000 },
  { id: 103, userId: 2, product: '모니터', amount: 320000 },
];

const orderService = createService('order-service', 3002, [
  {
    method: 'GET',
    path: '/health',
    handler: (req, res) =>
      json(res, 200, { status: 'ok', service: 'order-service', uptime: process.uptime() }),
  },
  {
    method: 'GET',
    path: '/orders',
    // 다른 서비스(user-service)와 HTTP로 통신하는 예제
    handler: async (req, res, url) => {
      const userId = Number(url.searchParams.get('userId') || 0);
      const myOrders = orders.filter((o) => o.userId === userId);

      // user-service 호출 (내부 HTTP 통신)
      const userUrl = `${registry.lookup('user-service')}/users/${userId}`;
      const userRes = await requestJson(userUrl);

      const body = {
        service: 'order-service',
        user: userRes.status === 200 ? userRes.body : null,
        orders: myOrders,
        via: userUrl, // 통신 경로 확인용
      };
      json(res, 200, body);
    },
  },
]);

// ---------- 메인 흐름 ----------
async function main() {
  registry.register('user-service', 'http://localhost:3001', 3001);
  registry.register('order-service', 'http://localhost:3002', 3002);

  await delay(300);

  console.log('\n=== 서비스 레지스트리 ===');
  console.table(registry.list());

  console.log('\n=== 주문 서비스가 사용자 서비스를 HTTP로 호출 ===');
  const orderRes = await requestJson('http://localhost:3002/orders?userId=1');
  console.log(JSON.stringify(orderRes.body, null, 2));

  console.log('\n=== 헬스 체크 (모든 서비스) ===');
  for (const svc of registry.list()) {
    const health = await requestJson(`${svc.url}/health`);
    console.log(`  ${svc.name}: ${health.status} ${JSON.stringify(health.body)}`);
  }

  console.log('\n(마이크로서비스 데모 완료, 서버 종료)');
  userService.close();
  orderService.close();
  setTimeout(() => process.exit(0), 200);
}

main().catch((err) => {
  console.error('오류:', err.message);
  process.exit(1);
});
