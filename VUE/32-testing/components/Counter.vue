<template>
  <div class="counter">
    <p>카운트: {{ count }}</p>
    <button @click="increment">+1</button>
    <button @click="decrement">-1</button>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

// 테스트 대상 컴포넌트
const props = defineProps({
  initial: { type: Number, default: 0 }
})

const count = ref(props.initial)
const doubled = computed(() => count.value * 2)

function increment() { count.value++ }
function decrement() { count.value-- }
</script>
