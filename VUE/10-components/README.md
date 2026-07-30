# 10: Components — 컴포넌트

## 컴포넌트 (Components)

UI를 재사용 가능한 독립적인 조각으로 나눕니다.

### 컴포넌트 정의

.vue 파일 (SFC):
```vue
<template>
  <div>{{ message }}</div>
</template>
<script>
export default {
  data() { return { message: 'Hello' } }
}
</script>
```

### 지역 등록
```js
import MyComponent from './MyComponent.vue'
export default {
  components: { MyComponent }
}
```

### 전역 등록
```js
app.component('MyComponent', MyComponent)
```

## 컴포넌트 간 통신

| 방향 | 방식 |
|------|------|
| 부모 → 자식 | props |
| 자식 → 부모 | emits (이벤트) |
| 형제 | 공통 부모 통하거나 provide/inject |
| 깊은 관계 | provide/inject 또는 Vuex/Pinia |
