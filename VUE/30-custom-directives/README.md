# 30: 커스텀 디렉티브 — v-focus, v-click-outside

## 디렉티브란?

`v-html`, `v-model`처럼 요소에 특정 동작을 부여하는 지시어입니다.
공통 DOM 로직은 커스텀 디렉티브로 재사용할 수 있습니다.

## 훅 (lifecycle)

| 훅 | 시점 |
|----|------|
| `mounted(el, binding)` | 요소가 DOM에 삽입될 때 |
| `updated(el, binding)` | 요소가 업데이트될 때 |
| `unmounted(el)` | 요소가 제거될 때 |

`binding`에는 `value`, `arg`, `modifiers`가 담깁니다.

## v-focus 예제

```js
const vFocus = {
  mounted: (el) => el.focus()
}
```

## v-click-outside 예제

```js
const vClickOutside = {
  mounted(el, binding) {
    el._onClickOutside = (e) => {
      if (!el.contains(e.target)) binding.value(e)
    }
    document.addEventListener('click', el._onClickOutside)
  },
  unmounted(el) {
    document.removeEventListener('click', el._onClickOutside)
  }
}
```

## 로컬 vs 전역 등록

`<script setup>`에서 `vFocus`라는 이름의 변수를 정의하면 자동으로
`v-focus` 디렉티브로 사용됩니다. 여러 컴포넌트에서 쓰려면 전역 등록이 낫습니다.

```js
app.directive('focus', {
  mounted: (el) => el.focus()
})
```

> 디렉티브로 등록한 `_onClickOutside` 같은 값은 `unmounted`에서 반드시 해제해야 합니다.

## binding 활용 (arg / value / modifiers)

```vue
<!-- arg: 방향, value: 내용, modifiers: 수식어 -->
<div v-position:top.hide="true"></div>
```

```js
const vPosition = {
  mounted(el, binding) {
    const pos = binding.arg         // "top"
    const hide = binding.modifiers.hide // true
    const enabled = binding.value   // true
    console.log({ pos, hide, enabled })
  }
}
```

## 동적 인자 (Dynamic Argument)

`v-demo:[dynamicArg]`처럼 인자를 데이터로 바꿀 수 있습니다.

```js
const vPosition = {
  mounted(el, binding) {
    if (binding.arg === 'left') el.style.left = '0'
    if (binding.arg === 'right') el.style.right = '0'
  },
  updated(el, binding) {
    // 인자가 바뀌면 위치 갱신
    el.style.left = ''
    el.style.right = ''
    if (binding.arg === 'left') el.style.left = '0'
    if (binding.arg === 'right') el.style.right = '0'
  }
}
```

## 실행

```bash
npm install && npx vite serve .
```
