<template>
  <div class="qty">
    <button @click="decrement" :disabled="quantity <= 1">-</button>
    <span class="num">{{ quantity }}</span>
    <button @click="increment">+</button>
  </div>
</template>

<script>
export default {
  props: { quantity: { type: Number, required: true } },
  emits: ['update:quantity'],
  methods: {
    increment() { this.$emit('update:quantity', this.quantity + 1) },
    decrement() { this.$emit('update:quantity', this.quantity - 1) }
  }
}
</script>

<style scoped>
.qty { display: flex; align-items: center; gap: 8px; }
.num { min-width: 24px; text-align: center; font-weight: bold; }
button { padding: 4px 12px; cursor: pointer; background: #007bff; color: white; border: none; border-radius: 4px; }
button:disabled { background: #ccc; cursor: not-allowed; }
</style>
