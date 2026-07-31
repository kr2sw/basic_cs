# 38: 디자인 시스템 — Design Systems & Storybook

재사용 가능한 컴포넌트 라이브러리를 설계하고, Storybook으로 문서화/시각 검증하는 방법을 배웁니다.

## 디자인 토큰

색상, 간격, 타이포 등 기본값을 **토큰**으로 분리하면 한 곳만 바꿔도 전체가 일관되게 변합니다.

```js
// tokens.js
export const colors = { primary: '#3b82f6', danger: '#ef4444', text: '#1f2937' }
export const radius = { sm: 6, md: 10 }
export const space = { sm: 8, md: 16 }
```

## 컴포넌트 API 설계

좋은 컴포넌트는 **props로 변형(variant)을 선언**합니다. 분기 문이 많아지면 데이터(변형 목록)로 치환합니다.

```jsx
function Button({ variant = 'primary', disabled, children }) {
  const styles = {
    primary: { background: '#3b82f6', color: '#fff' },
    outline: { background: '#fff', border: '1px solid #3b82f6', color: '#3b82f6' },
  }
  return <button style={styles[variant]} disabled={disabled}>{children}</button>
}
```

## Storybook 개념

스토리(story)는 컴포넌트의 상태별 "렌더링 사례"입니다. Storybook은 이 스토리들을 모아 브라우저에서 카탈로그로 보여줍니다.

```jsx
// Button.stories.jsx
import { Button } from './App'

export default { title: 'Button', component: Button }

export const Primary = () => <Button variant="primary">저장</Button>
export const Outline = () => <Button variant="outline">취소</Button>
```

스토리는 곧 문서이자 회귀 테스트가 됩니다. (참고: `npx storybook@latest init`으로 초기화)

## 실행

```bash
npm install && npm run dev
# Storybook 실행 (별도 프로젝트에서): npx storybook@latest init && npm run storybook
```
