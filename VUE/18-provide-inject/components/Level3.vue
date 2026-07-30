<template>
  <div class="level" :style="levelStyle">
    <p>Level 3 - 가장 깊은 레벨</p>
    <p>현재 테마: {{ theme }}</p>
    <button @click="toggle">테마 토글 (inject로 변경)</button>
  </div>
</template>

<script>
import { inject } from 'vue'

export default {
  setup() {
    const theme = inject('theme')
    const toggle = inject('toggleTheme')

    return {
      theme,
      toggle,
      levelStyle: {
        border: `1px solid ${theme.value === 'dark' ? '#555' : '#ddd'}`,
        background: theme.value === 'dark' ? '#0f3460' : '#f0f0f0',
        padding: '12px',
        borderRadius: '4px',
        margin: '8px 0'
      }
    }
  }
}
</script>
