# 00 개발환경 설정

## 필수 도구

- **Node.js** 18 이상 (https://nodejs.org)
- **npm** 또는 **yarn** 또는 **pnpm**
- **Vue CLI** 또는 **create-vue** (스캐폴딩 도구)

## create-vue (권장)

Vue 3 공식 프로젝트 생성 도구입니다.

```bash
npm create vue@latest my-app
cd my-app
npm install
npm run dev
```

### 옵션 선택 (대화형)
- TypeScript 지원
- JSX 지원
- Vue Router
- Pinia (상태 관리)
- Vitest (테스트)
- ESLint / Prettier

## Vue CLI (레거시)

```bash
npm install -g @vue/cli
vue create my-app
cd my-app
npm run serve
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

- **Vue - Official** (Volar, Vue 3 필수)
- **ESLint**
- **Prettier**
- **Vue VSCode Snippets**
