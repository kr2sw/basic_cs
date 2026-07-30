<template>
  <div class="slider">
    <input
      type="range"
      :min="min"
      :max="max"
      :value="modelValue"
      @input="$emit('update:modelValue', Number($event.target.value))"
    >
    <span>{{ modelValue }}px</span>
  </div>
</template>

<script>
export default {
  props: {
    modelValue: { type: Number, required: true },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 }
  },
  emits: ['update:modelValue']
}
</script>

<style scoped>
.slider { display: flex; align-items: center; gap: 12px; }
input[type="range"] { flex: 1; }
</style>
