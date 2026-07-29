# 03. npm (Node Package Manager)

## npm이란?

npm은 Node.js의 기본 패키지 관리자로, 라이브러리 설치/관리, 스크립트 실행 등을 수행합니다.

## npm init

프로젝트 시작 시 `package.json`을 생성합니다.

```bash
npm init -y   # 기본값으로 package.json 생성
npm init      # 대화형으로 생성
```

## package.json

```json
{
  "name": "my-app",
  "version": "1.0.0",
  "description": "내 앱",
  "main": "index.js",
  "scripts": {
    "start": "node index.js",
    "dev": "node --watch index.js"
  },
  "dependencies": {
    "express": "^4.18.0"
  },
  "devDependencies": {
    "nodemon": "^3.0.0"
  }
}
```

## dependencies vs devDependencies

| 구분 | 사용처 | 설치 옵션 |
|------|--------|-----------|
| dependencies | 실제 애플리케이션 실행에 필요 | `npm install <package>` |
| devDependencies | 개발/테스트 시에만 필요 | `npm install <package> --save-dev` |

- `dependencies`: express, lodash, axios 등
- `devDependencies`: nodemon, jest, eslint 등

## npm scripts

```bash
npm run start     # "node index.js" 실행
npm run dev       # "node --watch index.js" 실행
npm test          # 테스트 실행 (생략 가능)
```

`start`와 `test`는 `run` 생략 가능: `npm start`, `npm test`

## npx

패키지를 설치하지 않고 일회성으로 실행할 때 사용합니다.

```bash
npx create-react-app my-app
npx nodemon index.js
npx cowsay "Hello!"
```
