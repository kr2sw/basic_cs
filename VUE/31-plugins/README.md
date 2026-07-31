# 31: 플러그인 개발 — app.use, provide/inject

## 플러그인 구조

플러그인은 `install(app, options)` 함수를 가진 객체(또는 함수)입니다.

```js
// plugins/toast.js
import { reactive } from 'vue'

export default {
  install(app, options) {
    // 플러그인 로직
  }
}
```

## app.use() 등록

```js
import { createApp } from 'vue'
import toastPlugin from './plugins/toast.js'

const app = createApp(App)
app.use(toastPlugin, { duration: 3000 }) // 두 번째 인자로 옵션 전달
app.mount('#app')
```

## 플러그인 내부에서 할 수 있는 일

| 방법 | 목적 |
|------|------|
| `app.config.globalProperties.$toast = ...` | 전역 속성 등록 |
| `app.provide('toast', toast)` | `inject()`로 접근 가능한 전역 서비스 |
| `app.directive('focus', ...)` | 전역 디렉티브 등록 |
| `app.component('BaseButton', ...)` | 전역 컴포넌트 등록 |

## 전체 플러그인 예제

```js
// plugins/toast.js
import { reactive } from 'vue'

export default {
  install(app, options = {}) {
    const state = reactive({ messages: [] })
    const toast = {
      state,
      success: (m) => { /* ... */ },
      error: (m) => { /* ... */ }
    }

    app.config.globalProperties.$toast = toast
    app.provide('toast', toast)
    app.directive('focus', { mounted: (el) => el.focus() })
    app.component('BaseButton', { template: '<button><slot/></button>' })
  }
}
```

## 옵션 처리

`app.use(plugin, options)`의 두 번째 인자로 설정을 전달합니다.

```js
app.use(toastPlugin, { duration: 2500, position: 'top-right' })
```

플러그인에서는 `options`에서 기본값과 병합해 사용합니다.

```js
install(app, options = {}) {
  const { duration = 3000 } = options
}
```

## provide/inject와의 관계

플러그인이 `provide`한 값은 어떤 컴포넌트에서도 `inject`로 받을 수 있습니다.
`<script setup>`에서는 `inject('toast')` 호출 한 번으로 사용합니다.

```js
import { inject } from 'vue'
const toast = inject('toast') // 플러그인이 주입한 서비스
toast.success('저장되었습니다')
```

## 실행

```bash
npm install && npx vite serve .
```
