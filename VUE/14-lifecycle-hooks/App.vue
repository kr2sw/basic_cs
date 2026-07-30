<template>
  <div class="app">
    <h1>Lifecycle Hooks</h1>
    <p>콘솔에서 생명주기 로그를 확인하세요.</p>

    <h2>타이머</h2>
    <p>경과 시간: {{ elapsed }}초</p>

    <h2>컴포넌트 마운트/언마운트</h2>
    <button @click="showComponent = !showComponent">
      {{ showComponent ? '언마운트' : '마운트' }}
    </button>
    <LifecycleDemo v-if="showComponent" />

    <h2>에러 캡처</h2>
    <button @click="throwError">에러 발생</button>
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>

    <h2>KeepAlive</h2>
    <button @click="tab = tab === 'A' ? 'B' : 'A'">탭 전환: {{ tab }}</button>
    <KeepAlive>
      <TabA v-if="tab === 'A'" />
      <TabB v-if="tab === 'B'" />
    </KeepAlive>
  </div>
</template>

<script>
import LifecycleDemo from './components/LifecycleDemo.vue'
import TabA from './components/TabA.vue'
import TabB from './components/TabB.vue'

export default {
  components: { LifecycleDemo, TabA, TabB },
  data() {
    return {
      showComponent: true,
      elapsed: 0,
      timerId: null,
      errorMessage: '',
      tab: 'A'
    }
  },
  mounted() {
    console.log('App mounted')
    this.timerId = setInterval(() => {
      this.elapsed++
    }, 1000)
  },
  updated() {
    console.log('App updated')
  },
  beforeUnmount() {
    console.log('App beforeUnmount')
    clearInterval(this.timerId)
  },
  methods: {
    throwError() {
      this.errorMessage = '에러가 발생했습니다! (하지만 캡처됨)'
      setTimeout(() => { this.errorMessage = '' }, 3000)
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 6px 16px; cursor: pointer; }
.error { color: #dc3545; background: #f8d7da; padding: 8px 16px; border-radius: 4px; margin: 8px 0; }
</style>
