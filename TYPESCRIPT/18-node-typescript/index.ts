import express, { Request, Response, NextFunction } from 'express'

interface User {
  id: number
  name: string
  email: string
}

const app = express()
app.use(express.json())

const users: User[] = [
  { id: 1, name: 'Alice', email: 'alice@test.com' },
  { id: 2, name: 'Bob', email: 'bob@test.com' },
]

app.get('/api/users', (_req: Request, res: Response) => {
  res.json(users)
})

app.get('/api/users/:id', (req: Request<{ id: string }>, res: Response) => {
  const user = users.find(u => u.id === Number(req.params.id))
  if (!user) {
    res.status(404).json({ error: 'User not found' })
    return
  }
  res.json(user)
})

app.post('/api/users', (req: Request<{}, {}, Omit<User, 'id'>>, res: Response) => {
  const newUser: User = { id: users.length + 1, ...req.body }
  users.push(newUser)
  res.status(201).json(newUser)
})

app.use((err: Error, _req: Request, res: Response, _next: NextFunction) => {
  console.error(err.stack)
  res.status(500).json({ error: 'Internal server error' })
})

app.listen(3000, () => console.log('Server running on http://localhost:3000'))
