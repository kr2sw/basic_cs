<template>
  <div class="tab">
    <p>Tab A (캐시됨 - activated/deactivated 훅 실행)</p>
    <p>count: {{ count }}</p>
    <button @click="count++">+1</button>
  </div>
</template>

<script>
export default {
  data() { return { count: 0 } },
  activated() { console.log('[TabA] activated') },
  deactivated() { console.log('[TabA] deactivated') }
}
</script>

<style scoped>
.tab { background: #e3f2fd; padding: 16px; border-radius: 4px; margin: 8px 0; }
</style>
