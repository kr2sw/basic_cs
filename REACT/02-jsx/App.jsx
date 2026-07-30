function App() {
  const user = { name: 'Alice', age: 25, isAdmin: true }
  const items = ['Apple', 'Banana', 'Cherry']

  return (
    <>
      <h1>JSX Expressions</h1>

      <p>Name: {user.name}</p>
      <p>Age: {user.age + 5}</p>
      <p>Uppercase: {user.name.toUpperCase()}</p>

      <p>Admin: {user.isAdmin ? 'Yes' : 'No'}</p>

      <p>Items count: {items.length}</p>
      <ul>
        {items.map((item, i) => <li key={i}>{item}</li>)}
      </ul>

      <div className={user.isAdmin ? 'admin' : 'user'}>
        Dynamic class name
      </div>
    </>
  )
}

export default App
