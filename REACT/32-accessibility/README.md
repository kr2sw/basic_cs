# 32: 접근성 — Accessibility

ARIA, 키보드 내비게이션, 시맨틱 HTML로 모든 사용자가 쓸 수 있는 UI를 만듭니다.

## 시맨틱 HTML 먼저

ARIA를 붙이기 전에 네이티브 요소를 우선 사용하세요. `<button>`은 키보드·포커스·스크린리더를 기본 제공합니다.

```jsx
// ❌ div에 클릭만 붙이면 키보드 사용자/스크린리더가 무시된다
<div onClick={open}>열기</div>

// ✅ button이면 Enter/Space, 포커스, 역할이 모두 기본 제공
<button onClick={open}>열기</button>
```

## ARIA 핵심 속성

- `aria-label`: 눈에 보이지 않는 라벨 제공 (아이콘 버튼)
- `aria-expanded`: 접힘/펼침 상태
- `role="dialog"` + `aria-modal="true"`: 모달 선언
- `aria-live="polite"`: 상태 변화를 스크린리더가 알림

```jsx
<button aria-label="알림 3개" aria-expanded={open} onClick={toggle}>
  🔔
</button>
<div role="dialog" aria-modal="true" aria-labelledby="title">...</div>
```

## 키보드 내비게이션

포커스를 관리하려면 `tabIndex`와 포커스 이동 로직이 필요합니다.

```jsx
const ref = useRef(null)
useEffect(() => { ref.current?.focus() }, [])  // 열릴 때 첫 요소로 포커스

// Esc로 닫기
onKeyDown={e => { if (e.key === 'Escape') close() }}
```

## 기타 체크리스트

- 색상 대비: WCAG AA 기준 본문 4.5:1
- `:focus` 스타일 유지 (outline 제거 금지)
- `html lang="ko"`, `title` 포함
- 이미지 `alt`, 폼 `label htmlFor`
- ESLint: `eslint-plugin-jsx-a11y`, 검사 도구: axe, Lighthouse

## 실행

```bash
npm install && npm run dev
```
