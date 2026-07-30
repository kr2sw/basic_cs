import { useState, useEffect } from 'react'

function Timer() {
  const [seconds, setSeconds] = useState(0)

  useEffect(() => {
    const id = setInterval(() => setSeconds(s => s + 1), 1000)
    return () => clearInterval(id)
  }, [])

  return <p>Timer: {seconds}s</p>
}

function App() {
  const [count, setCount] = useState(0)
  const [data, setData] = useState(null)
  const [show, setShow] = useState(true)

  useEffect(() => {
    document.title = `Count: ${count}`
  }, [count])

  useEffect(() => {
    fetch('https://jsonplaceholder.typicode.com/todos/1')
      .then(r => r.json())
      .then(d => setData(d))
  }, [])

  return (
    <div>
      <h1>useEffect</h1>

      <section>
        <h2>Document title sync</h2>
        <p>Count: {count}</p>
        <button onClick={() => setCount(c => c + 1)}>Increment</button>
      </section>

      <section>
        <h2>Data fetching</h2>
        <pre>{JSON.stringify(data, null, 2)}</pre>
      </section>

      <section>
        <h2>Timer with cleanup</h2>
        <button onClick={() => setShow(v => !v)}>{show ? 'Hide' : 'Show'} timer</button>
        {show && <Timer />}
      </section>
    </div>
  )
}

export default App
