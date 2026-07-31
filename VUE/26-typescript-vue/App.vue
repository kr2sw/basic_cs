<template>
  <div class="app">
    <h1>TypeScript + Vue</h1>

    <h2>타입이 적용된 props / emits</h2>
    <p>사용자: {{ user.name }} ({{ user.role }})</p>
    <ul>
      <TodoItem
        :todo="sample"
        @toggle="onToggle"
        @remove="onRemove"
      />
    </ul>
    <p>{{ sample.done ? '완료됨' : '진행 중' }}</p>

    <h2>타입 안전한 입력</h2>
    <input v-model.number="num" type="number" placeholder="숫자만 입력">
    <p>입력값 타입: <code>{{ typeof num }}</code>, 값: {{ num }}</p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import TodoItem from './components/TodoItem.vue'

// 인터페이스로 도메인 타입 정의
interface User {
  name: string
  role: 'admin' | 'user'
}

interface Todo {
  id: number
  text: string
  done: boolean
}

const user = ref<User>({ name: '홍길동', role: 'admin' })
const sample = ref<Todo>({ id: 1, text: 'TypeScript 공부', done: false })

// v-model.number 수식어로 number 타입 유지
const num = ref<number>(10)

function onToggle() {
  sample.value.done = !sample.value.done
}

function onRemove() {
  alert('삭제 요청 (id=' + sample.value.id + ')')
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
input { padding: 4px 8px; }
code { background: #f5f5f5; padding: 2px 6px; border-radius: 4px; }
ul { list-style: none; padding: 0; }
</style>
