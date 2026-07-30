<template>
  <div class="counter">
    <p>count: {{ count }}</p>
    <button @click="count++">+1</button>
    <button @click="count = 0">reset</button>
  </div>
</template>

<script>
export default {
  data() {
    return { count: 0 }
  }
}
</script>

<style scoped>
.counter { border: 1px solid #42b883; padding: 12px; border-radius: 4px; margin: 8px 0; }
button { margin: 4px; padding: 4px 12px; cursor: pointer; }
</style>
