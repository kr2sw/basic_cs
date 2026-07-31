# 38: TypeScript + Node — Type-Safe Server Structure

TypeScript로 타입 안전한 Node.js 서버를 구성하는 방법을 학습합니다.

## 왜 TypeScript인가?

규모가 커질수록 자바스크립트의 타입 오류(정의되지 않은 속성 접근 등)가 런타임에서 발견됩니다. TypeScript는 **컴파일 시점**에 오류를 잡아줍니다.

```ts
interface User {
  id: number;
  name: string;
  email: string;
}

function getUser(id: number): User | null {
  // ...
}
```

## 설치와 설정

```bash
npm install -D typescript @types/node ts-node
npx tsc --init
```

```json
// tsconfig.json
{
  "compilerOptions": {
    "target": "ES2022",
    "module": "CommonJS",
    "rootDir": "src",
    "outDir": "dist",
    "strict": true
  }
}
```

## 실행

```bash
npx ts-node src/index.ts      # 개발 중
npx tsc && node dist/index.js # 프로덕션
```

## 제네릭 (Generic)

`Repository<T>`처럼 타입을 파라미터로 받아 재사용할 수 있습니다.

```ts
class Repository<T extends { id: number }> {
  create(entity: T): T { /* ... */ }
  findById(id: number): T | null { /* ... */ }
}
```

## 타입 가드 (Type Guard)

외부 데이터(API 응답, JSON)는 `unknown`으로 받아 검증한 뒤 사용합니다.

```ts
function isUser(value: unknown): value is User {
  if (!value || typeof value !== 'object') return false;
  const u = value as Record<string, unknown>;
  return typeof u.id === 'number' && typeof u.name === 'string';
}
```

## 예제 실행

예제는 TypeScript 컴파일 없이 실행되도록 **JSDoc 타입 주석**으로 타입 안전성을 보여줍니다.

```bash
node index.js
```
