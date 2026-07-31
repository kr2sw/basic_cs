# 36: Nuxt 심화 — data fetching, middleware, layouts

## Data Fetching

Nuxt는 `useAsyncData`, `useFetch` 같은 composable을 제공합니다.
SSR에서 데이터를 미리 가져와 페이지에 채워 넣습니다.

```js
// pages/users.vue
const { data, pending, error } = await useFetch('/api/users')

// 캐시/재검증 옵션
const { data: posts } = await useAsyncData('posts', () => $fetch('/api/posts'), {
  server: true   // 서버에서 미리 가져옴
})
```

## Middleware (미들웨어)

라우트 진입 전에 실행되는 가드입니다. `definePageMeta`로 페이지에 연결합니다.

```js
// middleware/auth.ts
export default defineNuxtRouteMiddleware((to) => {
  const user = useUser()
  if (!user.value.isLoggedIn) {
    return navigateTo('/login')
  }
})
```

```vue
<!-- pages/secret.vue -->
<script setup>
definePageMeta({ middleware: 'auth' })
</script>
```

## Layouts (레이아웃)

페이지마다 다른 골격을 적용합니다. `layouts/default.vue`, `layouts/auth.vue` 등
파일로 정의하고 `definePageMeta({ layout: 'auth' })`로 선택합니다.

```vue
<!-- layouts/default.vue -->
<template>
  <div>
    <header>공통 헤더</header>
    <slot /> <!-- 페이지 콘텐츠 -->
  </div>
</template>
```

## 기타 고급 기능

| 기능 | 설명 |
|------|------|
| `useHead` / `useSeoMeta` | 문서 head, SEO 메타 관리 |
| `server/api/` | 서버 엔드포인트 생성 (`/api/*`) |
| `app.config.ts` | 환경 설정 |
| `NuxtLink` | 내부 링크 최적화 |

## 실행

```bash
npm install && npx vite serve .
```
