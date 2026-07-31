import { createContext, useContext, useReducer, useState, useEffect, useMemo, useCallback, memo } from 'react'

// ---- 1. localStorage 연동 커스텀 훅 (21장 패턴) ----
function useLocalStorage(key, initial) {
  const [value, setValue] = useState(() => {
    try { return JSON.parse(localStorage.getItem(key)) ?? initial }
    catch { return initial }
  })
  useEffect(() => {
    localStorage.setItem(key, JSON.stringify(value))
  }, [key, value])
  return [value, setValue]
}

// ---- 2. 상태: Context + useReducer (22장) ----
const TodoStateContext = createContext(null)
const TodoDispatchContext = createContext(null)

function todoReducer(state, action) {
  switch (action.type) {
    case 'add':
      return [...state, { id: Date.now(), text: action.text, done: false, priority: action.priority }]
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

// ---- 3. 메모이제이션: props가 같은 리스트 아이템 리렌더 방지 (27장) ----
const TodoItem = memo(function TodoItem({ todo, onToggle, onRemove }) {
  return (
    <li>
      <label>
        <input
          type="checkbox"
          checked={todo.done}
          onChange={() => onToggle(todo.id)}
          aria-label={`${todo.text} 완료 처리`}
        />
        <span style={{ textDecoration: todo.done ? 'line-through' : 'none' }}>
          [{todo.priority === 'high' ? '중요' : '일반'}] {todo.text}
        </span>
      </label>
      <button onClick={() => onRemove(todo.id)} aria-label={`${todo.text} 삭제`}>삭제</button>
    </li>
  )
})

function TodoInput() {
  const dispatch = useContext(TodoDispatchContext)
  const [text, setText] = useState('')
  const [priority, setPriority] = useState('normal')

  function onSubmit(e) {
    e.preventDefault()
    if (!text.trim()) return
    dispatch({ type: 'add', text, priority })
    setText('')
  }

  return (
    <form onSubmit={onSubmit}>
      <input value={text} onChange={e => setText(e.target.value)} placeholder="새 할일" />
      <select value={priority} onChange={e => setPriority(e.target.value)} aria-label="우선순위">
        <option value="normal">일반</option>
        <option value="high">중요</option>
      </select>
      <button type="submit">추가</button>
    </form>
  )
}

function TodoList() {
  const todos = useContext(TodoStateContext)
  const dispatch = useContext(TodoDispatchContext)
  const [filter, setFilter] = useState('all')

  // useCallback: TodoItem의 memo가 효과를 발휘하도록 참조 고정
  const toggle = useCallback(id => dispatch({ type: 'toggle', id }), [dispatch])
  const remove = useCallback(id => dispatch({ type: 'remove', id }), [dispatch])

  // useMemo: 필터링 결과 재계산 최소화
  const visible = useMemo(() => {
    return todos.filter(t => {
      if (filter === 'active') return !t.done
      if (filter === 'done') return t.done
      return true
    })
  }, [todos, filter])

  return (
    <div>
      <div>
        {['all', 'active', 'done'].map(f => (
          <button key={f} onClick={() => setFilter(f)} disabled={filter === f}>{f}</button>
        ))}
      </div>
      <ul>
        {visible.map(t => <TodoItem key={t.id} todo={t} onToggle={toggle} onRemove={remove} />)}
      </ul>
    </div>
  )
}

function Stats() {
  const todos = useContext(TodoStateContext)
  const dispatch = useContext(TodoDispatchContext)

  // useMemo: 전체 통계는 todos가 바뀔 때만 계산
  const stats = useMemo(() => {
    const done = todos.filter(t => t.done).length
    const high = todos.filter(t => t.priority === 'high' && !t.done).length
    return { total: todos.length, done, high }
  }, [todos])

  return (
    <p>
      <span aria-live="polite">
        전체 {stats.total} · 완료 {stats.done} · 대기 중인 중요 항목 {stats.high}
      </span>
      {stats.done > 0 && <button onClick={() => dispatch({ type: 'clear' })}>완료 지우기</button>}
    </p>
  )
}

// ---- 4. 전체 통합 ----
function App() {
  // 저장된 할일을 꺼내 리듀서의 초기 상태로 사용 (재방문 시 복원)
  const [savedTodos, setSavedTodos] = useLocalStorage('final-todos', [])
  const [todos, dispatch] = useReducer(todoReducer, savedTodos)

  // todos가 바뀔 때마다 localStorage에 저장
  useEffect(() => {
    setSavedTodos(todos)
  }, [todos, setSavedTodos])

  return (
    <TodoStateContext.Provider value={todos}>
      <TodoDispatchContext.Provider value={dispatch}>
        <div style={{ maxWidth: 520, margin: '0 auto' }}>
          <h1>할일 관리 (종합 프로젝트)</h1>
          <TodoInput />
          <TodoList />
          <Stats />
        </div>
      </TodoDispatchContext.Provider>
    </TodoStateContext.Provider>
  )
}

export default App
