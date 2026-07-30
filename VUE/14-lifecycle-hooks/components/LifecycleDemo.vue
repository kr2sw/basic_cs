<template>
  <div class="demo">
    <p>LifecycleDemo: {{ count }}</p>
    <button @click="count++">증가</button>
  </div>
</template>

<script>
export default {
  data() { return { count: 0 } },
  beforeCreate() { console.log('  [demo] beforeCreate') },
  created() { console.log('  [demo] created') },
  beforeMount() { console.log('  [demo] beforeMount') },
  mounted() { console.log('  [demo] mounted') },
  beforeUpdate() { console.log('  [demo] beforeUpdate') },
  updated() { console.log('  [demo] updated') },
  beforeUnmount() { console.log('  [demo] beforeUnmount') },
  unmounted() { console.log('  [demo] unmounted') }
}
</script>

<style scoped>
.demo { border: 1px solid #ffc107; padding: 12px; border-radius: 4px; margin: 8px 0; }
</style>
