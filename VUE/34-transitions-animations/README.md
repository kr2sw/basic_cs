# 34: 전환과 애니메이션 심화 — Transition, TransitionGroup, state transition

## Transition

단일 요소/컴포넌트의 등장·퇴장을 애니메이션합니다.

```vue
<Transition name="fade">
  <p v-if="show">안녕하세요</p>
</Transition>
```

`name="fade"`가 붙으면 아래 클래스가 자동 적용됩니다.

| 클래스 | 시점 |
|--------|------|
| `fade-enter-from` | 등장 시작 상태 |
| `fade-enter-active` | 등장 중 (transition 지정) |
| `fade-leave-to` | 퇴장 끝 상태 |

## named transition

방향에 따라 서로 다른 이름의 전환을 지정합니다.

```js
const transitionName = ref('slide-left')
```

## TransitionGroup

리스트의 각 항목에 전환을 적용합니다. 이동 애니메이션은 `move` 클래스로 처리합니다.

```vue
<TransitionGroup name="list" tag="ul">
  <li v-for="item in items" :key="item.id">{{ item.text }}</li>
</TransitionGroup>
```

## JS 훅

CSS로 부족한 경우 `@before-enter`, `@enter`, `@leave` 등 훅으로 제어합니다.

```vue
<Transition @enter="onEnter">
  <div v-if="show"></div>
</Transition>
```

## state transition (상태 전환)

숫자 같은 값의 변화도 플러그인 없이 보간해 애니메이션할 수 있습니다.

```js
function animate(from, to, duration = 500) {
  const start = performance.now()
  const step = (now) => {
    const t = Math.min((now - start) / duration, 1)
    display.value = Math.round(from + (to - from) * t)
    if (t < 1) requestAnimationFrame(step)
  }
  requestAnimationFrame(step)
}
```

## 실행

```bash
npm install && npx vite serve .
```
