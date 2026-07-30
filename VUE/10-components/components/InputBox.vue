<template>
  <input
    :value="modelValue"
    @input="$emit('update:modelValue', $event.target.value)"
    class="input-box"
  >
</template>

<script>
export default {
  props: {
    modelValue: { type: String, default: '' }
  },
  emits: ['update:modelValue']
}
</script>

<style scoped>
.input-box { padding: 6px 12px; border: 1px solid #ddd; border-radius: 4px; width: 200px; }
</style>
