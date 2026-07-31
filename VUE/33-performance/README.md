# 33: 성능 최적화 — defineAsyncComponent, memoization, v-memo

## defineAsyncComponent (지연 로딩)

무거운 컴포넌트는 진입 시 함께 로드하면 초기 번들이 커집니다.
`defineAsyncComponent`로 코드를 분할해 필요할 때만 로드합니다.

```js
const HeavyComponent = defineAsyncComponent(() =>
  import('./components/Heavy.vue')
)
```

## Memoization (계산 결과 캐시)

`computed`는 의존 값이 변경될 때만 재계산됩니다. 무거운 계산을
함수 대신 `computed`로 두면 반복 렌더링 시 비용을 줄입니다.

```js
const filtered = computed(() =>
  items.value.filter(i => i.priority === 'high')
) // items가 안 바뀌면 재실행하지 않음
```

## v-memo

리스트 항목의 특정 의존성이 바뀌지 않으면 하위 VNode 재생성을 건너뜁니다.

```vue
<li v-for="todo in todos" :key="todo.id" v-memo="[todo.done, todo.text]">
  <!-- done/text가 그대로면 하위 렌더링 생략 -->
</li>
```

주의: `v-memo`를 쓰면 해당 항목의 다른 상태가 바뀌어도 반영되지 않을 수 있으므로
의존성 배열에 포함된 값만 항목에서 사용해야 합니다.

## shallowRef (깊은 반응성 제거)

대량 데이터에서 내부 변경 추적이 필요 없으면 `shallowRef`로 오버헤드를 줄입니다.

```js
const bigList = shallowRef([{...}, ...10000개])
```

## KeepAlive (컴포넌트 상태 보존)

컴포넌트를 전환해도 상태를 유지하고 파괴를 지연합니다.

```vue
<KeepAlive>
  <component :is="currentTab" />
</KeepAlive>
```

`include`/`exclude` prop으로 캐시 대상을 제한할 수 있습니다.

## 디바운스/쓰로틀

빈번한 이벤트(스크롤, 입력)를 제한해 렌더링 비용을 줄입니다.

```js
function debounce(fn, wait = 300) {
  let timer = null
  return (...args) => {
    clearTimeout(timer)
    timer = setTimeout(() => fn(...args), wait)
  }
}

const onSearch = debounce((q) => {
  results.value = search(q)
})
```

## 기타 기법

- `KeepAlive`: 컴포넌트 상태 보존
- 디바운스/쓰로틀: 이벤트 핸들러 호출 제한
- 가상 스크롤: 대량 리스트 렌더링 최적화
- Tree-shaking 친화적인 import (named import)

## 실행

```bash
npm install && npx vite serve .
```
