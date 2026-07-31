# 28: Teleport와 Suspense — 모달, 비동기 컴포넌트

## Teleport

`<Teleport to="...">`는 컴포넌트의 DOM을 특정 위치(예: `<body>`)로 옮겨 렌더링합니다.
z-index 문제가 흔한 모달, 툴팁, 알림에서 유용합니다.

```vue
<Teleport to="body">
  <div v-if="open" class="modal">
    <!-- 이 DOM은 #app 밖 body로 이동 -->
  </div>
</Teleport>
```

- `to` 속성: `"body"`, `"#app"`, `".class"` 등 CSS 선택자
- `disabled`: 조건부로 Teleport 비활성화 가능

```vue
<!-- 조건에 따라 텔레포트하거나 원위치에 둠 -->
<Teleport to="body" :disabled="isMobile">
  <Tooltip />
</Teleport>
```

## 중첩 Suspense

하위 컴포넌트마다 별도의 Suspense를 두면 각 비동기 영역을 독립적으로 처리합니다.
`@error` 훅과 `onErrorCaptured`로 실패 시 처리를 추가할 수 있습니다.

```vue
<Suspense @error="handleError">
  <template #default><AsyncSection /></template>
  <template #fallback><Skeleton /></template>
</Suspense>
```

## Suspense

비동기 컴포넌트의 로딩 상태를 선언적으로 처리합니다.
`<script setup>`에서 `await`를 사용하면 컴포넌트가 자동으로 "pending" 상태가 됩니다.

```vue
<Suspense>
  <template #default>
    <AsyncProfile /> <!-- async setup 컴포넌트 -->
  </template>
  <template #fallback>
    <p>로딩 중...</p>
  </template>
</Suspense>
```

## defineAsyncComponent

컴포넌트를 지연 로딩할 때 사용합니다. `Suspense`와 함께 쓰면
네트워크가 느린 환경에서도 좋은 UX를 제공합니다.

```js
const AsyncProfile = defineAsyncComponent(() =>
  import('./components/AsyncProfile.vue')
)
```

## 사용 시나리오

| API | 사용처 |
|-----|--------|
| `Teleport` | 모달, 드롭다운, 툴팁, 글로벌 알림 |
| `Suspense` | async setup, 비동기 데이터 로딩 UI |
| `defineAsyncComponent` | 코드 스플리팅, 무거운 컴포넌트 지연 |

## 실행

```bash
npm install && npx vite serve .
```
