<template>
  <div>
    <h2>할 일 관리</h2>

    <div class="add-row">
      <input v-model="newTodo" @keyup.enter="addTodo" placeholder="새 할 일 입력">
      <button @click="addTodo" :disabled="!newTodo.trim()">추가</button>
    </div>

    <p v-if="store.loading" class="loading">로딩 중...</p>
    <p v-if="store.error" class="error">{{ store.error }}</p>

    <ul class="todo-list">
      <li v-for="todo in store.todos" :key="todo.id">
        <input
          type="checkbox"
          :checked="todo.completed"
          @change="store.toggleTodo(todo)"
          :aria-label="'완료 처리: ' + todo.title"
        >
        <span :class="{ done: todo.completed }">{{ todo.title }}</span>
        <button class="remove" @click="store.removeTodo(todo.id)">삭제</button>
      </li>
    </ul>

    <p class="hint">
      추가/완료/삭제는 모두 스토어의 actions → API 레이어를 거쳐 서버와 동기화됩니다.
    </p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useTodosStore } from '../store/todos'

const store = useTodosStore()
const newTodo = ref('')

async function addTodo() {
  if (!newTodo.value.trim()) return
  await store.addTodo(newTodo.value.trim())
  newTodo.value = ''
}
</script>

<style scoped>
h2 { color: #333; border-bottom: 1px solid #eee; padding-bottom: 8px; }
.add-row { display: flex; gap: 8px; margin: 12px 0; }
.add-row input { flex: 1; padding: 8px; border: 1px solid #ccc; border-radius: 4px; }
.add-row button { padding: 8px 18px; background: #42b883; color: white; border: none; border-radius: 4px; cursor: pointer; }
.add-row button:disabled { opacity: 0.5; cursor: not-allowed; }
.todo-list { list-style: none; padding: 0; }
.todo-list li { background: white; border: 1px solid #eee; border-radius: 6px; padding: 10px 14px; margin: 6px 0; display: flex; align-items: center; gap: 10px; }
.todo-list span { flex: 1; }
.done { text-decoration: line-through; color: #999; }
.remove { padding: 4px 10px; border: 1px solid #dc3545; color: #dc3545; background: white; border-radius: 4px; cursor: pointer; }
.remove:hover { background: #dc3545; color: white; }
.loading { color: #42b883; }
.error { color: #dc3545; }
.hint { color: #999; font-size: 12px; }
</style>
