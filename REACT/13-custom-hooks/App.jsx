import { useState, useEffect, useCallback } from 'react'

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

function useFetch(url) {
  const [data, setData] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    let cancelled = false
    setLoading(true)
    fetch(url)
      .then(r => { if (!r.ok) throw new Error(`HTTP ${r.status}`); return r.json() })
      .then(d => { if (!cancelled) { setData(d); setError(null) } })
      .catch(e => { if (!cancelled) setError(e.message) })
      .finally(() => { if (!cancelled) setLoading(false) })
    return () => { cancelled = true }
  }, [url])

  return { data, loading, error }
}

function useCounter(initial = 0) {
  const [count, setCount] = useState(initial)
  const increment = useCallback(() => setCount(c => c + 1), [])
  const decrement = useCallback(() => setCount(c => c - 1), [])
  const reset = useCallback(() => setCount(initial), [initial])
  return { count, increment, decrement, reset }
}

function App() {
  const [name, setName] = useLocalStorage('name', '')
  const { data, loading } = useFetch('https://jsonplaceholder.typicode.com/todos/1')
  const { count, increment, reset } = useCounter(10)

  return (
    <div>
      <h1>Custom Hooks</h1>

      <section>
        <h2>useLocalStorage</h2>
        <input value={name} onChange={e => setName(e.target.value)} placeholder="Your name" />
        <p>Saved: {name}</p>
      </section>

      <section>
        <h2>useFetch</h2>
        {loading ? <p>Loading...</p> : <pre>{JSON.stringify(data, null, 2)}</pre>}
      </section>

      <section>
        <h2>useCounter</h2>
        <p>Count: {count}</p>
        <button onClick={increment}>+</button>
        <button onClick={reset}>Reset</button>
      </section>
    </div>
  )
}

export default App
