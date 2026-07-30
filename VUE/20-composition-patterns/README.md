# 20: Composition Patterns — 컴포지션 패턴

## Composables (컴포저블)

Composition API를 활용한 재사용 가능한 함수입니다.

### useCounter.js
```js
import { ref, computed } from 'vue'
export function useCounter(initial = 0) {
  const count = ref(initial)
  const double = computed(() => count.value * 2)
  function increment() { count.value++ }
  function decrement() { count.value-- }
  return { count, double, increment, decrement }
}
```

### 사용
```js
import { useCounter } from './composables/useCounter'
const { count, double, increment, decrement } = useCounter(10)
```

## Composition 패턴 예제

- 커스텀 훅 (useFetch, useLocalStorage, useMouse)
- 로직 분리 및 재사용
- 단일 책임 원칙
