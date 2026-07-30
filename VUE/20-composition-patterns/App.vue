<template>
  <div class="app">
    <h1>Composition Patterns</h1>

    <h2>useCounter</h2>
    <p>count: {{ count }}, double: {{ double }}</p>
    <button @click="increment">+1</button>
    <button @click="decrement">-1</button>
    <button @click="reset">reset</button>

    <h2>useCounter (초기값 10)</h2>
    <p>count: {{ count2 }}</p>
    <button @click="increment2">+1</button>

    <h2>useToggle</h2>
    <p>value: {{ value }}</p>
    <button @click="toggle">토글</button>
    <button @click="setTrue">true</button>
    <button @click="setFalse">false</button>

    <h2>useLocalStorage</h2>
    <p>저장된 이름: {{ name }}</p>
    <input v-model="name" placeholder="이름 입력 (자동 저장)">
    <button @click="name = ''">초기화</button>

    <h2>useMouse (마우스 위치)</h2>
    <div class="mouse-area" @mousemove="onMouseMove">
      마우스를 이 영역에서 움직이세요
    </div>
    <p>X: {{ mouseX }}, Y: {{ mouseY }}</p>

    <h2>useFetch</h2>
    <button @click="loadData">데이터 로드</button>
    <p v-if="loading">로딩 중...</p>
    <pre v-if="data">{{ JSON.stringify(data, null, 2) }}</pre>
    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script>
import { useCounter } from './composables/useCounter'
import { useToggle } from './composables/useToggle'
import { useLocalStorage } from './composables/useLocalStorage'
import { useMouse } from './composables/useMouse'
import { useFetch } from './composables/useFetch'

export default {
  setup() {
    const { count, double, increment, decrement, reset } = useCounter()
    const { count: count2, increment: increment2 } = useCounter(10)
    const { value, toggle, setTrue, setFalse } = useToggle()
    const { value: name } = useLocalStorage('name', '')
    const { x: mouseX, y: mouseY, onMouseMove } = useMouse()
    const { data, loading, error, execute } = useFetch()

    function loadData() {
      execute('https://jsonplaceholder.typicode.com/todos/1')
    }

    return {
      count, double, increment, decrement, reset,
      count2, increment2,
      value, toggle, setTrue, setFalse,
      name,
      mouseX, mouseY, onMouseMove,
      data, loading, error, loadData
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 4px 12px; cursor: pointer; }
input { padding: 4px 8px; }
.mouse-area { background: #f9f9f9; border: 1px solid #ddd; border-radius: 4px; padding: 40px; text-align: center; color: #999; cursor: crosshair; }
pre { background: #f5f5f5; padding: 12px; border-radius: 4px; font-size: 12px; overflow-x: auto; }
.error { color: #dc3545; }
</style>
