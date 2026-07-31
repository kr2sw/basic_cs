<template>
  <div class="app">
    <h1>Pinia 상태 관리</h1>

    <h2>카운터 (Option 스토어: state + getters + actions)</h2>
    <p>count: {{ store.count }}</p>
    <p>double (getter): {{ store.double }}, isPositive: {{ store.isPositive }}</p>
    <button @click="store.increment()">+1</button>
    <button @click="store.decrement()">-1</button>
    <button @click="store.incrementBy(5)">+5</button>
    <button @click="store.$reset()">reset</button>

    <h2>할 일 (Setup 스토어: Composition API 스타일)</h2>
    <input v-model="newTodo" @keyup.enter="addTodo" placeholder="할 일 입력">
    <ul>
      <li v-for="todo in todos" :key="todo.id">
        <input type="checkbox" :checked="todo.done" @change="todosStore.toggle(todo.id)">
        <span :class="{ done: todo.done }">{{ todo.text }}</span>
        <button @click="todosStore.remove(todo.id)">x</button>
      </li>
    </ul>
    <p>완료: {{ todosStore.doneCount }} / 전체: {{ todosStore.totalCount }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useCounterStore } from './store/counter'
import { useTodosStore } from './store/todos'

// Option 스토어 사용
const store = useCounterStore()

// Setup 스토어 사용
const todosStore = useTodosStore()
const newTodo = ref('')

// 템플릿에서 바로 사용할 수 있도록 반응형 배열만 노출
const todos = todosStore.todos

function addTodo() {
  if (newTodo.value.trim()) {
    todosStore.add(newTodo.value.trim())
    newTodo.value = ''
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 4px 12px; cursor: pointer; }
input { padding: 4px 8px; }
ul { list-style: none; padding: 0; }
li { padding: 4px 0; display: flex; align-items: center; gap: 8px; }
.done { text-decoration: line-through; color: #999; }
</style>
