import { useState, useRef, useEffect, forwardRef } from 'react'

const FancyInput = forwardRef((props, ref) => (
  <input ref={ref} style={{ border: '2px solid blue', padding: 8 }} {...props} />
))

function App() {
  const inputRef = useRef(null)
  const countRef = useRef(0)
  const prevRef = useRef(null)
  const [count, setCount] = useState(0)

  useEffect(() => {
    prevRef.current = count
  }, [count])

  function focusInput() {
    inputRef.current.focus()
  }

  return (
    <div>
      <h1>useRef & DOM</h1>

      <section>
        <h2>DOM ref</h2>
        <FancyInput ref={inputRef} placeholder="Fancy input" />
        <button onClick={focusInput}>Focus input</button>
      </section>

      <section>
        <h2>Mutable ref (no re-render)</h2>
        <p>Count: {count}</p>
        <p>Previous count: {prevRef.current}</p>
        <button onClick={() => {
          countRef.current++
          console.log('countRef:', countRef.current)
          setCount(c => c + 1)
        }}>Increment</button>
        <button onClick={() => console.log('Ref value:', countRef.current)}>Log ref</button>
      </section>
    </div>
  )
}

export default App
