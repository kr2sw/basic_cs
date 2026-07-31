<template>
  <div class="app">
    <header class="header">
      <h1>📊 대시보드</h1>
      <nav class="nav">
        <router-link to="/">대시보드</router-link>
        <router-link to="/todos">할 일 관리</router-link>
      </nav>
    </header>

    <main class="content">
      <router-view />
    </main>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useTodosStore } from './store/todos'

const store = useTodosStore()

// 앱 시작 시 데이터 로드 (Dashboard/TodoList에서도 공유되는 단일 상태)
onMounted(() => store.fetchTodos())
</script>

<style>
body { margin: 0; font-family: Arial; background: #fafafa; }
.app { max-width: 800px; margin: 0 auto; padding: 20px; }
.header { display: flex; justify-content: space-between; align-items: center; border-bottom: 2px solid #42b883; padding-bottom: 12px; }
.header h1 { color: #42b883; margin: 0; font-size: 24px; }
.nav { display: flex; gap: 12px; }
.nav a { text-decoration: none; color: #333; padding: 6px 14px; border-radius: 4px; }
.nav a:hover { background: #e8f5e9; }
.nav a.router-link-exact-active { background: #42b883; color: white; }
.content { padding: 20px 0; }
</style>
