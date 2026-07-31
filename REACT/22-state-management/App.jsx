import { createContext, useContext, useReducer, useState } from 'react'

// State와 Dispatch를 분리한 Context 2개 (불필요한 리렌더링 방지)
const TodoStateContext = createContext(null)
const TodoDispatchContext = createContext(null)

const initialState = []

// 리듀서: 모든 상태 변화가 여기 한 곳에 모인다
function todoReducer(state, action) {
  switch (action.type) {
    case 'add':
      return [...state, { id: Date.now(), text: action.text, done: false }]
    case 'toggle':
      return state.map(t => t.id === action.id ? { ...t, done: !t.done } : t)
    case 'remove':
      return state.filter(t => t.id !== action.id)
    case 'clear':
      return state.filter(t => !t.done)
    default:
      return state
  }
}

function TodoProvider({ children }) {
  const [todos, dispatch] = useReducer(todoReducer, initialState)
  return (
    <TodoStateContext.Provider value={todos}>
      <TodoDispatchContext.Provider value={dispatch}>
        {children}
      </TodoDispatchContext.Provider>
    </TodoStateContext.Provider>
  )
}

// 상태 읽기 훅
function useTodos() {
  return useContext(TodoStateContext)
}

// 액션 생성자: 컴포넌트는 의도만, 상세 로직은 여기서
function useTodoActions() {
  const dispatch = useContext(TodoDispatchContext)
  return {
    add: text => dispatch({ type: 'add', text }),
    toggle: id => dispatch({ type: 'toggle', id }),
    remove: id => dispatch({ type: 'remove', id }),
    clear: () => dispatch({ type: 'clear' }),
  }
}

function TodoInput() {
  const [text, setText] = useState('')
  const { add } = useTodoActions()

  function onSubmit(e) {
    e.preventDefault()
    if (!text.trim()) return
    add(text)
    setText('')
  }

  return (
    <form onSubmit={onSubmit}>
      <input value={text} onChange={e => setText(e.target.value)} placeholder="새 할일 입력" />
      <button type="submit">추가</button>
    </form>
  )
}

function TodoList() {
  const todos = useTodos()
  const { toggle, remove } = useTodoActions()
  const [filter, setFilter] = useState('all') // 'all' | 'active' | 'done'

  const visible = todos.filter(t => {
    if (filter === 'active') return !t.done
    if (filter === 'done') return t.done
    return true
  })

  return (
    <div>
      <div>
        {['all', 'active', 'done'].map(f => (
          <button key={f} onClick={() => setFilter(f)} disabled={filter === f}>{f}</button>
        ))}
      </div>
      <ul>
        {visible.map(t => (
          <li key={t.id}>
            <input type="checkbox" checked={t.done} onChange={() => toggle(t.id)} />
            <span style={{ textDecoration: t.done ? 'line-through' : 'none' }}>{t.text}</span>
            <button onClick={() => remove(t.id)}>삭제</button>
          </li>
        ))}
      </ul>
    </div>
  )
}

function Stats() {
  const todos = useTodos()
  const { clear } = useTodoActions()
  const done = todos.filter(t => t.done).length
  return (
    <p>
      전체 {todos.length} / 완료 {done}
      {done > 0 && <button onClick={clear}>완료 항목 지우기</button>}
    </p>
  )
}

function App() {
  return (
    <TodoProvider>
      <div>
        <h1>Context + useReducer 할일 앱</h1>
        <TodoInput />
        <TodoList />
        <Stats />
      </div>
    </TodoProvider>
  )
}

export default App
