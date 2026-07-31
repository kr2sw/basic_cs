<template>
  <div class="app">
    <h1>테스팅 (Vitest)</h1>

    <p class="hint">
      이 App.vue는 데모용입니다. 실제 테스트는 <code>tests/Counter.spec.js</code>를
      <code>npx vitest</code>로 실행하세요. (README 참고)
    </p>

    <Counter :initial="3" />
  </div>
</template>

<script setup>
import Counter from './components/Counter.vue'
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
.hint { color: #999; font-size: 13px; background: #f5f5f5; padding: 10px; border-radius: 6px; }
</style>
