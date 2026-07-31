# 29: 고급 인증 — JWT, Refresh Token, OAuth2 Concepts

JWT와 리프레시 토큰, OAuth2 흐름을 학습합니다.

## JWT 구조

JWT는 `헤더.페이로드.서명` 세 부분으로 구성됩니다.

```
eyJhbGciOiJIUzI1NiJ9.eyJ1c2VySWQiOjF9.9nW7yJWzX...
   헤더(알고리즘)     페이로드(정보)         서명(HMAC)
```

서명은 비밀키로 생성되어 토큰이 위조/변조되었는지 검증합니다.

```js
const jwt = require('jsonwebtoken');
const token = jwt.sign({ userId: 1, role: 'admin' }, SECRET, { expiresIn: '15m' });
const decoded = jwt.verify(token, SECRET);
```

## 액세스 토큰 vs 리프레시 토큰

| 구분 | 액세스 토큰 | 리프레시 토큰 |
|------|------------|--------------|
| 수명 | 짧음 (15분~1시간) | 김 (7일~30일) |
| 저장 | 메모리/로컬스토리지 | 서버 DB (또는 HttpOnly 쿠키) |
| 목적 | API 요청 인증 | 만료된 액세스 토큰 재발급 |

만료된 액세스 토큰은 리프레시 토큰으로 교체합니다.

```js
const newToken = jwt.sign({ userId }, SECRET, { expiresIn: '15m' });
```

## OAuth2 개념

제3자 앱이 비밀번호를 알지 못한 채 인가받는 흐름입니다.

1. 사용자가 앱의 "Google 로그인" 클릭
2. 앱이 인가 서버에 인가 코드 요청
3. 사용자가 로그인/동의
4. 인가 서버가 앱에 인가 코드 발급
5. 앱이 인가 코드로 액세스 토큰 교환
6. 토큰으로 사용자 정보 조회

## 예제 실행

예제는 jsonwebtoken 설치 없이 Node `crypto`로 JWT 서명/검증을 구현합니다.

```bash
node index.js
```
