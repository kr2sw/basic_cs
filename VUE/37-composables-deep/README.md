# 37: 컴포저블 심화 — useMouse, useFetch 구현, 패턴

## 컴포저블이란?

Composition API 함수로 상태와 로직을 캡슐화해 여러 컴포넌트에서 재사용합니다.
규칙: `use`로 시작하는 이름, 반환 값은 `ref`/`computed`/함수.

## useMouse 구현

```js
// composables/useMouse.js
import { ref, onMounted, onUnmounted } from 'vue'

export function useMouse() {
  const x = ref(0)
  const y = ref(0)

  function update(e) {
    x.value = e.clientX
    y.value = e.clientY
  }

  // 마운트 시 리스너 등록, 제거 시 해제 (생명주기 관리)
  onMounted(() => window.addEventListener('mousemove', update))
  onUnmounted(() => window.removeEventListener('mousemove', update))

  return { x, y }
}
```

## useFetch 구현

```js
// composables/useFetch.js
export function useFetch(url) {
  const data = ref(null)
  const loading = ref(false)
  const error = ref(null)

  async function execute() {
    loading.value = true
    error.value = null
    try {
      data.value = await (await fetch(url)).json()
    } catch (e) {
      error.value = e
    } finally {
      loading.value = false
    }
  }

  return { data, loading, error, execute }
}
```

## 좋은 패턴 정리

| 패턴 | 설명 |
|------|------|
| 단일 책임 | 하나의 composable은 하나의 관심사 |
| 명명 규칙 | `use` 접두사, 동사형 함수 |
| 반환 규칙 | 항상 객체로 반환해 구조 분해 |
| 정리(cleanup) | `onUnmounted`에서 리스너/타이머 해제 |
| 매개변수 | 옵션 객체를 받아 확장성 확보 |

## 실행

```bash
npm install && npx vite serve .
```
