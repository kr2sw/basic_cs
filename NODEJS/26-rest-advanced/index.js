// 고급 REST API: 버저닝, 입력 검증, 통일된 에러 응답을
// Node.js 핵심 http 모듈로 구현한 예제입니다.

const http = require('http');
const crypto = require('crypto');

// ---------- 유틸리티 ----------
function uuid() {
  return crypto.randomUUID();
}

function jsonBody(req) {
  return new Promise((resolve, reject) => {
    let data = '';
    req.on('data', (chunk) => (data += chunk));
    req.on('end', () => {
      try {
        resolve(data ? JSON.parse(data) : {});
      } catch (err) {
        err.statusCode = 400;
        err.message = '요청 본문이 올바른 JSON이 아닙니다';
        reject(err);
      }
    });
    req.on('error', reject);
  });
}

// ---------- 통일된 응답 형식 ----------
function send(res, status, data, error) {
  const body = {
    success: status < 400,
    status,
    timestamp: new Date().toISOString(),
  };
  if (data !== undefined) body.data = data;
  if (error) body.error = error;
  res.writeHead(status, { 'Content-Type': 'application/json; charset=utf-8' });
  res.end(JSON.stringify(body, null, 2));
}

// ---------- 입력 검증 ----------
function validate(data, rules) {
  const errors = [];
  for (const [field, opts] of Object.entries(rules)) {
    const value = data[field];

    if (opts.required && (value === undefined || value === '')) {
      errors.push(`${field} 필드는 필수입니다`);
      continue;
    }
    if (value === undefined) continue;

    if (opts.type === 'string' && typeof value !== 'string') {
      errors.push(`${field} 필드는 문자열이어야 합니다`);
    }
    if (opts.type === 'number') {
      if (typeof value !== 'number' || Number.isNaN(value)) {
        errors.push(`${field} 필드는 숫자여야 합니다`);
      }
    }
    if (opts.minLength && String(value).length < opts.minLength) {
      errors.push(`${field} 필드는 최소 ${opts.minLength}자 이상이어야 합니다`);
    }
    if (opts.min && value < opts.min) {
      errors.push(`${field} 필드는 ${opts.min} 이상이어야 합니다`);
    }
    if (opts.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      errors.push(`${field} 형식이 올바르지 않습니다`);
    }
    if (opts.enum && !opts.enum.includes(value)) {
      errors.push(`${field} 필드는 ${opts.enum.join(', ')} 중 하나여야 합니다`);
    }
  }
  return errors;
}

// ---------- 미니 라우터 ----------
function createApp() {
  const routes = [];
  const add = (method, path, handler) =>
    routes.push({ method, path: compile(path), handler });
  const app = {
    get: (p, h) => add('GET', p, h),
    post: (p, h) => add('POST', p, h),
    put: (p, h) => add('PUT', p, h),
    delete: (p, h) => add('DELETE', p, h),
    serve(req, res) {
      const url = new URL(req.url, 'http://localhost');
      const pathname = url.pathname;
      const route = routes.find(
        (r) => r.method === req.method && r.re.test(pathname)
      );
      if (!route) return send(res, 404, undefined, '요청한 리소스를 찾을 수 없습니다');
      const match = pathname.match(route.re);
      const params = {};
      route.names.forEach((name, i) => (params[name] = decodeURIComponent(match[i + 1])));
      req.params = params;
      req.query = Object.fromEntries(url.searchParams.entries());
      const next = (err) => {
        if (err) return send(res, err.statusCode || 500, undefined, err.message);
      };
      try {
        const result = route.handler(req, res, next);
        // async 핸들러의 오류는 catch로 전달
        if (result && typeof result.catch === 'function') {
          result.catch(next);
        }
      } catch (err) {
        next(err);
      }
    },
  };
  return app;
}

function compile(path) {
  const re = path.replace(/:[a-zA-Z]+/g, '([^/]+)');
  const names = [...path.matchAll(/:([a-zA-Z]+)/g)].map((m) => m[1]);
  return { re: new RegExp('^' + re + '$'), names };
}

