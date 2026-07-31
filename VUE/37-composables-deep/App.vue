<template>
  <div class="app">
    <h1>컴포저블 심화</h1>

    <h2>useMouse (리스너 자동 등록/해제)</h2>
    <div class="mouse-area">마우스를 움직여 보세요</div>
    <p>X: {{ x }}, Y: {{ y }}</p>

    <h2>useFetch (데이터 로딩 상태 관리)</h2>
    <button @click="execute" :disabled="loading">데이터 불러오기</button>
    <p v-if="loading" class="loading">로딩 중...</p>
    <p v-if="error" class="error">{{ error }}</p>
    <pre v-if="data">{{ JSON.stringify(data, null, 2) }}</pre>

    <h2>useLocalStorage (상태 영속화)</h2>
    <input v-model="name" placeholder="이름 입력 (자동 저장)">
    <p>저장된 이름: {{ name }}</p>
  </div>
</template>

<script setup>
import { useMouse } from './composables/useMouse'
import { useFetch } from './composables/useFetch'
import { useLocalStorage } from './composables/useLocalStorage'

const { x, y } = useMouse()

const { data, loading, error, execute } = useFetch(
  'https://jsonplaceholder.typicode.com/todos/1'
)

const { value: name } = useLocalStorage('name', '')
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { padding: 6px 14px; cursor: pointer; }
input { padding: 4px 8px; }
.mouse-area { background: #f9f9f9; border: 1px solid #ddd; border-radius: 4px; padding: 40px; text-align: center; color: #999; }
.loading { color: #42b883; }
.error { color: #dc3545; }
pre { background: #f5f5f5; padding: 12px; border-radius: 4px; font-size: 12px; }
</style>
