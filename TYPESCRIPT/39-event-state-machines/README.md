# 39: 타입 안전 이벤트와 상태 머신 — 이벤트 맵, FSM

이벤트 시스템과 상태 머신(FSM)에 타입을 부여하면 잘못된 상태 전이를 컴파일 시점에 막을 수 있습니다.

## 이벤트 맵

```typescript
type EventMap = {
  click: { x: number; y: number };
  keydown: { key: string };
  resize: { width: number };
};

// 'click' 이벤트의 payload는 { x, y }로 고정
```

## 상태 머신 (FSM)

```typescript
type State = "idle" | "loading" | "success" | "error";
type Event = "FETCH" | "RESOLVE" | "REJECT" | "RETRY";
```

상태와 이벤트를 조합해 가능한 전이만 허용하면 `fetch`를 `idle` 상태에서만 호출하도록 강제할 수 있습니다.

`index.ts`에서 이벤트 버스와 상태 머신을 직접 구현합니다.

## 실행

```bash
cd TYPESCRIPT/39-event-state-machines
npx ts-node index.ts
```
