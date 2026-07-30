<template>
  <div class="user-profile" :class="{ active: active }">
    <p><strong>{{ name }}</strong> ({{ age }}세) - {{ active ? '활성' : '비활성' }}</p>
  </div>
</template>

<script>
export default {
  props: {
    name: { type: String, required: true },
    age: { type: Number, required: true },
    active: { type: Boolean, default: false }
  }
}
</script>

<style scoped>
.user-profile { padding: 12px; background: #f9f9f9; border-radius: 4px; margin: 8px 0; }
.user-profile.active { background: #e8f5e9; border-left: 4px solid #42b883; }
</style>
