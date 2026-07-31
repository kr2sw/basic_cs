<template>
  <div class="app">
    <h1>Composition API 심화</h1>

    <h2>ref vs reactive</h2>
    <p>ref: {{ refCount }} (기본형, .value 사용)</p>
    <p>reactive: {{ state.name }} (객체, 직접 접근)</p>
    <button @click="refCount++">ref +1</button>
    <button @click="state.name = state.name + '!'">reactive 변경</button>

    <h2>함수 분리 (composable)</h2>
    <p>count: {{ counter.count.value }}, double: {{ counter.double.value }}</p>
    <button @click="counter.increment()">+1</button>
    <button @click="counter.decrement()">-1</button>

    <h2>toRefs (반응성 유지 구조 분해)</h2>
    <p>name: {{ name }}, count: {{ count }}</p>
    <button @click="count++">count +1</button>

    <h2>라이프사이클 로그</h2>
    <ul>
      <li v-for="(msg, i) in lifecycleLogs" :key="i" class="log">{{ msg }}</li>
    </ul>
  </div>
</template>

<script setup>
import { ref, reactive, toRefs, computed, onMounted, onUpdated, onBeforeUnmount, onUnmounted } from 'vue'

// ref: 기본형 값
const refCount = ref(0)

// reactive: 객체
const state = reactive({ name: 'Vue', count: 0 })

// toRefs: reactive 객체의 각 속성을 ref로 변환해 구조 분해해도 반응성 유지
const { name, count } = toRefs(state)

// 함수 분리: setup 밖에서 정의해도 반응성은 그대로 동작
function useCounter(initial = 0) {
  const count = ref(initial)
  const double = computed(() => count.value * 2)
  function increment() { count.value++ }
  function decrement() { count.value-- }
  return { count, double, increment, decrement }
}
const counter = useCounter(5)

// 라이프사이클 로그 수집
const lifecycleLogs = ref([])
const log = (msg) => lifecycleLogs.value.push(`${new Date().toLocaleTimeString()} - ${msg}`)

onMounted(() => log('onMounted: DOM 준비 완료'))
onUpdated(() => log('onUpdated: DOM이 업데이트됨'))
onBeforeUnmount(() => log('onBeforeUnmount: 제거 직전 (리소스 정리)'))
onUnmounted(() => log('onUnmounted: 제거 완료'))
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 4px 12px; cursor: pointer; }
ul { list-style: none; padding: 0; }
.log { font-size: 12px; color: #666; padding: 2px 0; }
</style>
