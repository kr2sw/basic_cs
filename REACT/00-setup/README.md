# 00 개발환경 설정

## 필수 도구

- **Node.js** 18 이상 (https://nodejs.org)
- **npm** 또는 **yarn** 또는 **pnpm**

## 프로젝트 생성 도구

### Vite (권장)
이 저장소의 React 프로젝트는 Vite로 생성되었습니다.

```bash
npm create vite@latest my-app -- --template react
cd my-app
npm install
npm run dev
```

### Create React App (CRA, 레거시)
```bash
npx create-react-app my-app
cd my-app
npm start
```

### 설치 확인
```bash
node --version
npm --version
```

## 프로젝트 실행

```bash
cd 01-introduction
npm install
npm run dev
# http://localhost:5173
```

### 빌드
```bash
npm run build
# dist/ 폴더에 정적 파일 생성
```

## VS Code 확장

- **ES7+ React/Redux/React-Native snippets**
- **ESLint**
- **Prettier**
