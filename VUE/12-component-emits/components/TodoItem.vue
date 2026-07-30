<template>
  <div class="todo">
    <span>{{ title }}</span>
    <div class="actions">
      <button @click="$emit('complete', title)">완료</button>
      <button @click="$emit('delete', title)" class="danger">삭제</button>
    </div>
  </div>
</template>

<script>
export default {
  props: { title: { type: String, required: true } },
  emits: ['complete', 'delete']
}
</script>

<style scoped>
.todo { display: flex; justify-content: space-between; align-items: center; padding: 12px; background: #f9f9f9; border-radius: 4px; margin: 8px 0; }
button { padding: 4px 12px; cursor: pointer; margin-left: 4px; }
.danger { background: #dc3545; color: white; border: none; border-radius: 4px; }
</style>
