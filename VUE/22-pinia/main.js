import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'

// Pinia 플러그인 등록
const app = createApp(App)
app.use(createPinia())
app.mount('#app')
