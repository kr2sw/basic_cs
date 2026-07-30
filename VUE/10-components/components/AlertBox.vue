<template>
  <div class="alert">
    <span class="msg"><slot></slot></span>
    <button class="close" @click="$emit('close')">x</button>
  </div>
</template>

<script>
export default {
  emits: ['close']
}
</script>

<style scoped>
.alert { background: #fff3cd; border: 1px solid #ffc107; padding: 12px 16px; border-radius: 4px; display: flex; justify-content: space-between; align-items: center; margin: 8px 0; }
.close { background: none; border: none; cursor: pointer; font-size: 18px; color: #856404; }
</style>
