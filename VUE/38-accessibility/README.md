# 38: 접근성 — ARIA, 포커스 관리, 키보드

## ARIA (Accessible Rich Internet Applications)

보조 기술(스크린 리더)이 UI를 이해하도록 돕는 속성입니다.

```html
<button role="switch" :aria-checked="on">알림</button>
<div role="dialog" aria-modal="true" aria-labelledby="title">...</div>
```

| 속성 | 용도 |
|------|------|
| `role` | 요소의 역할 명시 (dialog, switch, tablist...) |
| `aria-label` | 시각적 텍스트가 없는 요소에 이름 부여 |
| `aria-labelledby` | 다른 요소 id를 이름으로 참조 |
| `aria-checked` / `aria-expanded` | 상태 표현 |
| `aria-live` | 동적 콘텐츠 변경 알림 |

## 포커스 관리

모달이 열리면 포커스를 모달 안으로 이동하고, 닫히면 이전 요소로 돌려줍니다.
내부에서 Tab이 빠져나가지 않도록 **포커스 트랩(focus trap)** 을 구현합니다.

```js
function onKeydown(e) {
  if (e.key !== 'Tab') return
  // 모달 안의 포커스 가능한 요소들 사이를 순환
}
```

## 키보드 네비게이션

```js
function onListKeydown(e) {
  if (e.key === 'ArrowDown') next()
  if (e.key === 'ArrowUp') prev()
  if (e.key === 'Enter' || e.key === ' ') select()
}
```

## 기본 지침

- 모든 인터랙티브 요소는 키보드로 조작 가능해야 함
- 포커스가 보이는 상태 유지 (`:focus-visible` 스타일)
- 색상만으로 구분하지 말 것 (텍스트/아이콘 병행)
- 스킵 링크로 본문으로 바로 이동 제공

## focus-visible 스타일

키보드 사용자에게만 포커스 표시를 보여줍니다.

```css
:focus-visible {
  outline: 2px solid #42b883;
  outline-offset: 2px;
}
```

## aria-live (동적 변경 알림)

화면에 갑자기 나타나는 토스트/로딩 메시지는 보조 기술이 감지하지 못합니다.

```html
<!-- polite: 사용자 조작 완료 후 안내 (기본 권장) -->
<div aria-live="polite">저장 완료</div>

<!-- assertive: 즉시 중단하고 안내 (주의해서 사용) -->
<div aria-live="assertive">치명적 오류</div>
```

## 대표적인 역할/속성 요약

| 패턴 | 마크업 |
|------|--------|
| 스위치 | `role="switch"` + `aria-checked` |
| 다이얼로그 | `role="dialog"` + `aria-modal` + `aria-labelledby` |
| 목록 상자 | `role="listbox"` + `role="option"` + `aria-activedescendant` |
| 접기/펼치기 | `aria-expanded` |
| 툴팁 | `role="tooltip"` + `aria-describedby` |

## 실행

```bash
npm install && npx vite serve .
```
