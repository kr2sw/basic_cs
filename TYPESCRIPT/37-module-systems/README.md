# 37: 모듈 시스템 심화 — ESM/CJS 상호운용, import 타입

Node.js에서 TypeScript 모듈은 **ESM(ES Modules)** 과 **CommonJS(CJS)** 두 방식이 존재합니다.

## ESM vs CJS

```typescript
// ESM
import { add } from "./math.js";
export const result = add(1, 2);

// CommonJS
const { add } = require("./math");
module.exports = { result: add(1, 2) };
```

## ESM에서 CJS 가져오기

- 기본 내보내기가 `module.exports` 전체를 가리킵니다.
- named import는 대부분 브리징되지만, 일부 라이브러리는 `createRequire`가 필요합니다.

## import type

타입만 가져올 때 `import type`을 쓰면 런타임에서 제거되어 번들 크기가 줄고 순환 참조가 예방됩니다.

`index.ts`에서 ESM/CJS 로더를 모델링합니다.

## 실행

```bash
cd TYPESCRIPT/37-module-systems
npx ts-node index.ts
```
