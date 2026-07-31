<template>
  <div class="app">
    <h1>라우터 심화</h1>

    <nav class="nav">
      <router-link to="/">홈</router-link>
      <router-link to="/about">소개</router-link>
      <router-link to="/profile">프로필 (인증 필요)</router-link>
      <router-link to="/no-such-page">존재하지 않는 페이지</router-link>
    </nav>

    <div class="content">
      <router-view />
    </div>

    <div class="info">
      <p>현재 경로: {{ $route.path }}</p>
      <p>메타: {{ $route.meta }}</p>
      <p v-if="!isLoggedIn">
        로그인하지 않으면 <router-link to="/profile">/profile</router-link> 접근이 거부됩니다.
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

// 로그인 상태 표시 (실제 인증은 localStorage 기준)
const isLoggedIn = ref(localStorage.getItem('login') === 'true')

function setLoggedIn(value) {
  isLoggedIn.value = value
  localStorage.setItem('login', String(value))
  window.dispatchEvent(new Event('login-change'))
}
</script>

<style>
body { margin: 0; font-family: Arial; }
.app { max-width: 700px; margin: 0 auto; padding: 20px; }
h1 { color: #42b883; }
.nav { display: flex; gap: 12px; padding: 12px 0; border-bottom: 2px solid #42b883; margin-bottom: 20px; flex-wrap: wrap; }
.nav a { text-decoration: none; color: #333; padding: 4px 12px; border-radius: 4px; }
.nav a:hover { background: #e8f5e9; }
.nav a.router-link-exact-active { background: #42b883; color: white; }
.content { min-height: 200px; padding: 20px 0; }
.info { margin-top: 20px; padding: 12px; background: #f5f5f5; border-radius: 4px; font-size: 13px; color: #666; }
</style>
