<template>
  <div class="item">
    <h3>{{ title }}</h3>
    <p>₩{{ price.toLocaleString() }}</p>
    <div class="tags">
      <span v-for="tag in tags" :key="tag" class="tag">{{ tag }}</span>
    </div>
    <p :class="{ soldout: !inStock }">{{ inStock ? '재고 있음' : '품절' }}</p>
    <p v-if="metadata" class="meta">{{ metadata.author }}, {{ metadata.pages }}쪽</p>
  </div>
</template>

<script>
export default {
  props: {
    title: { type: String, required: true },
    price: { type: Number, required: true },
    tags: { type: Array, default: () => [] },
    inStock: { type: Boolean, default: true },
    metadata: { type: Object, default: null }
  }
}
</script>

<style scoped>
.item { border: 1px solid #ddd; border-radius: 8px; padding: 16px; margin: 8px 0; }
h3 { margin: 0 0 8px; }
.tags { margin: 8px 0; }
.tag { display: inline-block; background: #e0e0e0; padding: 2px 8px; border-radius: 12px; font-size: 12px; margin-right: 4px; }
.soldout { color: #dc3545; font-weight: bold; }
.meta { color: #666; font-size: 13px; }
</style>
