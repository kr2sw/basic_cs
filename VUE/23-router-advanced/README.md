# 23: 라우터 심화 — 가드, lazy loading, 메타 필드

## 설치

```bash
npm install vue-router@4
```

## Lazy Loading (지연 로딩)

동적 import로 컴포넌트를 코드 스플리팅하면 처음 진입 시 필요한 페이지만 로드합니다.

```js
const routes = [
  { path: '/', component: Home },                      // 즉시 로드
  { path: '/about', component: () => import('./views/About.vue') } // 방문 시 로드
]
```

## 메타 필드 (meta)

라우트에 추가 정보를 붙입니다. 가드에서 권한/제목 등을 판단할 때 사용합니다.

```js
{ path: '/profile', component: Profile, meta: { requiresAuth: true, title: '프로필' } }
```

## 네비게이션 가드

| 가드 | 실행 시점 |
|------|----------|
| `router.beforeEach` | 모든 내비게이션 전 (전역) |
| `router.beforeResolve` | 가드 해석 완료 직전 |
| `router.afterEach` | 내비게이션 확정 후 (리다이렉션 불가) |
| `beforeEnter` | 특정 라우트 진입 전 |
| 컴포넌트 내 `onBeforeRouteUpdate` | 라우트 파라미터 변경 시 |

```js
router.beforeEach((to, from) => {
  const isLoggedIn = localStorage.getItem('login') === 'true'
  if (to.meta.requiresAuth && !isLoggedIn) {
    return { name: 'login', query: { redirect: to.fullPath } }
  }
})
```

## 404 처리 (catch-all)

```js
{ path: '/:pathMatch(.*)*', name: 'not-found', component: NotFound }
```

## 컴포넌트 내 가드

```js
import { onBeforeRouteUpdate, onBeforeRouteLeave } from 'vue-router'

// 같은 라우트에서 파라미터만 바뀔 때 (예: /users/1 → /users/2)
onBeforeRouteUpdate((to, from) => {
  console.log('파라미터 변경:', from.params, '→', to.params)
})

// 라우트를 떠날 때 (저장 안 된 편지 경고 등)
onBeforeRouteLeave(() => {
  const ok = confirm('정말 떠나시겠습니까?')
  return ok
})
```

## scrollBehavior (스크롤 복원)

```js
const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    return savedPosition || { top: 0 } // 이동 시 상단으로
  }
})
```

## NavigationFailure (실패한 내비게이션)

```js
import { isNavigationFailure, NavigationFailureType } from 'vue-router'

const result = await router.push('/profile')
if (isNavigationFailure(result, NavigationFailureType.redirected)) {
  console.log('가드에 의해 리다이렉트되었습니다')
}
```

## 실행

```bash
npm install && npx vite serve .
```
