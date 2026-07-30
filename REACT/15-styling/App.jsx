import { useState } from 'react'

const btnBase = {
  padding: '8px 16px', border: 'none', borderRadius: 4, cursor: 'pointer', fontWeight: 'bold'
}

const btnVariants = {
  primary: { background: '#007bff', color: '#fff' },
  danger: { background: '#dc3545', color: '#fff' },
  ghost: { background: 'transparent', border: '1px solid #ccc' },
}

function Button({ variant = 'primary', children }) {
  return <button style={{ ...btnBase, ...btnVariants[variant] }}>{children}</button>
}

function App() {
  const [isDark, setIsDark] = useState(false)

  return (
    <div>
      <h1>Styling</h1>

      <section>
        <h2>Inline styles</h2>
        <Button variant="primary">Primary</Button>
        <Button variant="danger">Danger</Button>
        <Button variant="ghost">Ghost</Button>
      </section>

      <section>
        <h2>Dynamic className</h2>
        <button onClick={() => setIsDark(v => !v)}>Toggle dark</button>
        <div className={`card${isDark ? ' dark' : ''}`}>
          <p>This card has dynamic className</p>
          <span className="highlight">Highlighted text</span>
        </div>
      </section>
    </div>
  )
}

export default App
