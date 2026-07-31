import { createRouter, createWebHistory } from 'vue-router'
import Home from './views/Home.vue'

const routes = [
  { path: '/', name: 'home', component: Home, meta: { title: '홈' } },
  {
    // lazy loading: /about 방문 시점에 코드 분할 로드
    path: '/about',
    name: 'about',
    component: () => import('./views/About.vue'),
    meta: { title: '소개' }
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('./views/Login.vue'),
    meta: { title: '로그인', guestOnly: true }
  },
  {
    // requiresAuth: 인증이 필요한 라우트
    path: '/profile',
    name: 'profile',
    component: () => import('./views/Profile.vue'),
    meta: { title: '프로필', requiresAuth: true }
  },
  {
    // 404 catch-all
    path: '/:pathMatch(.*)*',
    name: 'not-found',
    component: () => import('./views/NotFound.vue'),
    meta: { title: '404' }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// 전역 가드: 인증 확인
router.beforeEach((to) => {
  const isLoggedIn = localStorage.getItem('login') === 'true'

  // 인증 필요 페이지: 로그인 페이지로 리다이렉트 (되돌아올 경로 저장)
  if (to.meta.requiresAuth && !isLoggedIn) {
    return { name: 'login', query: { redirect: to.fullPath } }
  }

  // 게스트 전용 페이지: 이미 로그인 상태면 홈으로
  if (to.meta.guestOnly && isLoggedIn) {
    return { name: 'home' }
  }
})

// afterEach: 페이지 타이틀 설정
router.afterEach((to) => {
  document.title = to.meta.title ? `${to.meta.title} - Vue` : 'Vue'
})

export default router
