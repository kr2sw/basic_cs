import React, { useState, useRef, ChangeEvent, FormEvent } from 'react'

interface Todo {
  id: number
  text: string
  done: boolean
}

interface TodoItemProps {
  todo: Todo
  onToggle: (id: number) => void
  onDelete: (id: number) => void
}

function TodoItem({ todo, onToggle, onDelete }: TodoItemProps) {
  return (
    <li>
      <span
        style={{ textDecoration: todo.done ? 'line-through' : 'none', cursor: 'pointer' }}
        onClick={() => onToggle(todo.id)}
      >
        {todo.text}
      </span>
      <button onClick={() => onDelete(todo.id)}>x</button>
    </li>
  )
}

function App() {
  const [todos, setTodos] = useState<Todo[]>([])
  const [text, setText] = useState('')
  const nextId = useRef(1)

  function handleSubmit(e: FormEvent) {
    e.preventDefault()
    if (!text.trim()) return
    setTodos(prev => [...prev, { id: nextId.current++, text, done: false }])
    setText('')
  }

  function toggleTodo(id: number) {
    setTodos(prev => prev.map(t => t.id === id ? { ...t, done: !t.done } : t))
  }

  function deleteTodo(id: number) {
    setTodos(prev => prev.filter(t => t.id !== id))
  }

  return (
    <div>
      <h1>React + TypeScript Todos</h1>
      <form onSubmit={handleSubmit}>
        <input value={text} onChange={(e: ChangeEvent<HTMLInputElement>) => setText(e.target.value)} />
        <button type="submit">Add</button>
      </form>
      <ul>
        {todos.map(todo => (
          <TodoItem key={todo.id} todo={todo} onToggle={toggleTodo} onDelete={deleteTodo} />
        ))}
      </ul>
    </div>
  )
}

export default App
