<template>
  <div class="layout">
    <header><slot name="header">기본 헤더</slot></header>
    <main><slot>기본 콘텐츠</slot></main>
    <footer><slot name="footer">기본 푸터</slot></footer>
  </div>
</template>

<style scoped>
.layout { border: 1px solid #ddd; border-radius: 8px; margin: 8px 0; }
header { background: #42b883; color: white; padding: 12px 16px; border-radius: 8px 8px 0 0; }
main { padding: 16px; min-height: 60px; }
footer { background: #f5f5f5; padding: 8px 16px; border-radius: 0 0 8px 8px; font-size: 13px; color: #666; }
</style>
