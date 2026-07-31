<template>
  <div class="app">
    <h1>성능 최적화</h1>

    <h2>defineAsyncComponent (지연 로딩)</h2>
    <button @click="showHeavy = true">무거운 컴포넌트 로드</button>
    <p class="hint">네트워크 탭에서 별도 청크가 요청되는 것을 확인하세요.</p>
    <HeavyComponent v-if="showHeavy" />

    <h2>computed memoization</h2>
    <p>high 우선순위 수: {{ highCount }}</p>
    <p class="hint">items가 변경될 때만 재계산됩니다.</p>
    <button @click="addItem">아이템 추가 (low)</button>

    <h2>v-memo</h2>
    <button @click="bump">부모 리렌더 (외부 카운트: {{ bumpCount }})</button>
    <ul>
      <li
        v-for="todo in todos"
        :key="todo.id"
        v-memo="[todo.done, todo.text]"
        class="memo-item"
      >
        <input type="checkbox" v-model="todo.done">
        <span :class="{ done: todo.done }">{{ todo.text }}</span>
      </li>
    </ul>
    <p class="hint">
      v-memo 덕분에 done/text가 같으면 부모가 리렌더되어도 항목이 다시 그려지지 않습니다.
    </p>
  </div>
</template>

<script setup>
import { ref, computed, defineAsyncComponent } from 'vue'

// 지연 로딩: 첫 화면에는 로드되지 않고 버튼 클릭 후 로드
const HeavyComponent = defineAsyncComponent(() =>
  import('./components/Heavy.vue')
)
const showHeavy = ref(false)

// memoization: computed는 의존성이 바뀔 때만 재계산
const items = ref([
  { id: 1, priority: 'high' },
  { id: 2, priority: 'low' }
])
const highCount = computed(() => items.value.filter(i => i.priority === 'high').length)

function addItem() {
  items.value.push({ id: items.value.length + 1, priority: 'low' })
}

// v-memo 예제
const todos = ref([
  { id: 1, text: '코드 리뷰', done: false },
  { id: 2, text: '성능 측정', done: false }
])
const bumpCount = ref(0)
function bump() {
  bumpCount.value++
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.hint { color: #999; font-size: 12px; }
button { margin: 4px; padding: 6px 14px; cursor: pointer; }
.memo-item { padding: 4px 0; }
.done { text-decoration: line-through; color: #999; }
</style>
