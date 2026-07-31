# 29: 렌더 함수 — h(), VNode, JSX in Vue

## h() 란?

템플릿 대신 JavaScript로 VNode(가상 DOM 노드)를 만드는 함수입니다.

```js
import { h } from 'vue'

// h(태그, props, children)
h('div', { class: 'box' }, 'Hello')
h('ul', [h('li', 'A'), h('li', 'B')])
```

## 렌더 함수 컴포넌트

```js
import { defineComponent, h } from 'vue'

const Button = defineComponent({
  props: { primary: Boolean },
  setup(props, { slots, emit }) {
    return () =>
      h(
        'button',
        {
          class: props.primary ? 'btn primary' : 'btn',
          onClick: () => emit('click')
        },
        [slots.default ? slots.default() : '버튼']
      )
  }
})
```

## JSX in Vue

템플릿 대신 JSX 문법도 사용할 수 있습니다. `@vitejs/plugin-vue-jsx` 설치가 필요합니다.

```bash
npm install @vitejs/plugin-vue-jsx
```

```jsx
// renderJsx.jsx
export default {
  setup() {
    return () => <div class="jsx-box">JSX 사용 가능</div>
  }
}
```

## 언제 렌더 함수가 필요한가?

- 동적으로 태그/구조를 생성해야 하는 고급 UI 라이브러리
- 템플릿으로 표현하기 어려운 재귀/추상 구조
- 하지만 일반 애플리케이션에서는 템플릿을 우선 권장합니다.

## 실행

```bash
npm install && npx vite serve .
```
