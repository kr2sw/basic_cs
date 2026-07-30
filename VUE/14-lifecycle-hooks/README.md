# 14: Lifecycle Hooks — 생명주기 훅

## Vue 3 라이프사이클

```
  setup()
    │
    ▼
  onBeforeMount()  ← 템플릿 컴파일 완료, DOM 아직 없음
    │
    ▼
  onMounted()      ← DOM 생성 완료 (DOM 접근 가능)
    │
    ▼
  onBeforeUpdate() ← 데이터 변경, DOM 업데이트 전
    │
    ▼
  onUpdated()      ← DOM 업데이트 완료
    │
    ▼
  onBeforeUnmount()← 컴포넌트 제거 직전
    │
    ▼
  onUnmounted()    ← 컴포넌트 제거 완료

  onActivated()    ← <KeepAlive> 캐시된 컴포넌트 재활성화
  onDeactivated()  ← <KeepAlive> 비활성화
  onErrorCaptured()← 하위 컴포넌트 에러 캐치
```

## Options API vs Composition API

| Options API | Composition API |
|------------|----------------|
| `beforeMount` | `onBeforeMount` |
| `mounted` | `onMounted` |
| `beforeUpdate` | `onBeforeUpdate` |
| `updated` | `onUpdated` |
| `beforeUnmount` | `onBeforeUnmount` |
| `unmounted` | `onUnmounted` |
