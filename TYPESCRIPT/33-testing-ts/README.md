# 33: TS 테스팅 — vitest/jest, 타입 테스팅

TypeScript 테스트는 **런타임 테스트**(vitest/jest)와 **타입 테스트**(tsd 스타일) 두 층위로 나뉩니다.

## 런타임 테스트

```typescript
import { describe, it, expect } from "vitest";

describe("add", () => {
  it("두 수를 더한다", () => {
    expect(add(1, 2)).toBe(3);
  });
});
```

## 타입 테스트

타입 수준에서 정확한지 컴파일 타임에 검증합니다.

```typescript
type Expect<T extends true> = T;
type Test1 = Expect<Equal<ReturnType<typeof add>, number>>;
```

`index.ts`에서 미니 테스트 러너와 타입 검증을 함께 구현합니다.

## 실행

```bash
cd TYPESCRIPT/33-testing-ts
npx ts-node index.ts
```
