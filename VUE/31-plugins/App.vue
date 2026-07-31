<template>
  <div class="app">
    <h1>플러그인 개발 (app.use)</h1>

    <p class="hint">toast 플러그인이 제공하는 inject('toast') 서비스를 사용합니다.</p>

    <h2>토스트 호출</h2>
    <button @click="toast.info('정보 메시지입니다')">info</button>
    <button @click="toast.success('저장 완료!')">success</button>
    <button @click="toast.error('오류가 발생했습니다')">error</button>

    <!-- 플러그인 내부 reactive 상태가 그대로 렌더링됨 -->
    <div class="toast-wrap">
      <transition-group name="toast">
        <div
          v-for="msg in toast.state.messages"
          :key="msg.id"
          class="toast"
          :class="msg.type"
        >
          {{ msg.message }}
        </div>
      </transition-group>
    </div>
  </div>
</template>

<script setup>
import { inject } from 'vue'

// 플러그인이 provide한 서비스 주입
const toast = inject('toast')
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.hint { color: #999; font-size: 12px; }
button { margin: 4px; padding: 6px 16px; cursor: pointer; }
</style>

<style>
.toast-wrap {
  position: fixed; top: 20px; right: 20px; z-index: 2000;
  display: flex; flex-direction: column; gap: 8px;
}
.toast {
  min-width: 200px; padding: 12px 16px; border-radius: 6px;
  color: white; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
}
.toast.info { background: #17a2b8; }
.toast.success { background: #42b883; }
.toast.error { background: #dc3545; }
.toast-enter-active, .toast-leave-active { transition: all 0.3s; }
.toast-enter-from, .toast-leave-to { opacity: 0; transform: translateX(30px); }
</style>
