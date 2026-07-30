import { useState } from 'react'

function App() {
  const [form, setForm] = useState({ name: '', email: '', role: 'user', agree: false })
  const [errors, setErrors] = useState({})

  function validate() {
    const e = {}
    if (!form.name.trim()) e.name = 'Name is required'
    if (!form.email.includes('@')) e.email = 'Invalid email'
    if (!form.agree) e.agree = 'You must agree'
    return e
  }

  function handleSubmit(e) {
    e.preventDefault()
    const errs = validate()
    setErrors(errs)
    if (Object.keys(errs).length === 0) {
      console.log('Submitted:', form)
      setForm({ name: '', email: '', role: 'user', agree: false })
    }
  }

  function set(field, value) {
    setForm(prev => ({ ...prev, [field]: value }))
    setErrors(prev => ({ ...prev, [field]: undefined }))
  }

  return (
    <div>
      <h1>Forms</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Name: <input value={form.name} onChange={e => set('name', e.target.value)} /></label>
          {errors.name && <span style={{ color: 'red' }}>{errors.name}</span>}
        </div>
        <div>
          <label>Email: <input value={form.email} onChange={e => set('email', e.target.value)} /></label>
          {errors.email && <span style={{ color: 'red' }}>{errors.email}</span>}
        </div>
        <div>
          <label>Role:
            <select value={form.role} onChange={e => set('role', e.target.value)}>
              <option value="user">User</option>
              <option value="admin">Admin</option>
              <option value="editor">Editor</option>
            </select>
          </label>
        </div>
        <div>
          <label>
            <input type="checkbox" checked={form.agree} onChange={e => set('agree', e.target.checked)} />
            I agree to terms
          </label>
          {errors.agree && <span style={{ color: 'red' }}>{errors.agree}</span>}
        </div>
        <button type="submit">Submit</button>
      </form>
      <pre>{JSON.stringify(form, null, 2)}</pre>
    </div>
  )
}

export default App
