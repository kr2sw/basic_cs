import client from './client'

// todo 도메인 API 레이어
export const todoApi = {
  async fetchAll(limit = 10) {
    const { data } = await client.get(`/todos?_limit=${limit}`)
    return data
  },

  async create(text) {
    const { data } = await client.post('/todos', { title: text, completed: false })
    return data
  },

  async toggle(todo) {
    const { data } = await client.patch(`/todos/${todo.id}`, {
      completed: !todo.completed
    })
    return data
  },

  async remove(id) {
    await client.delete(`/todos/${id}`)
  }
}
