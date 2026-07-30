# 19: Vue Router — 라우터

## Vue Router 설치

```bash
npm install vue-router@4
```

## 기본 설정

```js
import { createRouter, createWebHistory } from 'vue-router'
import Home from './views/Home.vue'
import About from './views/About.vue'

const routes = [
  { path: '/', name: 'home', component: Home },
  { path: '/about', name: 'about', component: About },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
```

## 주요 컴포넌트

| 컴포넌트 | 설명 |
|---------|------|
| `<router-link>` | 네비게이션 링크 |
| `<router-view>` | 매칭된 컴포넌트 렌더링 |

## 네비게이션 가드

```js
router.beforeEach((to, from) => { /* ... */ })
```
