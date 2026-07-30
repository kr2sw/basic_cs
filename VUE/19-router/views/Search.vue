<template>
  <div>
    <h2>🔍 Search</h2>
    <p>검색어: {{ $route.query.q || '없음' }}</p>
    <input v-model="query" placeholder="검색어 입력">
    <button @click="search">검색</button>
  </div>
</template>

<script>
export default {
  data() { return { query: '' } },
  methods: {
    search() {
      this.$router.push({ path: '/search', query: { q: this.query } })
    }
  }
}
</script>
