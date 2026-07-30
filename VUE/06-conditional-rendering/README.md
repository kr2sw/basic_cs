# 06: Conditional Rendering — 조건부 렌더링

## v-if / v-else-if / v-else

조건에 따라 요소를 DOM에 추가/제거합니다.

```html
<div v-if="type === 'A'">A</div>
<div v-else-if="type === 'B'">B</div>
<div v-else>기타</div>
```

## v-show

`display: none`으로 토글합니다. (DOM에 항상 존재)

## v-if vs v-show

| v-if | v-show |
|------|--------|
| 조건부 렌더링 (DOM 추가/제거) | CSS display 토글 |
| 초기 false면 렌더링 안 함 | 초기 false도 DOM에 있음 |
| 토글 비용 높음 | 초기 렌더링 비용 높음 |
| 빈번하지 않은 변경에 적합 | 빈번한 토글에 적합 |

## template 태그

조건부 그룹을 렌더링할 때 사용합니다. (실제 DOM에 추가되지 않음)
