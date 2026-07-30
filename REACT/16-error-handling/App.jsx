import { Component, useState } from 'react'

class ErrorBoundary extends Component {
  constructor(props) {
    super(props)
    this.state = { error: null }
  }
  static getDerivedStateFromError(error) {
    return { error }
  }
  render() {
    if (this.state.error) {
      return (
        <div style={{ border: '2px solid red', padding: 16, margin: 8 }}>
          <h2>Something went wrong</h2>
          <p>{this.state.error.message}</p>
          <button onClick={() => this.setState({ error: null })}>Try again</button>
        </div>
      )
    }
    return this.props.children
  }
}

function BuggyComponent({ shouldThrow }) {
  if (shouldThrow) throw new Error('Intentional crash!')
  return <p>Everything is fine here.</p>
}

function App() {
  const [crash, setCrash] = useState(false)
  const [asyncError, setAsyncError] = useState('')

  function handleAsyncError() {
    try {
      throw new Error('Async error caught!')
    } catch (e) {
      setAsyncError(e.message)
    }
  }

  return (
    <div>
      <h1>Error Handling</h1>

      <section>
        <h2>Error Boundary</h2>
        <ErrorBoundary key={crash}>
          <BuggyComponent shouldThrow={crash} />
          <button onClick={() => setCrash(true)}>Crash</button>
        </ErrorBoundary>
      </section>

      <section>
        <h2>Async error (try/catch)</h2>
        <button onClick={handleAsyncError}>Cause async error</button>
        {asyncError && <p style={{ color: 'red' }}>{asyncError}</p>}
      </section>
    </div>
  )
}

export default App
