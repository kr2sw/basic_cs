# 21: 고급 제네릭 — 타입 추론, 제약, infer 패턴

제네릭은 타입을 인자처럼 받아 재사용 가능한 타입/함수를 만드는 TypeScript의 핵심 기능입니다.

## 타입 추론 (Type Inference)

함수 호출 시 인자의 타입을 보고 제네릭 타입 변수를 자동으로 추론합니다.

```typescript
function identity<T>(value: T): T {
  return value;
}
identity(42);        // T = number
identity("hello");   // T = string
```

## 제약 조건 (Constraints)

`extends` 키워드로 타입 변수에 제약을 둘 수 있습니다.

```typescript
function getLength<T extends { length: number }>(value: T): number {
  return value.length;
}
```

## infer 패턴

조건부 타입에서 `infer`로 타입을 추출합니다. 배열 요소, 함수 반환 타입, Promise 언랩 등을 분리해냅니다.

```typescript
type ElementType<T> = T extends (infer U)[] ? U : never;
```

자세한 내용은 `index.ts` 예제를 실행해 확인하세요.

## 실행

```bash
cd TYPESCRIPT/21-generics-advanced
npx ts-node index.ts
```
