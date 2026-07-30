# 00 개발환경 설정

## 필수 도구

- **Node.js** 18 이상 (https://nodejs.org)
- **TypeScript 컴파일러** (`typescript` npm 패키지)
- **ts-node** (선택 사항, 직접 실행)

## TypeScript 설치

### 전역 설치
```bash
npm install -g typescript ts-node
```

### 로컬 설치 (권장)
```bash
npm install --save-dev typescript ts-node @types/node
```

### 설치 확인
```bash
tsc --version
```

## tsconfig.json 설정

```json
{
  "compilerOptions": {
    "target": "ES2020",
    "module": "commonjs",
    "strict": true,
    "esModuleInterop": true,
    "outDir": "./dist",
    "rootDir": "./src"
  }
}
```

## 컴파일 및 실행

```bash
# tsconfig.json 기준 컴파일
tsc

# 단일 파일 컴파일
tsc index.ts

# ts-node로 직접 실행 (컴파일 없이)
ts-node index.ts

# Node.js로 실행 (컴파일 후)
node dist/index.js
```

## VS Code 확장

- **TypeScript and JavaScript Language Features** (VS Code 내장)
- **ESLint**
- **Prettier**

## npx ts-node (설치 없이 실행)
```bash
npx ts-node index.ts
```
