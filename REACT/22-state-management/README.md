# 22: 상태 관리 — State Management with Context + useReducer

여러 컴포넌트가 공유하는 대형 상태를 Context + useReducer 조합으로 구조적으로 관리합니다.

## 왜 useReducer인가?

상태가 복잡해지면 `useState` 여러 개보다 **액션(action) → 리듀서(reducer)** 패턴이 유지보수에 유리합니다. 상태 변화의 모든 경로가 `switch` 문 하나에 모이므로 디버깅이 쉽습니다.

```jsx
const initialState = []

function todoReducer(state, action) {
  switch (action.type) {
    case 'add':    return [...state, { id: Date.now(), text: action.text, done: false }]
    case 'toggle': return state.map(t => t.id === action.id ? { ...t, done: !t.done } : t)
    case 'remove': return state.filter(t => t.id !== action.id)
    default:       return state
  }
}
```

## Context로 리듀서 전달하기

`useReducer`가 만든 `[state, dispatch]`를 Context로 내려보내면 어떤 깊이의 컴포넌트든 dispatch만으로 상태를 바꿀 수 있습니다.

```jsx
const TodoContext = createContext(null)

function TodoProvider({ children }) {
  const [todos, dispatch] = useReducer(todoReducer, initialState)
  return <TodoContext.Provider value={{ todos, dispatch }}>{children}</TodoContext.Provider>
}
```

## 액션 생성자 (Action Creators)

컴포넌트에서 `dispatch({ type: 'add', text })`를 직접 쓰면 오타와 중복이 발생합니다. **액션 생성자 함수**로 비즈니스 로직을 캡슐화하면 컴포넌트는 의도만 전달합니다.

```jsx
function useTodoActions() {
  const { dispatch } = useContext(TodoContext)
  return {
    add: (text) => dispatch({ type: 'add', text }),
    toggle: (id) => dispatch({ type: 'toggle', id }),
    clear: () => dispatch({ type: 'clear' }),
  }
}
```

## 컨텍스트 분리

값을 하나의 Context로 묶으면 상태가 조금만 바뀌어도 모든 소비자가 리렌더링됩니다. **State 컨텍스트와 Dispatch 컨텍스트를 분리**하면 필요한 컴포넌트만 리렌더링되어 성능이 좋아집니다.

## 실행

```bash
npm install && npm run dev
```
