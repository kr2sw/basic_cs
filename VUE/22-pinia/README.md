# 22: 상태 관리 — Pinia store, actions, getters

## Pinia란?

Vue 3 공식 상태 관리 라이브러리입니다. Vuex 4를 대체하며 Composition API와
자연스럽게 통합되고 TypeScript 지원이 뛰어납니다.

```bash
npm install pinia
```

## Store 생성 (Option 스토어)

```js
// store/counter.js
import { defineStore } from 'pinia'

export const useCounterStore = defineStore('counter', {
  state: () => ({ count: 0 }),
  getters: {
    double: (state) => state.count * 2
  },
  actions: {
    increment() { this.count++ },
    incrementBy(n) { this.count += n }
  }
})
```

## Store 생성 (Setup 스토어)

Composition API 스타일로도 작성할 수 있습니다. `ref`, `computed`를 그대로 사용합니다.

```js
// store/todos.js
export const useTodosStore = defineStore('todos', () => {
  const todos = ref([])
  const doneCount = computed(() => todos.value.filter(t => t.done).length)
  function add(text) { todos.value.push({ id: ++id, text, done: false }) }
  return { todos, doneCount, add }
})
```

## 앱에 등록 (main.js)

```js
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'

const app = createApp(App)
app.use(createPinia())
app.mount('#app')
```

## 컴포넌트에서 사용

```js
import { useCounterStore } from './store/counter'

const store = useCounterStore()
store.count        // state 접근
store.double       // getter 접근
store.increment()  // action 호출
```

## state / getters / actions 정리

| 개념 | 역할 | 주의 |
|------|------|------|
| `state` | 반응형 데이터 | 구조 분해 시 `storeToRefs()` 필요 |
| `getters` | 파생 상태 (computed) | 화살표 함수로 `state` 참조 |
| `actions` | 상태 변경 + 비즈니스 로직 | 비동기(async/await) 가능 |

## 실행

```bash
npm install && npx vite serve .
```
