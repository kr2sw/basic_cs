# 35: Node + TS 고급 — 워커, 스트림 타입

Node.js의 고급 기능(워커 스레드, 스트림)을 TypeScript 타입과 함께 사용합니다.

## 워커 스레드

```typescript
import { Worker } from "node:worker_threads";

const worker = new Worker("./worker.js");
worker.postMessage({ task: "fib", n: 30 });
worker.on("message", (result: number) => console.log(result));
```

## 스트림 타입

`Readable`, `Writable`, `Transform` 스트림에 제네릭 타입을 부여합니다.

```typescript
import { Transform } from "node:stream";
const upper = new Transform({
  transform(chunk: Buffer, _enc: BufferEncoding, cb: TransformCallback) {
    cb(null, chunk.toString().toUpperCase());
  },
});
```

`index.ts`에서 스트림 파이프라인과 워커 구조를 타입과 함께 구현합니다.

## 실행

```bash
cd TYPESCRIPT/35-node-advanced
npx ts-node index.ts
```
