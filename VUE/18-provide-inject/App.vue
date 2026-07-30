<template>
  <div class="app">
    <h1>Provide & Inject</h1>

    <h2>테마 설정</h2>
    <div>
      <button @click="theme = 'light'">☀ Light</button>
      <button @click="theme = 'dark'">🌙 Dark</button>
    </div>

    <h2>깊은 트리 (Props Drilling vs Provide/Inject)</h2>
    <Level1 />
  </div>
</template>

<script>
import { ref, provide } from 'vue'
import Level1 from './components/Level1.vue'

export default {
  components: { Level1 },
  setup() {
    const theme = ref('light')

    provide('theme', theme)
    provide('toggleTheme', () => {
      theme.value = theme.value === 'light' ? 'dark' : 'light'
    })

    return { theme }
  }
}

// App level theme class
</script>

<style>
:root { --bg: white; --text: #333; }
.app.dark, .dark .app { --bg: #1a1a2e; --text: #eee; }
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; background: var(--bg); color: var(--text); transition: all 0.3s; }
h1 { color: #42b883; }
h2 { color: var(--text); margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 6px 16px; cursor: pointer; }
</style>
