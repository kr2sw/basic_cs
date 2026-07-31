import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

// Setup 스토어: Composition API 스타일 (ref, computed 그대로 사용)
export const useTodosStore = defineStore('todos', () => {
  const todos = ref([])
  let nextId = 1

  // getters 역할
  const doneCount = computed(() => todos.value.filter(t => t.done).length)
  const totalCount = computed(() => todos.value.length)

  // actions 역할
  function add(text) {
    todos.value.push({ id: nextId++, text, done: false })
  }

  function remove(todoId) {
    todos.value = todos.value.filter(t => t.id !== todoId)
  }

  function toggle(todoId) {
    const todo = todos.value.find(t => t.id === todoId)
    if (todo) todo.done = !todo.done
  }

  return { todos, doneCount, totalCount, add, remove, toggle }
})
