import { useState, memo, useMemo, useCallback, lazy, Suspense } from 'react'

const ExpensiveList = memo(function ExpensiveList({ items }) {
  console.log('ExpensiveList render')
  return (
    <ul>
      {items.map((item, i) => <li key={i}>{item}</li>)}
    </ul>
  )
})

const LazyComponent = lazy(() => new Promise(resolve => {
  setTimeout(() => resolve({ default: () => <p>Lazy loaded component!</p> }), 1500)
}))

function App() {
  const [count, setCount] = useState(0)
  const [showLazy, setShowLazy] = useState(false)
  const [search, setSearch] = useState('')

  const items = useMemo(() => {
    return Array.from({ length: 100 }, (_, i) => `Item ${i + 1}${search ? ` (filter: ${search})` : ''}`)
  }, [search])

  const increment = useCallback(() => setCount(c => c + 1), [])

  return (
    <div>
      <h1>Performance</h1>

      <section>
        <h2>React.memo + useCallback</h2>
        <p>Count: {count}</p>
        <button onClick={increment}>Increment (does not re-render list)</button>
      </section>

      <section>
        <h2>useMemo</h2>
        <input value={search} onChange={e => setSearch(e.target.value)} placeholder="Search filter" />
        <ExpensiveList items={items} />
      </section>

      <section>
        <h2>lazy + Suspense</h2>
        <button onClick={() => setShowLazy(v => !v)}>Toggle lazy component</button>
        <Suspense fallback={<p>Loading...</p>}>
          {showLazy && <LazyComponent />}
        </Suspense>
      </section>
    </div>
  )
}

export default App
