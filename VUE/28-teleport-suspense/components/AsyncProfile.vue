<template>
  <div class="profile">
    <p>이름: {{ user.name }}</p>
    <p>이메일: {{ user.email }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const user = ref(null)

// async setup: Suspense가 이 컴포넌트의 pending 상태를 감지
await new Promise((resolve) => setTimeout(resolve, 1500))
user.value = {
  name: 'Vue 사용자',
  email: 'vue@example.com'
}
</script>

<style scoped>
.profile { padding: 12px; background: #e8f5e9; border-radius: 6px; }
</style>
