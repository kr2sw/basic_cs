import { useEffect, useState } from 'react'

// 테스트 대상 컴포넌트. /api/todos를 fetch로 읽는다.
export function TodoCard({ todo, onToggle }) {
  return (
    <li>
      <label>
        <input
          type="checkbox"
          checked={todo.done}
          onChange={() => onToggle(todo.id)}
          aria-label={`${todo.title} 완료 처리`}
        />
        {todo.title}
      </label>
    </li>
  )
}

export default function App() {
  const [todos, setTodos] = useState([])
  const [status, setStatus] = useState('loading')   // loading | done | error
  const [title, setTitle] = useState('')

  useEffect(() => {
    fetch('/api/todos')
      .then(r => r.json())
      .then(data => { setTodos(data); setStatus('done') })
      .catch(() => setStatus('error'))
  }, [])

  // 토글도 실제로는 PATCH 요청이지만, 이 예제에서는 로컬 상태만 갱신
  function toggle(id) {
    setTodos(list => list.map(t => t.id === id ? { ...t, done: !t.done } : t))
  }

  function onSubmit(e) {
    e.preventDefault()
    if (!title.trim()) return
    setTodos(list => [...list, { id: Date.now(), title, done: false }])
    setTitle('')
  }

  return (
    <div>
      <h1>고급 테스팅 대상</h1>

      {status === 'loading' && <p role="status">불러오는 중...</p>}
      {status === 'error' && <p style={{ color: 'red' }}>불러오기 실패</p>}

      {status === 'done' && (
        <>
          <form onSubmit={onSubmit}>
            <input value={title} onChange={e => setTitle(e.target.value)} placeholder="새 할일" />
            <button type="submit">추가</button>
          </form>
          <ul>
            {todos.map(t => <TodoCard key={t.id} todo={t} onToggle={toggle} />)}
          </ul>
        </>
      )}
    </div>
  )
}
