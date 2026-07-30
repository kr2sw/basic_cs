import { useState } from 'react'

const initialTodos = [
  { id: 1, text: 'Learn React', done: false },
  { id: 2, text: 'Build a project', done: false },
  { id: 3, text: 'Deploy to production', done: false },
]

function App() {
  const [todos, setTodos] = useState(initialTodos)
  const [filter, setFilter] = useState('all')

  function toggleTodo(id) {
    setTodos(prev => prev.map(t => t.id === id ? { ...t, done: !t.done } : t))
  }

  const filtered = todos.filter(t => {
    if (filter === 'done') return t.done
    if (filter === 'active') return !t.done
    return true
  })

  return (
    <div>
      <h1>Lists & Keys</h1>

      <div>
        <button onClick={() => setFilter('all')}>All</button>
        <button onClick={() => setFilter('active')}>Active</button>
        <button onClick={() => setFilter('done')}>Done</button>
      </div>

      <ul>
        {filtered.map(todo => (
          <li key={todo.id}
            style={{ textDecoration: todo.done ? 'line-through' : 'none', cursor: 'pointer' }}
            onClick={() => toggleTodo(todo.id)}>
            {todo.text}
          </li>
        ))}
      </ul>

      <p>Total: {todos.length} | Active: {todos.filter(t => !t.done).length} | Done: {todos.filter(t => t.done).length}</p>
    </div>
  )
}

export default App
