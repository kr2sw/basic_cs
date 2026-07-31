<template>
  <div>
    <h2>통계 개요</h2>
    <div class="stats">
      <div class="stat-card">
        <p class="stat-label">전체 할 일</p>
        <p class="stat-value">{{ store.totalCount }}</p>
      </div>
      <div class="stat-card">
        <p class="stat-label">완료</p>
        <p class="stat-value">{{ store.doneCount }}</p>
      </div>
      <div class="stat-card">
        <p class="stat-label">진행률</p>
        <p class="stat-value">{{ store.progress }}%</p>
      </div>
    </div>

    <div class="progress-bar">
      <div class="progress-fill" :style="{ width: store.progress + '%' }"></div>
    </div>

    <h2>최근 할 일</h2>
    <p v-if="store.loading" class="loading">로딩 중...</p>
    <p v-if="store.error" class="error">{{ store.error }}</p>
    <ul v-else class="recent-list">
      <li v-for="todo in store.recent" :key="todo.id">
        <span class="badge" :class="{ done: todo.completed }">
          {{ todo.completed ? '완료' : '진행' }}
        </span>
        {{ todo.title }}
      </li>
    </ul>

    <p class="hint">
      Pinia 스토어의 getters(총개수, 완료, 진행률)가 대시보드 통계로 표시됩니다.
    </p>
  </div>
</template>

<script setup>
import { useTodosStore } from '../store/todos'

// App.vue에서 로드한 데이터를 동일 스토어로 공유
const store = useTodosStore()
</script>

<style scoped>
h2 { color: #333; border-bottom: 1px solid #eee; padding-bottom: 8px; }
.stats { display: flex; gap: 12px; margin: 12px 0; }
.stat-card { flex: 1; background: white; border: 1px solid #ddd; border-radius: 8px; padding: 16px; text-align: center; }
.stat-label { margin: 0; color: #999; font-size: 12px; }
.stat-value { margin: 6px 0 0; font-size: 28px; font-weight: bold; color: #42b883; }
.progress-bar { height: 12px; background: #eee; border-radius: 6px; overflow: hidden; }
.progress-fill { height: 100%; background: #42b883; transition: width 0.4s; }
.recent-list { list-style: none; padding: 0; }
.recent-list li { padding: 6px 0; display: flex; align-items: center; gap: 8px; }
.badge { font-size: 11px; padding: 2px 8px; border-radius: 10px; background: #fff3cd; }
.badge.done { background: #d4edda; }
.loading { color: #42b883; }
.error { color: #dc3545; }
.hint { color: #999; font-size: 12px; }
</style>
