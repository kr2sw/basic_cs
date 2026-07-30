# 17: AssemblyScript — TypeScript를 WASM으로

AssemblyScript는 TypeScript와 유사한 문법으로 WebAssembly를 작성할 수 있는 언어입니다. JavaScript 개발자에게 가장 친숙한 WASM 개발 도구입니다.

## 설치

```bash
npm init -y
npm install --save-dev assemblyscript
npx asc --version
```

## 프로젝트 초기화

```bash
npx asinit .
```

## 컴파일

```bash
npx asc assembly/index.ts -o build/optimized.wasm --optimize --exportRuntime
```

## 실행

```bash
npx http-server .
```
