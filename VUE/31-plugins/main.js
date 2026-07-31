import { createApp } from 'vue'
import App from './App.vue'
import toastPlugin from './plugins/toast.js'

const app = createApp(App)

// 플러그인 등록 (두 번째 인자로 옵션 전달 가능)
app.use(toastPlugin, { duration: 2500 })
app.mount('#app')
