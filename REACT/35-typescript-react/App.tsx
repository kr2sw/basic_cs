import { useState, useRef } from 'react'

// 1. Props 인터페이스로 컴포넌트 API 정의
interface Todo {
  id: number
  text: string
  done: boolean
}

interface TodoItemProps {
  todo: Todo
  onToggle: (id: number) => void
  onRemove: (id: number) => void
}

function TodoItem({ todo, onToggle, onRemove }: TodoItemProps) {
  return (
    <li>
      <label>
        <input type="checkbox" checked={todo.done} onChange={() => onToggle(todo.id)} />
        <span style={{ textDecoration: todo.done ? 'line-through' : 'none' }}>{todo.text}</span>
      </label>
      <button onClick={() => onRemove(todo.id)}>삭제</button>
    </li>
  )
}

// 2. 제네릭 컴포넌트: T에 따라 자동 타입 추론
interface SelectOption<T> {
  value: T
  label: string
}

interface SelectProps<T> {
  value: T
  options: SelectOption<T>[]
  onChange: (value: T) => void
}

function Select<T>({ value, options, onChange }: SelectProps<T>) {
  return (
    <select value={String(value)} onChange={e => {
      // 옵션 값 타입으로 되돌려 onChange에 전달
      const option = options.find(o => String(o.value) === e.target.value)
      if (option) onChange(option.value)
    }}>
      {options.map(o => <option key={String(o.value)} value={String(o.value)}>{o.label}</option>)}
    </select>
  )
}

function App() {
  // 3. useState 타입 명시
  const [todos, setTodos] = useState<Todo[]>([])
  const [text, setText] = useState('')
  const [filter, setFilter] = useState<'all' | 'active' | 'done'>('all')

  // 4. useRef + 이벤트 타입
  const inputRef = useRef<HTMLInputElement>(null)

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    if (!text.trim()) return
    setTodos(list => [...list, { id: Date.now(), text, done: false }])
    setText('')
    inputRef.current?.focus()   // 추가 후 입력창에 포커스 유지
  }

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => setText(e.target.value)

  const toggle = (id: number) =>
    setTodos(list => list.map(t => t.id === id ? { ...t, done: !t.done } : t))

  const remove = (id: number) => setTodos(list => list.filter(t => t.id !== id))

  const visible = todos.filter(t =>
    filter === 'active' ? !t.done : filter === 'done' ? t.done : true
  )

  return (
    <div>
      <h1>TypeScript + React</h1>

      <Select<'all' | 'active' | 'done'>
        value={filter}
        options={[
          { value: 'all', label: '전체' },
          { value: 'active', label: '진행 중' },
          { value: 'done', label: '완료' },
        ]}
        onChange={setFilter}
      />

      <form onSubmit={handleSubmit}>
        <input ref={inputRef} value={text} onChange={handleChange} placeholder="새 할일" />
        <button type="submit">추가</button>
      </form>

      <ul>
        {visible.map(t => <TodoItem key={t.id} todo={t} onToggle={toggle} onRemove={remove} />)}
      </ul>
    </div>
  )
}

export default App
