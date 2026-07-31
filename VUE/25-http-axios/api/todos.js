import client from './client'

// API 레이어: 컴포넌트는 이 모듈만 호출 (URL 분리, 데이터 가공 담당)
export const todoApi = {
  async fetchAll(limit = 5) {
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
