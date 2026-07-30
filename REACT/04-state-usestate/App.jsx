import { useState } from 'react'

function App() {
  const [count, setCount] = useState(0)
  const [text, setText] = useState('')
  const [items, setItems] = useState(['a', 'b', 'c'])
  const [user, setUser] = useState({ name: '', age: 0 })

  return (
    <div>
      <h1>useState Examples</h1>

      <section>
        <h2>Counter (function update)</h2>
        <p>Count: {count}</p>
        <button onClick={() => setCount(c => c + 1)}>+1</button>
        <button onClick={() => setCount(c => c - 1)}>-1</button>
      </section>

      <section>
        <h2>Text input</h2>
        <input value={text} onChange={e => setText(e.target.value)} />
        <p>You typed: {text}</p>
      </section>

      <section>
        <h2>Array state</h2>
        <button onClick={() => setItems(prev => [...prev, String.fromCharCode(97 + prev.length)])}>
          Add item
        </button>
        <ul>
          {items.map((item, i) => (
            <li key={i}>
              {item}
              <button onClick={() => setItems(prev => prev.filter((_, j) => j !== i))}>
                x
              </button>
            </li>
          ))}
        </ul>
      </section>

      <section>
        <h2>Object state</h2>
        <input placeholder="Name" value={user.name}
          onChange={e => setUser(prev => ({ ...prev, name: e.target.value }))} />
        <input placeholder="Age" type="number" value={user.age}
          onChange={e => setUser(prev => ({ ...prev, age: Number(e.target.value) }))} />
        <p>{user.name}, age {user.age}</p>
      </section>
    </div>
  )
}

export default App
