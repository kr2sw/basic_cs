# 16. 환경 변수 (Environment Variables)

환경별 설정을 분리하고 민감한 정보를 안전하게 관리하는 방법을 학습합니다.

## dotenv

`.env` 파일의 키-값 쌍을 `process.env`에 로드합니다.

### 설치

```bash
npm install dotenv
```

## 사용법

```js
require('dotenv').config();
console.log(process.env.PORT);
```

## `.env` 파일 예제

```
PORT=3000
DB_HOST=localhost
DB_USER=admin
DB_PASSWORD=secret123
JWT_SECRET=mySuperSecretKey
NODE_ENV=development
```

## 환경별 설정 분리

```
.env          # 공통 설정 (버전 관리에 포함)
.env.development  # 개발 환경
.env.production   # 운영 환경
```

```js
const envFile = `.env.${process.env.NODE_ENV || 'development'}`;
require('dotenv').config({ path: envFile });
```

## `.gitignore` 설정

`.env` 파일은 절대 Git에 커밋하지 않습니다.

```gitignore
.env
.env.development
.env.production
```

## 예제 실행

```bash
node index.js
```
