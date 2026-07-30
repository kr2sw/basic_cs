import express, { Request, Response } from 'express'
import { getAll, getById, create, update, remove } from './todoService'
import { CreateTodoDTO, UpdateTodoDTO } from './types'

const app = express()
app.use(express.json())

app.get('/api/todos', (_req: Request, res: Response) => {
  res.json(getAll())
})

app.get('/api/todos/:id', (req: Request<{ id: string }>, res: Response) => {
  const todo = getById(Number(req.params.id))
  if (!todo) {
    res.status(404).json({ error: 'Todo not found' })
    return
  }
  res.json(todo)
})

app.post('/api/todos', (req: Request<{}, {}, CreateTodoDTO>, res: Response) => {
  if (!req.body.title?.trim()) {
    res.status(400).json({ error: 'Title is required' })
    return
  }
  const todo = create(req.body)
  res.status(201).json(todo)
})

app.put('/api/todos/:id', (req: Request<{ id: string }, {}, UpdateTodoDTO>, res: Response) => {
  const todo = update(Number(req.params.id), req.body)
  if (!todo) {
    res.status(404).json({ error: 'Todo not found' })
    return
  }
  res.json(todo)
})

app.delete('/api/todos/:id', (req: Request<{ id: string }>, res: Response) => {
  const deleted = remove(Number(req.params.id))
  if (!deleted) {
    res.status(404).json({ error: 'Todo not found' })
    return
  }
  res.status(204).send()
})

app.listen(3000, () => {
  console.log('Todo API running on http://localhost:3000')
})
