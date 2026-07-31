<template>
  <div class="app">
    <h1>렌더 함수 (h, VNode)</h1>

    <h2>h() 로 만든 컴포넌트들</h2>

    <!-- 렌더 함수 컴포넌트: Button -->
    <VNodeButton :primary="true" @click="clicked++">프라이머리 버튼</VNodeButton>
    <VNodeButton @click="clicked++">일반 버튼</VNodeButton>
    <p>클릭 횟수: {{ clicked }}</p>

    <!-- 함수형 렌더 컴포넌트: Label -->
    <VNodeLabel :text="'빨간 레이블'" :color="'#dc3545'" />
    <VNodeLabel :text="'초록 레이블'" :color="'#42b883'" />

    <!-- Slot을 받는 렌더 컴포넌트 -->
    <VNodeCard>
      <template #title>렌더 함수 카드</template>
      <p>이 내용은 slot으로 전달된 VNode입니다.</p>
    </VNodeCard>

    <h2>직접 렌더링 예시</h2>
    <p>{{ 'h("div", { class: "box" }, "Hello")' }}</p>
    <div class="box">{{ renderHello() }}</div>
  </div>
</template>

<script setup>
import { ref, defineComponent, h } from 'vue'

const clicked = ref(0)

// 1) 객체 스타일 렌더 컴포넌트
const VNodeLabel = {
  props: { text: String, color: String },
  render() {
    return h('span', { style: { color: this.color, fontWeight: 'bold' } }, this.text)
  }
}

// 2) defineComponent + setup 렌더 컴포넌트 (props + emit + slots)
const VNodeButton = defineComponent({
  props: { primary: Boolean },
  emits: ['click'],
  setup(props, { slots, emit }) {
    return () =>
      h(
        'button',
        {
          class: props.primary ? 'vnode-btn primary' : 'vnode-btn',
          onClick: () => emit('click')
        },
        [slots.default ? slots.default() : '버튼']
      )
  }
})

// 3) slot을 h()로 구성하는 컴포넌트
const VNodeCard = defineComponent({
  setup(_, { slots }) {
    return () =>
      h('div', { class: 'vnode-card' }, [
        h('h3', { class: 'vnode-card-title' }, slots.title ? slots.title() : '카드'),
        h('div', { class: 'vnode-card-body' }, slots.default ? slots.default() : [])
      ])
  }
})

// 4) 템플릿에 삽입할 문자열 반환 (h() 결과 대신 실제 사용 예시)
function renderHello() {
  return h('em', 'Hello from h()').children
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.vnode-btn { padding: 6px 16px; margin: 4px; border: 2px solid #333; background: white; cursor: pointer; border-radius: 4px; }
.vnode-btn.primary { background: #42b883; border-color: #42b883; color: white; }
.vnode-card { border: 1px solid #ddd; border-radius: 8px; margin: 12px 0; overflow: hidden; }
.vnode-card-title { margin: 0; padding: 10px 14px; background: #f5f5f5; }
.vnode-card-body { padding: 14px; }
.box { padding: 8px; background: #e8f5e9; border-radius: 4px; display: inline-block; }
</style>
