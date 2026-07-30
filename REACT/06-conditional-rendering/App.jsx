import { useState } from 'react'

function StatusBadge({ status }) {
  if (status === 'online') return <span style={{ color: 'green' }}>Online</span>
  if (status === 'away') return <span style={{ color: 'orange' }}>Away</span>
  return <span style={{ color: 'gray' }}>Offline</span>
}

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false)
  const [items, setItems] = useState([])
  const [status, setStatus] = useState('online')

  return (
    <div>
      <h1>Conditional Rendering</h1>

      <section>
        <h2>if/else component</h2>
        <select value={status} onChange={e => setStatus(e.target.value)}>
          <option value="online">Online</option>
          <option value="away">Away</option>
          <option value="offline">Offline</option>
        </select>
        <StatusBadge status={status} />
      </section>

      <section>
        <h2>&& operator</h2>
        <button onClick={() => setIsLoggedIn(v => !v)}>
          {isLoggedIn ? 'Logout' : 'Login'}
        </button>
        {isLoggedIn && <p>Welcome back! You are logged in.</p>}
      </section>

      <section>
        <h2>Ternary operator for className</h2>
        <button onClick={() => setItems(prev => prev.length === 0 ? ['Item 1', 'Item 2'] : [])}>
          {items.length === 0 ? 'Load items' : 'Clear items'}
        </button>
        <ul>
          {items.length > 0 ? (
            items.map((item, i) => <li key={i}>{item}</li>)
          ) : (
            <li style={{ color: '#999' }}>No items</li>
          )}
        </ul>
      </section>
    </div>
  )
}

export default App
