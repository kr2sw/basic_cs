# 18: Provide & Inject — 의존성 주입

## Props Drilling 문제

깊은 중첩 구조에서 props를 여러 단계 전달해야 하는 문제입니다.

## Provide / Inject

```js
// 부모 (provider)
import { provide } from 'vue'
provide('key', value)

// 자식 (injector)
import { inject } from 'vue'
const value = inject('key', defaultValue)
```

## 반응형 데이터 제공

`ref`나 `reactive`를 provide하면 자식에서 변경도 가능합니다.
(단, 단방향을 권장하며 변경 함수도 함께 제공하는 것이 좋습니다.)

## App Level Provide

```js
const app = createApp(App)
app.provide('globalKey', value)
```