// ---------- 데이터 저장소 ----------
let users = [
  { id: 'u1', name: '홍길동', email: 'hong@example.com', age: 30, role: 'admin' },
  { id: 'u2', name: '김철수', email: 'kim@example.com', age: 25, role: 'viewer' },
];

const USER_RULES = {
  name: { required: true, type: 'string', minLength: 2 },
  email: { required: true, type: 'string', email: true },
  age: { type: 'number', min: 0 },
  role: { type: 'string', enum: ['admin', 'editor', 'viewer'] },
};

// ---------- v1 라우트 ----------
const v1 = createApp();

// 페이지네이션 적용 목록 조회
v1.get('/users', (req, res) => {
  const page = Number(req.query.page) || 1;
  const limit = Number(req.query.limit) || 10;
  const start = (page - 1) * limit;
  const total = users.length;
  send(res, 200, {
    items: users.slice(start, start + limit),
    page,
    limit,
    total,
    hasMore: start + limit < total,
  });
});

v1.get('/users/:id', (req, res) => {
  const user = users.find((u) => u.id === req.params.id);
  if (!user) return send(res, 404, undefined, '사용자를 찾을 수 없습니다');
  send(res, 200, user);
});

v1.post('/users', async (req, res) => {
  const data = await jsonBody(req);
  const errors = validate(data, USER_RULES);
  if (errors.length) return send(res, 400, { errors }, '입력 검증에 실패했습니다');

  if (users.some((u) => u.email === data.email)) {
    return send(res, 409, undefined, '이미 존재하는 이메일입니다');
  }

  const user = { id: uuid(), ...data };
  users.push(user);
  send(res, 201, user);
});

v1.put('/users/:id', async (req, res) => {
  const user = users.find((u) => u.id === req.params.id);
  if (!user) return send(res, 404, undefined, '사용자를 찾을 수 없습니다');

  const data = await jsonBody(req);
  const errors = validate(data, USER_RULES);
  if (errors.length) return send(res, 400, { errors }, '입력 검증에 실패했습니다');

  Object.assign(user, data);
  send(res, 200, user);
});

v1.delete('/users/:id', (req, res) => {
  const before = users.length;
  users = users.filter((u) => u.id !== req.params.id);
  if (users.length === before) return send(res, 404, undefined, '사용자를 찾을 수 없습니다');
  send(res, 200, { deletedId: req.params.id }); // 멱등성 보장용
});

// ---------- v2 라우트 (확장 예제: 전화번호 필드 추가) ----------
const v2 = createApp();
v2.get('/users', (req, res) => {
  send(res, 200, {
    items: users,
    version: 'v2',
    message: 'v2에서는 프로필 추가 필드를 제공합니다',
  });
});

// ---------- 라우터 디스패치 ----------
const VERSIONS = { '/api/v1': v1, '/api/v2': v2 };

const server = http.createServer((req, res) => {
  const pathname = new URL(req.url, 'http://localhost').pathname;
  const versionPrefix = Object.keys(VERSIONS)
    .sort((a, b) => b.length - a.length)
    .find((prefix) => pathname === prefix || pathname.startsWith(prefix + '/'));

  if (!versionPrefix) {
    return send(res, 404, undefined, '존재하지 않는 API 버전입니다. /api/v1 사용');
  }

  req.url = pathname.slice(versionPrefix.length) || '/';
  VERSIONS[versionPrefix].serve(req, res);
});

const PORT = 3000;
server.listen(PORT, () => {
  console.log(`고급 REST API 서버: http://localhost:${PORT}`);
  console.log('  GET    /api/v1/users            목록 조회 (page, limit)');
  console.log('  GET    /api/v1/users/:id        단건 조회');
  console.log('  POST   /api/v1/users            생성 (검증 적용)');
  console.log('  PUT    /api/v1/users/:id        수정 (검증 적용)');
  console.log('  DELETE /api/v1/users/:id        삭제');
  console.log('  GET    /api/v2/users            v2 예제');
  console.log('\n검증 실패 예시: POST /api/v1/users {"email":"bad"} -> 400');
});
