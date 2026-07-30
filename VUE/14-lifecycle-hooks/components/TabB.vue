<template>
  <div class="tab">
    <p>Tab B (캐시됨)</p>
    <input v-model="text" placeholder="입력값 유지됨">
  </div>
</template>

<script>
export default {
  data() { return { text: '' } },
  activated() { console.log('[TabB] activated') },
  deactivated() { console.log('[TabB] deactivated') }
}
</script>

<style scoped>
.tab { background: #fce4ec; padding: 16px; border-radius: 4px; margin: 8px 0; }
</style>
