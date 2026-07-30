# 07: List Rendering — 리스트 렌더링

## v-for

```html
<li v-for="item in items" :key="item.id">{{ item }}</li>
<li v-for="(item, index) in items" :key="index">{{ index }}: {{ item }}</li>
<li v-for="(value, key, index) in obj" :key="key">{{ key }}: {{ value }}</li>
<li v-for="n in 10" :key="n">{{ n }}</li>
```

## key 속성

`key`는 Vue가 각 노드를 식별하는 데 사용합니다. **고유한 값**을 지정해야 합니다.

## 배열 변경 감지

Vue는 다음 배열 변경 메서드를 감지합니다:
- `push()`, `pop()`, `shift()`, `unshift()`, `splice()`, `sort()`, `reverse()`
- 직접 인덱스 할당은 감지 못 함 → `Vue.set()` 또는 spread 사용
