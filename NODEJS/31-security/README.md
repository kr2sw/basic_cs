# 31: 보안 — Helmet, Rate Limiting, Input Validation, Crypto

Node.js 웹 애플리케이션 보안 필수 요소를 학습합니다.

## Helmet (보안 헤더)

Helmet은 HTTP 응답에 보안 관련 헤더를 자동으로 추가합니다.

```js
const helmet = require('helmet');
app.use(helmet());
```

주요 헤더:

| 헤더 | 역할 |
|------|------|
| `Content-Security-Policy` | XSS, 데이터 주입 방지 |
| `X-Frame-Options: DENY` | 클릭재킹 방지 |
| `X-Content-Type-Options: nosniff` | MIME 스니핑 방지 |
| `Strict-Transport-Security` | HTTPS 강제 (HSTS) |
| `Referrer-Policy` | 리퍼러 정보 유출 방지 |

## Rate Limiting

특정 IP가 짧은 시간에 과도한 요청을 보내면 차단합니다.

```js
const rateLimit = require('express-rate-limit');
app.use(rateLimit({ windowMs: 60_000, max: 100 }));
```

## 입력 검증과 이스케이프

- **SQL 인젝션**: 파라미터화된 쿼리 사용 (`?`, prepared statements)
- **XSS**: 사용자 입력을 HTML로 렌더링하지 않고 이스케이프
- **검증**: 라이브러리(joi, zod) 또는 정규식으로 화이트리스트 검증

## 비밀번호 해싱

비밀번호는 절대 평문으로 저장하지 않습니다. 소금(salt)을 넣고 해시합니다.

```js
const hash = crypto.scryptSync(password, salt, 64);
```

검증 시 `timingSafeEqual`으로 타이밍 공격을 방지합니다.

## 예제 실행

```bash
node index.js
```

```bash
curl http://localhost:3000/
curl -H "Authorization: Bearer wrong" http://localhost:3000/protected
# 100번 이상 요청하면 429 응답을 확인할 수 있습니다
```
