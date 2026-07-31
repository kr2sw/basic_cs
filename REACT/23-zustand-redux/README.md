# 23: Zustand / Redux Toolkit — External State Libraries

프로젝트가 커지면 Context만으로는 부족합니다. 외부 상태 라이브러리인 **Zustand**와 **Redux Toolkit**을 비교하며 배웁니다.

## Zustand — 가벼운 전역 상태

작은 API, Context 없이 훅 하나로 전역 상태를 사용합니다. Provider가 필요 없고 **선택자**로 필요한 값만 구독합니다.

```jsx
import { create } from 'zustand'

const useStore = create(set => ({
  count: 0,
  increment: () => set(s => ({ count: s.count + 1 })),
  decrement: () => set(s => ({ count: s.count - 1 })),
}))

function Counter() {
  const { count, increment } = useStore() // 훅으로 바로 접근
  return <button onClick={increment}>{count}</button>
}
```

선택자를 쓰면 값이 바뀔 때만 리렌더링됩니다: `const count = useStore(s => s.count)`.

## Redux Toolkit — 예측 가능한 대규모 상태

Redux는 하나의 스토어 + 순수 리듀서로 상태를 관리합니다. RTK의 `createSlice`는 액션/리듀서/불변성을 한 번에 처리합니다.

```jsx
import { configureStore, createSlice } from '@reduxjs/toolkit'
import { Provider, useSelector, useDispatch } from 'react-redux'

const counterSlice = createSlice({
  name: 'counter',
  initialState: { value: 0 },
  reducers: {
    increment: state => { state.value += 1 },  // Immer가 불변 업데이트를 대신 처리
    decrement: state => { state.value -= 1 },
  },
})

const store = configureStore({ reducer: counterSlice.reducer })
```

## 둘을 언제 쓰나?

- **Zustand**: 작은 앱, 빠른 도입, 미들웨어 없이 간단할 때
- **Redux Toolkit**: 팀 규모가 크고 디버깅/추적(DevTools, devtools-middleware)이 중요할 때

## 실행

```bash
npm install zustand @reduxjs/toolkit react-redux && npm run dev
```
