import { Todo, CreateTodoDTO, UpdateTodoDTO } from './types'

let nextId = 1
const todos: Todo[] = [
  { id: nextId++, title: 'Learn TypeScript', completed: false, createdAt: new Date().toISOString() },
]

export function getAll(): Todo[] {
  return todos
}

export function getById(id: number): Todo | undefined {
  return todos.find(t => t.id === id)
}

export function create(dto: CreateTodoDTO): Todo {
  const todo: Todo = {
    id: nextId++,
    title: dto.title,
    completed: false,
    createdAt: new Date().toISOString(),
  }
  todos.push(todo)
  return todo
}

export function update(id: number, dto: UpdateTodoDTO): Todo | null {
  const todo = todos.find(t => t.id === id)
  if (!todo) return null
  if (dto.title !== undefined) todo.title = dto.title
  if (dto.completed !== undefined) todo.completed = dto.completed
  return todo
}

export function remove(id: number): boolean {
  const index = todos.findIndex(t => t.id === id)
  if (index === -1) return false
  todos.splice(index, 1)
  return true
}
