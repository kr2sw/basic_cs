<template>
  <div class="drill">
    <p>Level {{ level }} - Child</p>
    <Child v-if="level < 5" :level="level + 1" />
  </div>
</template>

<script>
export default {
  props: { level: { type: Number, required: true } }
}
</script>

<style scoped>
.drill { border-left: 3px solid #ffc107; padding-left: 16px; margin: 8px 0; }
</style>
