<template>
  <div class="heavy">
    <h3>Heavy 컴포넌트</h3>
    <p>이 컴포넌트는 1초 후 로드됩니다. (지연 로딩 시뮬레이션)</p>
    <p>렌더링 시간: {{ renderMs }}ms</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const renderMs = ref(0)

onMounted(() => {
  // 로드 시간 시뮬레이션
  const start = performance.now()
  const end = performance.now()
  renderMs.value = Math.round(end - start)
})
</script>

<style scoped>
.heavy { margin-top: 12px; padding: 12px; background: #f5f5f5; border-radius: 6px; }
h3 { margin: 0 0 6px; color: #42b883; }
</style>
