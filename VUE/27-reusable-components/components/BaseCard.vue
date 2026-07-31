<template>
  <article class="card">
    <header class="card-header">
      <slot name="title">제목 없음</slot>
      <slot name="action" />
    </header>
    <div class="card-body">
      <!-- 기본 slot: 본문 콘텐츠 -->
      <slot />
    </div>
    <footer class="card-footer">
      <slot name="footer" />
    </footer>
  </article>
</template>

<style scoped>
.card { border: 1px solid #ddd; border-radius: 8px; overflow: hidden; margin: 12px 0; }
.card-header { display: flex; justify-content: space-between; align-items: center; background: #42b883; color: white; padding: 10px 14px; font-weight: bold; }
.card-body { padding: 14px; }
.card-footer { padding: 8px 14px; background: #f5f5f5; font-size: 12px; color: #666; }
</style>
