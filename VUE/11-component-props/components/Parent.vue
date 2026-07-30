<template>
  <div class="drill">
    <p>Level {{ level }} - Parent</p>
    <Child :level="level + 1" />
  </div>
</template>

<script>
import Child from './Child.vue'
export default {
  components: { Child },
  props: { level: { type: Number, required: true } }
}
</script>

<style scoped>
.drill { border-left: 3px solid #42b883; padding-left: 16px; margin: 8px 0; }
</style>
