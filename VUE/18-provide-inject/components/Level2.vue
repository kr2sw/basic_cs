<template>
  <div class="level" :style="levelStyle">
    <p>Level 2 - props 없이 theme 사용</p>
    <p>현재 테마: {{ theme }}</p>
    <Level3 />
  </div>
</template>

<script>
import { inject } from 'vue'
import Level3 from './Level3.vue'

export default {
  components: { Level3 },
  setup() {
    const theme = inject('theme')

    return {
      theme,
      levelStyle: {
        border: `1px solid ${theme.value === 'dark' ? '#555' : '#ddd'}`,
        background: theme.value === 'dark' ? '#1a1a2e' : 'white',
        padding: '12px',
        borderRadius: '4px',
        margin: '8px 0'
      }
    }
  }
}
</script>
