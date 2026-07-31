# 21: Composition API 심화 — Lifecycle, ref vs reactive, 함수 분리

## setup()의 실행 순서

`<script setup>`은 컴포넌트 생성 시 가장 먼저 실행됩니다.
이 시점에는 아직 DOM이 없으므로 DOM 접근은 반드시 `onMounted` 이후에 해야 합니다.

```js
<script setup>
// 1. 상태와 함수를 정의한다 (setup 스코프)
const count = ref(0)

// 2. 마운트 이후 실행
onMounted(() => {
  console.log('DOM 준비 완료')
})
</script>
```

## ref vs reactive

| 구분 | `ref` | `reactive` |
|------|-------|-----------|
| 대상 | 기본형(숫자, 문자열, 불리언) | 객체(Object, Array) |
| 접근 | `.value` 사용 | 속성 직접 접근 |
| 구조 분해 | 가능 | 불가능 (반응성 유실) |
| 템플릿 | 자동 언래핑 (`{{ count }}`) | 자동 |

- `reactive` 객체를 구조 분해하면 반응성이 사라지므로 `toRefs()`로 변환해야 합니다.
- 처음부터 `ref`로 시작하는 것을 권장하며, 객체는 `ref({ ... })`로 감싸는 방법도 흔합니다.

```js
const state = reactive({ name: 'Vue', count: 0 })
const { name, count } = toRefs(state) // 반응성 유지
```

## 함수 분리 (composable)

setup 내부가 비대해지면 관련 로직을 `useXxx()` 형태의 함수로 분리합니다.
컴포넌트 간 재사용이 가능해지고 테스트가 쉬워집니다.

```js
// useCounter.js
export function useCounter(initial = 0) {
  const count = ref(initial)
  const double = computed(() => count.value * 2)

  function increment() { count.value++ }
  function decrement() { count.value-- }

  return { count, double, increment, decrement }
}
```

```js
// 컴포넌트에서 사용
const { count, double, increment } = useCounter(10)
```

## 라이프사이클 훅 정리

| 훅 | 시점 | 주요 용도 |
|----|------|----------|
| `onMounted` | DOM 생성 완료 | API 호출, DOM 조작 |
| `onBeforeUnmount` | 컴포넌트 제거 직전 | setInterval 해제, 리스너 제거 |
| `onUnmounted` | 제거 완료 | 최종 정리 |
| `onErrorCaptured` | 하위 에러 발생 | 에러 로깅, fallback UI |
| `onActivated` | KeepAlive 재활성화 | 캐시된 컴포넌트 초기화 |

훅은 반드시 setup 컨텍스트 안에서 **동기적으로** 호출되어야 합니다.

## 실행

```bash
npm install && npx vite serve .
```
