import { create } from 'zustand'
import { configureStore, createSlice } from '@reduxjs/toolkit'
import { Provider, useSelector, useDispatch } from 'react-redux'

// --- 1. Zustand 스토어 ---
// Provider 없이 전역 훅으로 바로 사용. set()으로 상태를 불변하게 갱신한다.
const useZustandStore = create(set => ({
  count: 0,
  increment: () => set(s => ({ count: s.count + 1 })),
  decrement: () => set(s => ({ count: s.count - 1 })),
  reset: () => set({ count: 0 }),
}))

// --- 2. Redux Toolkit slice ---
// createSlice가 액션 타입과 리듀서를 자동 생성. Immer가 불변성을 처리한다.
const counterSlice = createSlice({
  name: 'counter',
  initialState: { value: 0 },
  reducers: {
    increment: state => { state.value += 1 },
    decrement: state => { state.value -= 1 },
    reset: state => { state.value = 0 },
  },
})

const store = configureStore({ reducer: counterSlice.reducer })

function ZustandCounter() {
  // 선택자 구독: 필요한 값만 구독해 불필요한 리렌더링을 막는다
  const count = useZustandStore(s => s.count)
  const increment = useZustandStore(s => s.increment)
  const decrement = useZustandStore(s => s.decrement)
  const reset = useZustandStore(s => s.reset)

  return (
    <section>
      <h2>Zustand</h2>
      <p>count: {count}</p>
      <button onClick={increment}>+1</button>
      <button onClick={decrement}>-1</button>
      <button onClick={reset}>reset</button>
    </section>
  )
}

function ReduxCounter() {
  // useSelector로 스토어에서 값 읽기, useDispatch로 액션 보내기
  const value = useSelector(s => s.value)
  const dispatch = useDispatch()

  return (
    <section>
      <h2>Redux Toolkit</h2>
      <p>value: {value}</p>
      <button onClick={() => dispatch(counterSlice.actions.increment())}>+1</button>
      <button onClick={() => dispatch(counterSlice.actions.decrement())}>-1</button>
      <button onClick={() => dispatch(counterSlice.actions.reset())}>reset</button>
    </section>
  )
}

function App() {
  return (
    // Redux는 Provider로 스토어를 주입해야 한다 (Zustand는 불필요)
    <Provider store={store}>
      <div>
        <h1>외부 상태 라이브러리</h1>
        <ZustandCounter />
        <ReduxCounter />
      </div>
    </Provider>
  )
}

export default App
