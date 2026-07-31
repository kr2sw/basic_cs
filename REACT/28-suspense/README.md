# 28: Suspense와 동시성 — Suspense & Concurrent Features

`lazy`, `Suspense`, `useTransition`, `useDeferredValue`로 비동기 UI를 선언적으로 그리고 렌더링을 부드럽게 유지합니다.

## React.lazy + Suspense

`lazy`로 만든 컴포넌트는 로드가 끝날 때까지 `Suspense`의 `fallback`이 대신 표시됩니다. 로딩 로직이 컴포넌트 바깥(선언적)에 있습니다.

```jsx
import { lazy, Suspense } from 'react'

const SlowProfile = lazy(() => import('./SlowProfile'))

<Suspense fallback={<p>프로필 로딩 중...</p>}>
  <SlowProfile userId={id} />
</Suspense>
```

여러 lazy 컴포넌트는 중첩된 Suspense로 개별 fallback을 가질 수 있습니다.

## useTransition — 긴급/비긴급 업데이트 구분

상태 업데이트를 "긴급(타이핑)"과 "비긴급(목록 갱신)"으로 나눕니다. 비긴급 업데이트는 백그라운드에서 수행되고, 그동안 기존 UI가 반응을 유지합니다.

```jsx
const [isPending, startTransition] = useTransition()
startTransition(() => setTab('comments'))  // 비긴급 처리
```

## useDeferredValue — 값 지연

다른 계산에 바쁜 하위 트리 때문에 입력이 버벅일 때, 검색어가 아닌 **지연된 값**으로 무거운 리스트를 필터링합니다.

```jsx
const deferredQuery = useDeferredValue(query)
const results = useMemo(() => bigList.filter(x => x.includes(deferredQuery)), [deferredQuery])
```

`deferredQuery !== query`인 동안은 아직 화면에 이전 결과가 보이므로, 그때 "처리 중" 표시를 띄울 수 있습니다.

## useTransition vs fallback

- **fallback**(Suspense)은 컴포넌트가 아직 로드되지 않아 *대체 화면*을 보여줍니다.
- **isPending**(useTransition)은 화면이 있는 채로 백그라운드 갱신이 진행 중임을 알립니다.
- 결합 예: 탭 전환은 `startTransition`으로 감싸고, lazy 컴포넌트만 `Suspense`로 감쌉니다. 둘을 함께 쓰면 "있던 화면 유지 → 준비되면 교체"가 됩니다.

## Suspense와 데이터 페칭

Suspense는 lazy만을 위한 것이 아닙니다. 데이터를 "읽는 동안 suspend"하는 훅(`use` 또는 서드파티 쿼리 라이브러리의 suspense 모드)과 함께 쓰면 로딩 UI를 선언적으로 관리합니다. 이 패턴은 SSR의 `streaming`과 결합하면 특히 유용합니다.

## 실행

```bash
npm install && npm run dev
```
