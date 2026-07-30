# 12: Component Emits — 컴포넌트 이벤트

## emits

자식 컴포넌트가 부모로 이벤트를 보내 통신합니다.

### 선언
```js
// 배열 문법
emits: ['update', 'delete']

// 객체 문법 (검증)
emits: {
  update(payload) {
    return payload.id > 0
  }
}
```

### 발생
```js
// 템플릿
<button @click="$emit('update', { id: 1 })">

// script
this.$emit('update', { id: 1 })
```

## v-model 커스텀

```js
// 자식
props: { modelValue: String }
emits: ['update:modelValue']

// 부모
<Child v-model="value" />
```
