<template>
  <input
    :value="model"
    :placeholder="placeholder"
    @input="model = $event.target.value"
    v-bind="$attrs"
  >
</template>

<script setup>
// defineModel: v-model과 연결되는 양방향 바인딩 prop
const model = defineModel()
defineProps({
  placeholder: { type: String, default: '' }
})
</script>
