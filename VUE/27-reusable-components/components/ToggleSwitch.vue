<template>
  <button
    class="switch"
    :class="{ on: model }"
    role="switch"
    :aria-checked="model"
    @click="model = !model"
  >
    {{ model ? 'ON' : 'OFF' }}
  </button>
</template>

<script setup>
// v-model:checked 와 같은 이름 있는 v-model 패턴 (defineModel('checked'))
const model = defineModel({ default: false })
</script>

<style scoped>
.switch { padding: 6px 16px; border: 2px solid #ccc; border-radius: 20px; background: white; cursor: pointer; font-weight: bold; }
.switch.on { background: #42b883; border-color: #42b883; color: white; }
</style>
