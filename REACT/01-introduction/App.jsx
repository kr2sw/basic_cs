function Welcome() {
  return <h1>Hello, React!</h1>
}

function Intro() {
  const name = 'React'
  const version = 19
  return (
    <div>
      <Welcome />
      <p>{name} version {version} - UI library for building user interfaces</p>
      <p>Current time: {new Date().toLocaleTimeString()}</p>
    </div>
  )
}

export default Intro
