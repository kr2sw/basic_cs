<template>
  <div class="app">
    <h1>Component Emits</h1>

    <h2>emit 이벤트</h2>
    <p>로그: {{ log }}</p>
    <TodoItem
      title="Vue 공부하기"
      @complete="onComplete"
      @delete="onDelete"
    />

    <h2>emit with payload</h2>
    <div v-for="item in cart" :key="item.id" class="cart-item">
      {{ item.name }} - ₩{{ item.price }}
      <QuantityControl
        :quantity="item.qty"
        @update:quantity="(qty) => updateQty(item.id, qty)"
      />
    </div>

    <h2>커스텀 v-model</h2>
    <p>폰트 크기: {{ fontSize }}px</p>
    <FontSlider v-model="fontSize" :min="12" :max="40" />
    <p :style="{ fontSize: fontSize + 'px' }">이 텍스트의 크기가 변합니다.</p>
  </div>
</template>

<script>
import TodoItem from './components/TodoItem.vue'
import QuantityControl from './components/QuantityControl.vue'
import FontSlider from './components/FontSlider.vue'

export default {
  components: { TodoItem, QuantityControl, FontSlider },
  data() {
    return {
      log: '이벤트 대기 중...',
      cart: [
        { id: 1, name: '노트북', price: 1500000, qty: 1 },
        { id: 2, name: '마우스', price: 30000, qty: 2 }
      ],
      fontSize: 16
    }
  },
  methods: {
    onComplete(title) {
      this.log = `✅ 완료: ${title}`
    },
    onDelete(title) {
      this.log = `❌ 삭제: ${title}`
    },
    updateQty(id, qty) {
      const item = this.cart.find(i => i.id === id)
      if (item) item.qty = qty
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.cart-item { display: flex; align-items: center; justify-content: space-between; padding: 8px; background: #f9f9f9; border-radius: 4px; margin: 4px 0; }
</style>
