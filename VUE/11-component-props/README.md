# 11: Component Props — 컴포넌트 Props

## props 선언

### 문자열 배열
```js
props: ['name', 'age']
```

### 객체 문법 (타입 검증)
```js
props: {
  name: { type: String, required: true },
  age: { type: Number, default: 0 },
  items: { type: Array, default: () => [] },
  obj: { type: Object, default: () => ({}) },
  status: {
    type: String,
    validator(value) {
      return ['active', 'inactive'].includes(value)
    }
  }
}
```

## Props 드릴링 (Props Drilling)

부모 → 자식 → 손자로 props를 계속 전달해야 하는 현상입니다.
- Provide/Inject로 해결 가능
- Pinia (Vuex)로 전역 상태 관리

## 단방향 데이터 흐름

props는 부모 → 자식 단방향입니다. 자식에서 props를 직접 수정하면 안 됩니다.
(변경이 필요하면 부모에 emit으로 알림)
