<template>
  <div class="level" :style="levelStyle">
    <p>Level 1</p>
    <Level2 />
  </div>
</template>

<script>
import { inject } from 'vue'
import Level2 from './Level2.vue'

export default {
  components: { Level2 },
  setup() {
    const theme = inject('theme')

    const levelStyle = {
      border: `2px solid ${theme.value === 'dark' ? '#555' : '#ddd'}`,
      background: theme.value === 'dark' ? '#16213e' : '#f9f9f9',
      padding: '12px',
      borderRadius: '4px',
      margin: '8px 0'
    }

    return { levelStyle }
  }
}
</script>
