// 보안: helmet 개념(보안 헤더), rate limiting, 입력 검증, crypto 해싱 예제

const http = require('http');
const crypto = require('crypto');

// ---------- 1. Helmet 유사: 보안 헤더 적용 ----------
function applySecurityHeaders(res) {
  res.setHeader('X-Frame-Options', 'DENY');
  res.setHeader('X-Content-Type-Options', 'nosniff');
  res.setHeader('Referrer-Policy', 'no-referrer');
  res.setHeader('X-XSS-Protection', '0');
  res.setHeader('Content-Security-Policy', "default-src 'self'");
  res.setHeader('Strict-Transport-Security', 'max-age=31536000; includeSubDomains');
}

// ---------- 2. Rate Limiter (슬라이딩 윈도우) ----------
class RateLimiter {
  constructor(max, windowMs) {
    this.max = max;
    this.windowMs = windowMs;
    this.requests = new Map(); // key -> [타임스탬프]
  }

  check(key) {
    const now = Date.now();
    const recent = (this.requests.get(key) || []).filter(
      (t) => now - t < this.windowMs
    );
    if (recent.length >= this.max) {
      this.requests.set(key, recent);
      return false;
    }
    recent.push(now);
    this.requests.set(key, recent);
    return true;
  }
}

const limiter = new RateLimiter(5, 60_000); // 1분에 5회

// ---------- 3. 입력 검증 ----------
function sanitize(value) {
  // HTML 이스케이프로 XSS 방지
  return String(value)
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#39;');
}

const EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

function validateUserInput(input) {
  const errors = [];
  if (!input.name || input.name.length < 2) errors.push('이름은 2자 이상이어야 합니다');
  if (!EMAIL_RE.test(input.email || '')) errors.push('이메일 형식이 올바르지 않습니다');
  if (typeof input.age !== 'number' || input.age < 0 || input.age > 150) {
    errors.push('나이는 0~150 사이 숫자여야 합니다');
  }
  // 필드 화이트리스트: 알 수 없는 필드는 거부
  const allowed = Object.keys(input).every((k) => ['name', 'email', 'age'].includes(k));
  if (!allowed) errors.push('허용되지 않은 필드가 포함되어 있습니다');
  return errors;
}

// ---------- 4. 비밀번호 해싱 (scrypt + salt) ----------
function hashPassword(password) {
  const salt = crypto.randomBytes(16).toString('hex');
  const hash = crypto.scryptSync(password, salt, 64).toString('hex');
  return `${salt}:${hash}`;
}

function verifyPassword(password, stored) {
  const [salt, hash] = stored.split(':');
  const candidate = crypto.scryptSync(password, salt, 64);
  const expected = Buffer.from(hash, 'hex');
  // 타이밍 공격 방지: 상수 시간 비교
  return candidate.length === expected.length && crypto.timingSafeEqual(candidate, expected);
}

const users = new Map();

// ---------- 5. 보안이 적용된 서버 ----------
function json(res, status, body) {
  applySecurityHeaders(res);
  res.writeHead(status, { 'Content-Type': 'application/json; charset=utf-8' });
  res.end(JSON.stringify(body, null, 2));
}

const server = http.createServer((req, res) => {
  const url = new URL(req.url, 'http://localhost');

  // rate limiting: 모든 요청에 적용
  const ip = req.socket.remoteAddress || 'unknown';
  if (!limiter.check(ip)) {
    return json(res, 429, { error: '요청이 너무 많습니다. 잠시 후 다시 시도하세요' });
  }

  if (req.method === 'GET' && url.pathname === '/') {
    return json(res, 200, {
      message: '보안 데모 서버',
      headers: '아래 응답 헤더에 보안 헤더가 포함되어 있습니다',
      loginHint: 'POST /register {name, email, age, password}',
    });
  }

  if (req.method === 'POST' && url.pathname === '/register') {
    let data = '';
    req.on('data', (c) => (data += c));
    req.on('end', () => {
      let input;
      try {
        input = JSON.parse(data);
      } catch {
        return json(res, 400, { error: 'JSON 형식이 올바르지 않습니다' });
      }

      const errors = validateUserInput(input);
      if (errors.length) return json(res, 400, { errors });

      const email = sanitize(input.email); // 출력 시 이스케이프
      const hashed = hashPassword(input.password);
      users.set(input.email, { name: input.name, hashed });

      json(res, 201, {
        message: '회원가입 완료',
        user: { name: sanitize(input.name), email },
        passwordStoredAs: hashed, // 데모용 출력 (실제로는 노출 금지)
      });
    });
    return;
  }

  if (req.method === 'GET' && url.pathname === '/login') {
    const email = url.searchParams.get('email');
    const password = url.searchParams.get('password');
    const user = users.get(email);
    if (!user || !verifyPassword(password || '', user.hashed)) {
      return json(res, 401, { error: '인증 실패' });
    }
    return json(res, 200, { message: '로그인 성공', user: { name: user.name } });
  }

  json(res, 404, { error: 'Not Found' });
});

const PORT = 3000;
server.listen(PORT, () => {
  console.log(`보안 데모 서버: http://localhost:${PORT}`);
  console.log('  GET  /                          보안 헤더 확인');
  console.log('  POST /register                  회원가입 (입력 검증 + 해시 저장)');
  console.log('  GET  /login?email=&password=    로그인 (해시 비교)');
  console.log('\n동일 IP에서 1분에 6회 이상 요청하면 429 응답');
});
