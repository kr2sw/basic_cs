import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'dashboard',
    component: () => import('./views/Dashboard.vue'),
    meta: { title: '대시보드' }
  },
  {
    path: '/todos',
    name: 'todos',
    component: () => import('./views/TodoList.vue'),
    meta: { title: '할 일 관리' }
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'not-found',
    component: () => import('./views/Dashboard.vue'),
    meta: { title: '404' }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// afterEach: 문서 타이틀 설정
router.afterEach((to) => {
  document.title = to.meta.title ? `${to.meta.title} - 대시보드` : '대시보드'
})

export default router
