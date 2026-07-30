import { useState } from 'react'

export function Counter({ initial = 0 }) {
  const [count, setCount] = useState(initial)

  return (
    <div>
      <p data-testid="count">Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>+1</button>
      <button onClick={() => setCount(c => c - 1)}>-1</button>
      <button onClick={() => setCount(initial)}>Reset</button>
    </div>
  )
}

export function Greeting({ name }) {
  return <h1>Hello, {name}!</h1>
}

function App() {
  const [name, setName] = useState('')

  return (
    <div>
      <h1>Testing Examples</h1>
      <Counter initial={5} />
      <input value={name} onChange={e => setName(e.target.value)} placeholder="Enter name" />
      {name && <Greeting name={name} />}
    </div>
  )
}

export default App
