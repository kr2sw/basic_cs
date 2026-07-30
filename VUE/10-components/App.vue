<template>
  <div class="app">
    <h1>컴포넌트 기초</h1>

    <h2>기본 컴포넌트</h2>
    <Counter />
    <Counter />

    <h2>Props 전달</h2>
    <Greeting name="Alice" />
    <Greeting name="Bob" />

    <h2>Slot 사용</h2>
    <Card title="Vue 공부">
      <p>Vue.js 기초부터 고급까지 학습합니다.</p>
    </Card>
    <Card title="할 일 목록">
      <ul>
        <li>예제 작성</li>
        <li>문서 정리</li>
        <li>커밋</li>
      </ul>
    </Card>

    <h2>emit 이벤트</h2>
    <AlertBox @close="onClose">이 알림을 닫으세요.</AlertBox>

    <h2>컴포넌트와 v-model</h2>
    <InputBox v-model="inputValue" />
    <p>부모 입력값: {{ inputValue }}</p>
  </div>
</template>

<script>
import Counter from './components/Counter.vue'
import Greeting from './components/Greeting.vue'
import Card from './components/Card.vue'
import AlertBox from './components/AlertBox.vue'
import InputBox from './components/InputBox.vue'

export default {
  components: { Counter, Greeting, Card, AlertBox, InputBox },
  data() {
    return {
      inputValue: 'Hello'
    }
  },
  methods: {
    onClose() {
      alert('알림 닫힘')
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
</style>
