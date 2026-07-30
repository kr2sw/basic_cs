<template>
  <div class="card">
    <slot>
      <p class="fallback">기본 콘텐츠입니다.</p>
    </slot>
  </div>
</template>

<style scoped>
.card { border: 1px solid #ddd; border-radius: 8px; padding: 16px; margin: 8px 0; }
.fallback { color: #999; font-style: italic; text-align: center; }
</style>
