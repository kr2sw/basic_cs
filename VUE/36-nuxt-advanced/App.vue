<template>
  <div class="app">
    <h1>Nuxt 심화 (데모)</h1>

    <p class="hint">
      이 App.vue는 Nuxt의 useFetch/미들웨어/레이아웃 개념을 Vite 환경에서
      재현한 데모입니다. 실제 코드는 README의 Nuxt 예제를 참고하세요.
    </p>

    <h2>useAsyncData 유사 composable</h2>
    <button @click="loadUsers">사용자 불러오기</button>
    <p v-if="pending" class="loading">로딩 중...</p>
    <p v-if="error" class="error">{{ error }}</p>
    <ul v-if="users.length">
      <li v-for="user in users" :key="user.id">
        {{ user.name }} ({{ user.email }})
      </li>
    </ul>

    <h2>미들웨어(가드) 시뮬레이션</h2>
    <p>접근 허용 상태: {{ canAccess ? '허용' : '차단' }}</p>
    <button @click="canAccess = !canAccess">토글</button>
    <button @click="tryAccess">관리자 페이지 시도</button>
    <p v-if="guardMessage" class="guard">{{ guardMessage }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'

// Nuxt useAsyncData / useFetch의 동작을 단순 재현한 composable
function useApi(url) {
  const data = ref([])
  const pending = ref(false)
  const error = ref('')

  async function execute() {
    pending.value = true
    error.value = ''
    try {
      const res = await fetch(url)
      data.value = await res.json()
    } catch (e) {
      error.value = '요청 실패: ' + e.message
    } finally {
      pending.value = false
    }
  }

  return { data, pending, error, execute }
}

const { data: users, pending, error, execute: loadUsers } = useApi(
  'https://jsonplaceholder.typicode.com/users?_limit=5'
)

// Nuxt middleware(auth) 시뮬레이션: 라우트 진입 전 권한 확인
const canAccess = ref(false)
const guardMessage = ref('')

function tryAccess() {
  if (!canAccess.value) {
    // Nuxt: navigateTo('/login') 에 해당
    guardMessage.value = '차단! 로그인 페이지로 리다이렉트됩니다. (navigateTo)'
  } else {
    guardMessage.value = '허용! 관리자 페이지에 진입했습니다.'
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.hint { color: #999; font-size: 13px; background: #f5f5f5; padding: 10px; border-radius: 6px; }
.loading { color: #42b883; }
.error { color: #dc3545; }
.guard { background: #fff3cd; padding: 8px; border-radius: 4px; font-size: 13px; }
button { margin: 4px; padding: 6px 14px; cursor: pointer; }
li { padding: 2px 0; }
</style>
