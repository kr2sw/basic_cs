import { useState } from 'react'

function App() {
  const [clicks, setClicks] = useState(0)
  const [text, setText] = useState('')
  const [submitted, setSubmitted] = useState('')

  function handleClick(msg) {
    setClicks(c => c + 1)
    console.log(msg)
  }

  function handleSubmit(e) {
    e.preventDefault()
    setSubmitted(text)
    setText('')
  }

  return (
    <div>
      <h1>Event Handling</h1>

      <section>
        <h2>Click events</h2>
        <p>Clicks: {clicks}</p>
        <button onClick={() => handleClick('Button A clicked')}>Button A</button>
        <button onClick={e => handleClick(`Button B at (${e.clientX},${e.clientY})`)}>Button B</button>
      </section>

      <section>
        <h2>Form submit</h2>
        <form onSubmit={handleSubmit}>
          <input value={text} onChange={e => setText(e.target.value)} placeholder="Type something" />
          <button type="submit">Submit</button>
        </form>
        {submitted && <p>Submitted: {submitted}</p>}
      </section>

      <section>
        <h2>Mouse events</h2>
        <div style={{ width: 200, height: 100, background: '#eee', display: 'flex', alignItems: 'center', justifyContent: 'center' }}
          onMouseEnter={() => console.log('Mouse entered')}
          onMouseLeave={() => console.log('Mouse left')}>
          Hover me (check console)
        </div>
      </section>
    </div>
  )
}

export default App
