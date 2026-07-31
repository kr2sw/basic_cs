<template>
  <div class="app">
    <h1>SSR과 Nuxt 기초</h1>

    <p class="hint">
      이 App.vue는 Vite SFC에서 "SSR에서 안전한 코드 패턴"을 보여주는 데모입니다.
      실제 SSR 프로젝트는 Nuxt로 시작하세요. (README 참고)
    </p>

    <h2>서버에서 렌더링 가능한 부분</h2>
    <p>사용자: {{ username }} (서버/클라이언트 모두 표시 가능)</p>

    <h2>클라이언트 전용 상태 (hydration 예시)</h2>
    <p v-if="viewportLoaded">창 크기: {{ windowWidth }} x {{ windowHeight }}</p>
    <p v-else>창 크기를 측정 중... (SSR에는 이 값이 없습니다)</p>

    <h2>Nuxt 프로젝트 구조 (개념)</h2>
    <pre class="tree">pages/
  index.vue        → /
  about.vue        → /about
app.vue            → 루트 컴포넌트
nuxt.config.ts</pre>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

// SSR에서 안전한 코드: setup 단계에서는 브라우저 전역에 접근 금지
const username = ref('Vue 사용자')

// window/document는 onMounted 이후에만 접근
const windowWidth = ref(0)
const windowHeight = ref(0)
const viewportLoaded = ref(false)

onMounted(() => {
  windowWidth.value = window.innerWidth
  windowHeight.value = window.innerHeight
  viewportLoaded.value = true
})
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.hint { color: #999; font-size: 13px; background: #f5f5f5; padding: 10px; border-radius: 6px; }
.tree { background: #f5f5f5; padding: 12px; border-radius: 6px; font-size: 12px; }
</style>
