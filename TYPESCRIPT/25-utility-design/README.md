# 25: 유틸리티 타입 설계 — Partial, Pick, ReturnType 직접 구현

내장 유틸리티 타입의 동작 원리를 이해하고 직접 구현해 봅니다.

## 유틸리티 타입이란?

`Partial<T>`, `Required<T>`, `Pick<T,K>`, `Omit<T,K>`, `ReturnType<T>` 등 타입 변환을 수행하는 타입입니다. 이들은 전부 **매핑 타입**과 **조건부 타입**으로 구현됩니다.

## 직접 구현하기

```typescript
type MyPartial<T> = { [K in keyof T]?: T[K] };
type MyReturnType<T> = T extends (...args: never[]) => infer R ? R : never;
```

직접 구현하면 내장 타입의 동작 원리를 깊게 이해할 수 있고, 커스텀 유틸리티를 만들 수 있습니다.

자세한 구현과 검증은 `index.ts`를 참고하세요.

## 실행

```bash
cd TYPESCRIPT/25-utility-design
npx ts-node index.ts
```
