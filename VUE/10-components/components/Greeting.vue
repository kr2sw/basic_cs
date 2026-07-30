<template>
  <div class="greeting">
    <p>Hello, {{ name }}!</p>
  </div>
</template>

<script>
export default {
  props: {
    name: { type: String, required: true }
  }
}
</script>

<style scoped>
.greeting { background: #e8f5e9; padding: 8px 16px; border-radius: 4px; margin: 4px 0; }
</style>
