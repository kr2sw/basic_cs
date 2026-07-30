export interface Todo {
  id: number
  title: string
  completed: boolean
  createdAt: string
}

export type CreateTodoDTO = Pick<Todo, 'title'>
export type UpdateTodoDTO = Partial<Pick<Todo, 'title' | 'completed'>>
