<template>
  <div class="app">
    <h1>Teleport & Suspense</h1>

    <h2>Teleport 모달</h2>
    <button @click="modalOpen = true">모달 열기</button>

    <!-- Teleport: 이 DOM은 body로 이동되어 렌더링됨 -->
    <Teleport to="body">
      <div v-if="modalOpen" class="modal-backdrop" @click.self="modalOpen = false">
        <div class="modal" role="dialog" aria-modal="true" aria-labelledby="modal-title">
          <h3 id="modal-title">Teleport 모달</h3>
          <p>
            이 모달은 <code>&lt;#app&gt;</code> 밖의 <code>&lt;body&gt;</code>로
            이동해 렌더링됩니다. (요소 검사로 확인해 보세요)
          </p>
          <button @click="modalOpen = false">닫기</button>
        </div>
      </div>
    </Teleport>

    <h2>Suspense + defineAsyncComponent</h2>
    <Suspense>
      <template #default>
        <AsyncProfile />
      </template>
      <template #fallback>
        <p class="loading">프로필 로딩 중...</p>
      </template>
    </Suspense>
  </div>
</template>

<script setup>
import { ref, defineAsyncComponent } from 'vue'

const modalOpen = ref(false)

// 지연 로딩: 방문 시점에 코드 스플리팅되어 로드
const AsyncProfile = defineAsyncComponent(() =>
  import('./components/AsyncProfile.vue')
)
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.loading { color: #42b883; }
button { padding: 6px 16px; cursor: pointer; }
</style>

<style>
/* Teleport로 이동된 DOM은 이 컴포넌트 밖이므로 전역 스타일로 작성 */
.modal-backdrop {
  position: fixed; inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex; align-items: center; justify-content: center;
  z-index: 1000;
}
.modal {
  background: white; padding: 24px; border-radius: 8px;
  width: 320px; text-align: center;
}
.modal code { background: #f5f5f5; padding: 2px 6px; border-radius: 4px; }
</style>
