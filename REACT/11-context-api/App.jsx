import { createContext, useContext, useState } from 'react'

const ThemeContext = createContext()
const UserContext = createContext()

function ThemeToggle() {
  const { theme, toggleTheme } = useContext(ThemeContext)
  return (
    <button onClick={toggleTheme}
      style={{ background: theme === 'dark' ? '#333' : '#eee', color: theme === 'dark' ? '#fff' : '#000' }}>
      Current: {theme}
    </button>
  )
}

function UserInfo() {
  const user = useContext(UserContext)
  return <p>Logged in as: {user.name} ({user.role})</p>
}

function Profile() {
  return (
    <div>
      <ThemeToggle />
      <UserInfo />
    </div>
  )
}

function App() {
  const [theme, setTheme] = useState('light')
  const toggleTheme = () => setTheme(t => t === 'light' ? 'dark' : 'light')

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      <UserContext.Provider value={{ name: 'Alice', role: 'Admin' }}>
        <div style={{ padding: 20, background: theme === 'dark' ? '#222' : '#fff', color: theme === 'dark' ? '#fff' : '#000', minHeight: '100vh' }}>
          <h1>Context API</h1>
          <Profile />
        </div>
      </UserContext.Provider>
    </ThemeContext.Provider>
  )
}

export default App
