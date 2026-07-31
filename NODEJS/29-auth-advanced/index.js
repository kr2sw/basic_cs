// 고급 인증: JWT 서명/검증, 리프레시 토큰, OAuth2 흐름을
// Node.js 핵심 crypto 모듈로 구현한 예제입니다.
// 실제 사용 시: npm install jsonwebtoken

const crypto = require('crypto');

const ACCESS_SECRET = 'access-secret-key';
const REFRESH_SECRET = 'refresh-secret-key';

// ---------- Base64Url 인코딩 ----------
function base64url(value) {
  return Buffer.from(JSON.stringify(value))
    .toString('base64')
    .replace(/=/g, '')
    .replace(/\+/g, '-')
    .replace(/\//g, '_');
}

// ---------- JWT 생성 (jwt.sign) ----------
function sign(payload, secret, expiresInSec) {
  const header = { alg: 'HS256', typ: 'JWT' };
  const now = Math.floor(Date.now() / 1000);
  const body = { ...payload, iat: now, exp: now + expiresInSec };

  const headerPart = base64url(header);
  const bodyPart = base64url(body);
  const signature = crypto
    .createHmac('sha256', secret)
    .update(`${headerPart}.${bodyPart}`)
    .digest('base64url');
  return `${headerPart}.${bodyPart}.${signature}`;
}

// ---------- JWT 검증 (jwt.verify) ----------
function verify(token, secret) {
  const [headerPart, bodyPart, signature] = token.split('.');
  const expected = crypto
    .createHmac('sha256', secret)
    .update(`${headerPart}.${bodyPart}`)
    .digest('base64url');

  // 타이밍 공격 방지용 상수 시간 비교
  if (!crypto.timingSafeEqual(Buffer.from(signature), Buffer.from(expected))) {
    throw new Error('서명이 올바르지 않습니다 (변조된 토큰)');
  }
  const payload = JSON.parse(Buffer.from(bodyPart, 'base64url').toString());
  if (payload.exp < Math.floor(Date.now() / 1000)) {
    throw new Error('토큰이 만료되었습니다');
  }
  return payload;
}

// ---------- 1. JWT 발급/검증 데모 ----------
console.log('=== 1. JWT 데모 ===');
const accessToken = sign({ userId: 1, role: 'admin' }, ACCESS_SECRET, 15); // 15초
console.log('발급된 JWT:\n', accessToken, '\n');

const decoded = verify(accessToken, ACCESS_SECRET);
console.log('검증 결과:', decoded, '\n');

// 변조 시도
const tampered = accessToken.slice(0, -2) + 'xx';
try {
  verify(tampered, ACCESS_SECRET);
} catch (err) {
  console.log('변조 토큰 검증:', err.message);
}

// 만료 토큰 데모
const expired = sign({ userId: 1 }, ACCESS_SECRET, -1);
try {
  verify(expired, ACCESS_SECRET);
} catch (err) {
  console.log('만료 토큰 검증:', err.message);
}

// ---------- 2. 리프레시 토큰 ----------
console.log('\n=== 2. 리프레시 토큰 ===');

const refreshTokens = new Map(); // 서버 DB라고 가정

function issueRefresh(userId) {
  const token = crypto.randomBytes(32).toString('hex');
  refreshTokens.set(token, {
    userId,
    expiresAt: Date.now() + 7 * 24 * 60 * 60 * 1000, // 7일
  });
  return token;
}

// 리프레시 토큰으로 새 액세스 토큰 발급 (rotation 포함)
function refreshAccess(oldRefreshToken) {
  const record = refreshTokens.get(oldRefreshToken);
  if (!record) throw new Error('리프레시 토큰이 존재하지 않습니다');
  if (record.expiresAt < Date.now()) {
    refreshTokens.delete(oldRefreshToken);
    throw new Error('리프레시 토큰이 만료되었습니다');
  }
  refreshTokens.delete(oldRefreshToken); // 재사용 방지 (rotation)
  const newRefresh = issueRefresh(record.userId);
  const newAccess = sign({ userId: record.userId }, ACCESS_SECRET, 900);
  return { accessToken: newAccess, refreshToken: newRefresh };
}

let refreshToken = issueRefresh(1);
let currentAccess = sign({ userId: 1 }, ACCESS_SECRET, 900);
console.log('액세스 토큰:', currentAccess.slice(0, 30) + '...');
console.log('리프레시 토큰:', refreshToken.slice(0, 12) + '...');

console.log('\n[액세스 토큰 만료 -> 리프레시로 재발급]');
const rotated = refreshAccess(refreshToken);
console.log('새 액세스 토큰 발급 완료 (수명 15분)');
refreshToken = rotated.refreshToken;
console.log('새 리프레시 토큰 발급 완료 (rotation 적용)');

try {
  refreshAccess(refreshTokens.size ? '이전토큰' : rotated.refreshToken);
} catch (err) {
  console.log('이전 리프레시 토큰 재사용 시도:', err.message);
}

// ---------- 3. OAuth2 인가 코드 흐름 ----------
console.log('\n=== 3. OAuth2 인가 코드 흐름 ===');

class OAuth2Server {
  constructor() {
    this.codes = new Map();
    this.clients = [
      { clientId: 'web-app', redirectUri: 'http://localhost:3000/callback' },
    ];
  }

  // 1) 로그인 + 인가 요청 -> 인가 코드 발급
  authorize(username, clientId, scope) {
    const code = crypto.randomBytes(16).toString('hex');
    this.codes.set(code, {
      username,
      clientId,
      scope,
      expiresAt: Date.now() + 10 * 60 * 1000, // 10분
    });
    return code;
  }

  // 2) 인가 코드 -> 액세스 토큰 교환
  exchange(clientId, code) {
    const entry = this.codes.get(code);
    if (!entry || entry.clientId !== clientId || entry.expiresAt < Date.now()) {
      throw new Error('인가 코드가 유효하지 않습니다');
    }
    this.codes.delete(code); // 1회성
    return {
      accessToken: sign({ username: entry.username, scope: entry.scope }, ACCESS_SECRET, 3600),
      tokenType: 'Bearer',
      expiresIn: 3600,
      scope: entry.scope,
    };
  }
}

const oauth = new OAuth2Server();
const code = oauth.authorize('hong@example.com', 'web-app', 'profile email');
console.log('인가 코드 발급:', code.slice(0, 12) + '...');
const tokenResult = oauth.exchange('web-app', code);
console.log('액세스 토큰 교환 완료:', tokenResult.accessToken.slice(0, 30) + '...');
console.log('유효 범위(scope):', tokenResult.scope);
