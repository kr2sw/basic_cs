<template>
  <div class="app">
    <h1>Vue Router</h1>

    <nav class="nav">
      <router-link to="/" exact>Home</router-link>
      <router-link to="/about">About</router-link>
      <router-link :to="{ name: 'user', params: { id: 1 } }">User 1</router-link>
      <router-link :to="{ name: 'user', params: { id: 2 } }">User 2</router-link>
      <router-link to="/search?q=vue">Search</router-link>
    </nav>

    <div class="content">
      <router-view />
    </div>

    <div class="info">
      <p>현재 경로: {{ $route.path }}</p>
      <p>라우트 이름: {{ $route.name }}</p>
      <p>쿼리: {{ $route.query }}</p>
    </div>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
body { margin: 0; font-family: Arial; }
.app { max-width: 700px; margin: 0 auto; padding: 20px; }
h1 { color: #42b883; }
.nav { display: flex; gap: 12px; padding: 12px 0; border-bottom: 2px solid #42b883; margin-bottom: 20px; }
.nav a { text-decoration: none; color: #333; padding: 4px 12px; border-radius: 4px; }
.nav a:hover { background: #e8f5e9; }
.nav a.router-link-exact-active { background: #42b883; color: white; }
.content { min-height: 200px; padding: 20px 0; }
.info { margin-top: 20px; padding: 12px; background: #f5f5f5; border-radius: 4px; font-size: 13px; color: #666; }
</style>
