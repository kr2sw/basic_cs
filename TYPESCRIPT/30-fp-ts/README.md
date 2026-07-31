# 30: 함수형 프로그래밍 — Option/Either 패턴, 파이프라인

함수형 프로그래밍(FP)은 부수 효과를 없애고 데이터 변환의 파이프라인으로 프로그램을 구성합니다.

## Option 타입

`null`/`undefined` 대신 **값이 있거나(Some) 없거나(None)** 를 명시적으로 표현합니다.

```typescript
type Option<T> = { kind: "some"; value: T } | { kind: "none" };
```

## Either 타입

성공(Left)/실패(Right) 두 경로를 타입으로 표현해 예외 대신 값으로 오류를 다룹니다.

## 파이프라인

```typescript
const result = pipe(
  getData(),
  map(transform),
  filter(isValid)
);
```

`index.ts`에서 직접 구현해 확인합니다.

## 실행

```bash
cd TYPESCRIPT/30-fp-ts
npx ts-node index.ts
```
