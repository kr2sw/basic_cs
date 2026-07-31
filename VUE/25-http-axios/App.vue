<template>
  <div class="app">
    <h1>HTTP 통신 (Axios)</h1>

    <h2>API 레이어로 할 일 불러오기</h2>
    <p class="hint">인터셉터가 요청/응답을 로그로 남깁니다 (콘솔 확인)</p>
    <button @click="loadTodos" :disabled="loading">새로고침</button>

    <p v-if="loading" class="loading">로딩 중...</p>
    <p v-if="error" class="error">{{ error }}</p>

    <ul>
      <li v-for="todo in todos" :key="todo.id">
        <input type="checkbox" :checked="todo.completed" @change="toggleTodo(todo)">
        <span :class="{ done: todo.completed }">{{ todo.title }}</span>
        <button @click="removeTodo(todo.id)">x</button>
      </li>
    </ul>

    <h2>새 할 일 추가 (POST)</h2>
    <input v-model="newTodo" @keyup.enter="addTodo" placeholder="할 일 입력">
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { todoApi } from './api/todos'

const todos = ref([])
const loading = ref(false)
const error = ref('')
const newTodo = ref('')

// API 레이어를 통해서만 호출
async function loadTodos() {
  loading.value = true
  error.value = ''
  try {
    todos.value = await todoApi.fetchAll(5)
  } catch (e) {
    error.value = '데이터를 불러오지 못했습니다: ' + e.message
  } finally {
    loading.value = false
  }
}

async function toggleTodo(todo) {
  // 낙관적 업데이트 없이 서버 응답을 반영
  const updated = await todoApi.toggle(todo)
  Object.assign(todo, updated)
}

async function addTodo() {
  if (!newTodo.value.trim()) return
  const created = await todoApi.create(newTodo.value.trim())
  todos.value.unshift(created)
  newTodo.value = ''
}

async function removeTodo(id) {
  await todoApi.remove(id)
  todos.value = todos.value.filter(t => t.id !== id)
}

// 마운트 시 최초 로드
onMounted(loadTodos)
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.hint { color: #999; font-size: 12px; }
.loading { color: #42b883; }
.error { color: #dc3545; }
button { margin: 4px; padding: 4px 12px; cursor: pointer; }
input { padding: 4px 8px; }
ul { list-style: none; padding: 0; }
li { padding: 4px 0; display: flex; align-items: center; gap: 8px; }
.done { text-decoration: line-through; color: #999; }
</style>
