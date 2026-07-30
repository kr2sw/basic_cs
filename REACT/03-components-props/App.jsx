function UserCard({ name, age, role = 'User', children }) {
  return (
    <div style={{ border: '1px solid #ccc', padding: 16, margin: 8, borderRadius: 8 }}>
      <h2>{name}</h2>
      <p>Age: {age}</p>
      <p>Role: {role}</p>
      {children && <div style={{ marginTop: 8 }}>{children}</div>}
    </div>
  )
}

function App() {
  return (
    <div>
      <h1>Components & Props</h1>
      <UserCard name="Alice" age={25} />
      <UserCard name="Bob" age={30} role="Admin" />
      <UserCard name="Charlie" age={28} role="Editor">
        <p>Extra info about Charlie</p>
      </UserCard>
    </div>
  )
}

export default App
