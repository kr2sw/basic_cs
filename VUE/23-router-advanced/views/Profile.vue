<template>
  <div>
    <h2>프로필</h2>
    <p>인증(requiresAuth)을 통과한 사용자만 접근할 수 있습니다.</p>
    <button @click="logout">로그아웃</button>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

function logout() {
  localStorage.setItem('login', 'false')
  window.dispatchEvent(new Event('login-change'))
  router.push({ name: 'home' })
}
</script>
