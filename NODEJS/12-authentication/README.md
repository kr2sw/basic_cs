# 12. 인증 (Authentication)

JWT와 bcrypt를 사용한 사용자 인증 시스템을 학습합니다.

## 개념

- **해싱(Hashing)**: 비밀번호를 안전하게 저장하기 위해 단방향 암호화
- **JWT (JSON Web Token)**: 사용자 인증 정보를 토큰에 담아 클라이언트에 전달
- **토큰 기반 인증**: 서버가 세션을 유지하지 않아도 됨 (Stateless)

## 설치

```bash
npm install jsonwebtoken bcrypt
```

## bcrypt로 비밀번호 해싱

```js
const bcrypt = require('bcrypt');
const saltRounds = 10;

const hashedPassword = await bcrypt.hash('userPassword', saltRounds);
const isMatch = await bcrypt.compare('userPassword', hashedPassword);
```

## JWT sign / verify

```js
const jwt = require('jsonwebtoken');
const secret = 'mySecretKey';

// 토큰 발급
const token = jwt.sign({ userId: 1, role: 'user' }, secret, { expiresIn: '1h' });

// 토큰 검증
const decoded = jwt.verify(token, secret);
console.log(decoded.userId); // 1
```

## 로그인/회원가입 플로우

1. 회원가입: 비밀번호 해싱 후 DB 저장
2. 로그인: 비밀번호 비교 후 JWT 발급
3. 보호된 라우트: JWT 미들웨어로 검증

## 예제 실행

```bash
node index.js
```
