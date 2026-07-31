<template>
  <div>
    <h2>로그인</h2>
    <p>가드에서 /profile 접근 시 이 페이지로 리다이렉트됩니다.</p>
    <button @click="login">로그인 (localStorage 저장)</button>
  </div>
</template>

<script setup>
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

function login() {
  // 실제 인증 로직 대신 localStorage에 기록
  localStorage.setItem('login', 'true')
  window.dispatchEvent(new Event('login-change'))

  // 리다이렉트 예약 경로가 있으면 그곳으로 이동
  router.push(route.query.redirect || '/')
}
</script>
