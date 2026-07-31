# 23: 조건부/매핑 타입 — 분배 법칙, 재귀 타입

조건부 타입(Conditional Types)과 매핑 타입(Mapped Types)은 타입을 프로그램처럼 다루는 고급 기능입니다.

## 조건부 타입

```typescript
type IsString<T> = T extends string ? true : false;
```

## 분배 법칙 (Distributive Conditional Types)

`T extends U` 에서 `T`가 유니온이면 각 멤버에 대해 개별 평가 후 유니온으로 합쳐집니다.

```typescript
type ToArray<T> = T extends unknown ? T[] : never;
// string[] | number[]  (배열 전체가 아니라 멤버별 분배)
```

## 매핑 타입

```typescript
type Readonly<T> = { readonly [K in keyof T]: T[K] };
```

## 재귀 타입

`DeepReadonly`, `DeepPartial` 처럼 재귀적으로 순회하는 타입을 만들 수 있습니다.

자세한 예제는 `index.ts`를 참고하세요.

## 실행

```bash
cd TYPESCRIPT/23-conditional-mapped
npx ts-node index.ts
```
