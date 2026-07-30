<template>
  <div class="app">
    <h1>Composition API</h1>
    <p style="background:#fff3cd;padding:8px 16px;border-radius:4px;">
      이 챕터의 모든 예제는 Composition API로 작성되었습니다.
    </p>

    <h2>Counter (ref)</h2>
    <p>count: {{ count }}</p>
    <button @click="increment">+1</button>
    <button @click="decrement">-1</button>
    <button @click="reset">reset</button>

    <h2>계산된 속성</h2>
    <input v-model="text" placeholder="입력">
    <p>길이: {{ textLength }}</p>
    <p>뒤집기: {{ reversedText }}</p>

    <h2>Todo 리스트</h2>
    <input v-model="newTodo" @keyup.enter="addTodo" placeholder="할 일 입력">
    <ul>
      <li v-for="(todo, i) in todos" :key="i">
        <input type="checkbox" v-model="todo.done">
        <span :class="{ done: todo.done }">{{ todo.text }}</span>
        <button @click="removeTodo(i)">x</button>
      </li>
    </ul>
    <p>남은 할 일: {{ remaining }}</p>

    <h2>라이프사이클</h2>
    <p>{{ lifecycleMsg }}</p>
  </div>
</template>

<script>
import { ref, computed, onMounted, onUnmounted, reactive } from 'vue'

export default {
  setup() {
    const count = ref(0)
    const text = ref('')
    const newTodo = ref('')
    const lifecycleMsg = ref('로딩 중...')

    const todos = reactive([
      { text: 'Vue 공부', done: true },
      { text: '예제 작성', done: false }
    ])

    const textLength = computed(() => text.value.length)
    const reversedText = computed(() => text.value.split('').reverse().join(''))

    const remaining = computed(() => todos.filter(t => !t.done).length)

    function increment() { count.value++ }
    function decrement() { count.value-- }
    function reset() { count.value = 0 }

    function addTodo() {
      if (newTodo.value.trim()) {
        todos.push({ text: newTodo.value.trim(), done: false })
        newTodo.value = ''
      }
    }

    function removeTodo(index) {
      todos.splice(index, 1)
    }

    let timerId
    onMounted(() => {
      lifecycleMsg.value = '✅ 컴포넌트 마운트 완료'
      timerId = setInterval(() => {}, 1000)
    })
    onUnmounted(() => {
      clearInterval(timerId)
    })

    return {
      count, increment, decrement, reset,
      text, textLength, reversedText,
      newTodo, todos, addTodo, removeTodo, remaining,
      lifecycleMsg
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
input { padding: 4px 8px; margin: 4px 0; }
button { margin: 4px; padding: 4px 12px; cursor: pointer; }
.done { text-decoration: line-through; color: #999; }
ul { list-style: none; padding: 0; }
li { padding: 4px 0; display: flex; align-items: center; gap: 8px; }
</style>
