# 03: Data & Event Binding — 데이터와 이벤트 바인딩

## data 옵션

컴포넌트의 반응형 상태를 정의합니다.

```js
data() {
  return {
    count: 0,
    name: 'Alice'
  }
}
```

## methods 옵션

컴포넌트에서 사용할 함수를 정의합니다.

```js
methods: {
  increment() {
    this.count++
  }
}
```

## v-on (@)

이벤트 리스너를 연결합니다.

```html
<button @click="handler">클릭</button>
<button @click="handler($event, 'param')">파라미터 전달</button>
```

## v-bind (:)

HTML 속성에 데이터를 바인딩합니다.

```html
<img :src="imageSrc">
<a :href="url">링크</a>
```
