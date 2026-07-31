import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { todoApi } from '../api/todos'

// Setup 스토어: 상태 + 액션 + API 레이어 결합
export const useTodosStore = defineStore('todos', () => {
  const todos = ref([])
  const loading = ref(false)
  const error = ref('')

  // getters: 파생 통계
  const totalCount = computed(() => todos.value.length)
  const doneCount = computed(() => todos.value.filter(t => t.completed).length)
  const progress = computed(() =>
    totalCount.value === 0 ? 0 : Math.round((doneCount.value / totalCount.value) * 100)
  )
  const recent = computed(() => todos.value.slice(0, 5))

  // actions: API 호출 → 상태 갱신
  async function fetchTodos() {
    loading.value = true
    error.value = ''
    try {
      todos.value = await todoApi.fetchAll(10)
    } catch (e) {
      error.value = '로드 실패: ' + e.message
    } finally {
      loading.value = false
    }
  }

  async function addTodo(text) {
    const created = await todoApi.create(text)
    todos.value.unshift(created)
  }

  async function toggleTodo(todo) {
    const updated = await todoApi.toggle(todo)
    const found = todos.value.find(t => t.id === todo.id)
    if (found) Object.assign(found, updated)
  }

  async function removeTodo(id) {
    await todoApi.remove(id)
    todos.value = todos.value.filter(t => t.id !== id)
  }

  return {
    todos, loading, error,
    totalCount, doneCount, progress, recent,
    fetchTodos, addTodo, toggleTodo, removeTodo
  }
})
