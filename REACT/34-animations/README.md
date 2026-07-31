# 34: 애니메이션 — CSS Transitions & Framer Motion

CSS 전환으로 가벼운 애니메이션을, Framer Motion으로 선언적이고 강력한 애니메이션을 구현합니다.

## CSS transitions 기초

요소의 상태 변화(스타일 변경)에 **보간**을 적용합니다. `transition` 속성으로 대상과 지속 시간, 타이밍 함수를 지정합니다.

```css
.button { transition: transform 0.2s ease, background 0.2s; }
.button:hover { transform: scale(1.05); }
```

```jsx
// className만 바꿔도 브라우저가 전환을 그린다
<div className={open ? 'panel open' : 'panel'} />
```

## Framer Motion — 선언적 애니메이션

`motion.div`는 일반 JSX처럼 쓰면서 `animate`/`initial`로 상태를 선언합니다. 진입/퇴장은 `AnimatePresence`가 담당합니다.

```jsx
import { motion, AnimatePresence } from 'framer-motion'

<motion.div
  initial={{ opacity: 0, y: 20 }}
  animate={{ opacity: 1, y: 0 }}
  exit={{ opacity: 0, x: -100 }}
  transition={{ duration: 0.3 }}
/>
```

## 제스처와 변형

`whileHover`, `whileTap`은 마우스/터치 반응을, `variants`는 재사용 가능한 상태 묶음을 정의합니다.

```jsx
const variants = {
  hidden: { opacity: 0 },
  show: { opacity: 1, transition: { staggerChildren: 0.1 } },
}
```

## layout 애니메이션

`layout` prop을 주면 요소의 크기/위치가 바뀔 때 Framer Motion이 자동으로 부드러운 전환을 그립니다. 리스트의 정렬·삭제 시 특히 유용합니다.

```jsx
<motion.li layout exit={{ opacity: 0, x: -80 }}>...</motion.li>
```

## prefers-reduced-motion 존중

전정장애(전정계) 사용자는 움직임이 큰 애니메이션을 꺼두도록 요청할 수 있습니다. `useReducedMotion` 훅으로 감지해 끄거나 축소합니다.

```jsx
import { useReducedMotion } from 'framer-motion'

const reduce = useReducedMotion()
<motion.div animate={reduce ? { opacity: 1 } : { opacity: 1, y: 0 }} />
```

## 실행

```bash
npm install framer-motion && npm run dev
```
