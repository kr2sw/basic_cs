# 08: Event Handling — 이벤트 처리

## v-on (@)

```html
<button @click="handler">클릭</button>
<button @click="handler($event, 'param')">파라미터 전달</button>
<button @click="count++">인라인 표현식</button>
```

## 이벤트 수식어 (Event Modifiers)

| 수식어 | 설명 |
|--------|------|
| `.stop` | `event.stopPropagation()` |
| `.prevent` | `event.preventDefault()` |
| `.capture` | 캡처 모드 |
| `.self` | 이벤트 타겟이 자신일 때만 |
| `.once` | 한 번만 실행 |
| `.passive` | 성능 최적화 (터치/스크롤) |

## 키 수식어 (Key Modifiers)

```html
<input @keyup.enter="submit">
<input @keyup.ctrl.enter="save">
<input @keyup.esc="cancel">
```

## 마우스 수식어

```html
<button @click.left="leftClick">좌클릭</button>
<button @click.middle="middleClick">휠클릭</button>
<button @click.right.prevent="rightClick">우클릭</button>
```
