# 31: Express + TS 심화 — 타입 안전 라우터

Express 서버를 TypeScript로 만들 때 라우터와 미들웨어, 핸들러에 타입을 부여해 오류를 줄입니다.

## 핸들러 타입

```typescript
import type { Request, Response, NextFunction } from "express";

type Handler = (req: Request, res: Response, next: NextFunction) => void;
```

## 타입 안전한 응답

응답 JSON의 형태를 인터페이스로 정의하고 `res.json<T>(data: T)`로 감싸면 응답 구조를 강제할 수 있습니다.

## 제네릭 라우터 팩토리

경로 + 핸들러 목록을 받아 Express `Router`를 만들어주는 팩토리 함수를 작성해 보겠습니다.

> 이 챕터는 express가 설치된 환경을 가정합니다. 예제 실행: `npm install express @types/express` 후 `npx ts-node index.ts`

## 실행

```bash
cd TYPESCRIPT/31-express-ts
npm install express @types/express
npx ts-node index.ts
```
