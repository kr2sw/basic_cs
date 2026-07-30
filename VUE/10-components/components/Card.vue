<template>
  <div class="card">
    <h3>{{ title }}</h3>
    <div class="content">
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    title: { type: String, required: true }
  }
}
</script>

<style scoped>
.card { border: 1px solid #ddd; border-radius: 8px; padding: 16px; margin: 12px 0; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
h3 { margin: 0 0 8px; color: #42b883; }
.content { color: #555; }
</style>
