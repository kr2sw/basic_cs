import { Routes, Route, Link, useParams, useSearchParams } from 'react-router-dom'

function Home() {
  return <h2>Home Page</h2>
}

function About() {
  return <h2>About Page</h2>
}

function User() {
  const { id } = useParams()
  return <h2>User ID: {id}</h2>
}

function Search() {
  const [params] = useSearchParams()
  return <h2>Search: {params.get('q') || '(none)'}</h2>
}

function App() {
  return (
    <div>
      <h1>React Router</h1>
      <nav>
        <Link to="/">Home</Link> | <Link to="/about">About</Link> | <Link to="/user/42">User 42</Link> | <Link to="/search?q=react">Search</Link>
      </nav>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/user/:id" element={<User />} />
        <Route path="/search" element={<Search />} />
        <Route path="*" element={<h2>404 Not Found</h2>} />
      </Routes>
    </div>
  )
}

export default App
