# 36: 상태 머신 — State Machines with XState

가능한 상태와 전이(transition)를 명시적으로 설계하는 유한 상태 머신(FSM)을 XState로 구현합니다.

## FSM 개념

상태 머신은 **상태(state) + 이벤트(event) + 전이(transition)** 의 모델입니다. 예를 들어 신호등은 `red → green → yellow → red`만 허용됩니다.

```js
import { createMachine } from 'xstate'

const trafficMachine = createMachine({
  id: 'traffic',
  initial: 'red',
  states: {
    red:    { on: { NEXT: 'green' } },
    green:  { on: { NEXT: 'yellow' } },
    yellow: { on: { NEXT: 'red' } },
  },
})
```

"불가능한 상태"가 컴파일 타임에 구조적으로 배제되므로 버그가 줄어듭니다.

## 컨텍스트와 액션

상태 전이와 함께 데이터(`context`)를 갱신하려면 `assign`을 사용합니다.

```js
import { createMachine, assign } from 'xstate'

const asyncMachine = createMachine({
  id: 'fetch',
  initial: 'idle',
  context: { data: null, error: null },
  states: {
    idle:    { on: { FETCH: 'loading' } },
    loading: { on: { SUCCESS: 'success', FAIL: 'failure' } },
    success: { entry: assign({ data: ({ event }) => event.data }), on: { FETCH: 'loading' } },
    failure: { entry: assign({ error: ({ event }) => event.error }), on: { FETCH: 'loading' } },
  },
})
```

## useMachine 훅

`useMachine`이 머신의 현재 상태와 전이 함수를 반환합니다.

```jsx
const [state, send] = useMachine(trafficMachine)
send({ type: 'NEXT' })   // state.value가 red -> green 으로 변경
```

## 실행

```bash
npm install xstate @xstate/react && npm run dev
```
