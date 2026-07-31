import { reactive } from 'vue'

// 토스트 알림 플러그인
// install(app, options) 구조를 가진 객체
export default {
  install(app, options = {}) {
    const duration = options.duration ?? 3000
    const state = reactive({ messages: [] })

    function show(message, type = 'info') {
      state.messages.push({ id: Date.now() + Math.random(), message, type })
      setTimeout(() => {
        state.messages.shift()
      }, duration)
    }

    const toast = {
      state,
      info: (m) => show(m, 'info'),
      success: (m) => show(m, 'success'),
      error: (m) => show(m, 'error')
    }

    // 1) globalProperties: Options API에서 this.$toast 로 접근
    app.config.globalProperties.$toast = toast

    // 2) provide: <script setup>에서 inject('toast') 로 접근
    app.provide('toast', toast)
  }
}
