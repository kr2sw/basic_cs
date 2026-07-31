# 27: 성능 최적화 — Performance Optimization

`React.memo`, 코드 스플리팅, `Profiler`로 렌더링 비용을 줄이고 병목을 찾는 방법을 배웁니다.

## React.memo — props 비교로 리렌더 방지

`memo`로 감싼 컴포넌트는 **props가 변하지 않으면** 리렌더되지 않습니다. props에 전달되는 함수는 `useCallback`으로 고정해야 memo의 효과가 있습니다.

```jsx
import { memo } from 'react'

const ListItem = memo(function ListItem({ item, onToggle }) {
  return <li onClick={() => onToggle(item.id)}>{item.name}</li>
})
```

## 코드 스플리팅 — 번들 분할

`React.lazy`로 컴포넌트를 분리하면 필요할 때만 번들이 로드됩니다. 처음 진입 시 로드되는 코드가 줄어듭니다.

```jsx
const ExpensiveChart = lazy(() => import('./ExpensiveChart'))

<Suspense fallback={<p>차트 로딩 중...</p>}>
  {show && <ExpensiveChart data={data} />}
</Suspense>
```

## Profiler — 병목 측정

`Profiler`가 자식 트리의 렌더링에 걸린 시간을 콜백으로 알려줍니다. 개발 모드에서만 활성화됩니다.

```jsx
<Profiler id="List" onRender={(id, phase, actualDuration) => {
  console.log(id, phase, actualDuration.toFixed(2) + 'ms')
}}>
  <BigList items={items} />
</Profiler>
```

## 순서: memo -> useMemo -> 코드 스플리팅

가장 먼저 실제 병목을 측정하고(Profiler), 값비싼 재계산에 useMemo, 하위 트리에 memo를 적용한 뒤, 마지막 수단으로 코드를 분할합니다.

Vite는 `build.rollupOptions.output.manualChunks`로 라이브러리 청크를 나눌 수도 있지만, 대부분의 경우 `React.lazy`로 컴포넌트 단위 분할이 먼저입니다.

## 실행

```bash
npm install && npm run dev
```
