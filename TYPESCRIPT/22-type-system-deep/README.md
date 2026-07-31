# 22: 타입 시스템 심화 — 구조적 타이핑, 타입 동작 원리

TypeScript의 타입 시스템은 **구조적 타이핑(Structural Typing)** 을 기반으로 합니다. 멤버의 이름이 아닌 **모양(구조)** 이 같으면 호환됩니다.

## 구조적 타이핑

```typescript
interface Point { x: number; y: number; }
interface NamedPoint { x: number; y: number; name: string; }

const p: Point = { x: 1, y: 2 };
const np: NamedPoint = { x: 1, y: 2, name: "A" };
const p2: Point = np;  // OK - NamedPoint는 Point의 슈퍼셋
```

## 공변성과 반공변성

함수 타입의 호환성은 매개변수는 반공변(bivariant 검사), 반환값은 공변입니다.

## 할당 호환성 규칙

- **좌표 초과 속성 검사**: 객체 리터럴은 초과 속성이 있으면 에러 (fresh literal)
- **선택적 속성**: `?` 는 `undefined`를 허용
- **readonly**: 런타임이 아닌 컴파일 타임 검사

`index.ts`에서 각 규칙을 직접 확인할 수 있습니다.

## 실행

```bash
cd TYPESCRIPT/22-type-system-deep
npx ts-node index.ts
```
