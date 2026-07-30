<template>
  <div class="app">
    <h1>데이터 & 이벤트 바인딩</h1>

    <h2>카운터</h2>
    <p>count: {{ count }}</p>
    <button @click="increment">+1</button>
    <button @click="decrement">-1</button>
    <button @click="reset">리셋</button>

    <h2>인라인 핸들러</h2>
    <button @click="count = 0">인라인 리셋</button>

    <h2>이벤트 객체</h2>
    <button @click="handleClick">클릭 정보 보기</button>

    <h2>파라미터 전달</h2>
    <button @click="greet('Alice')">Alice 인사</button>
    <button @click="greet('Bob')">Bob 인사</button>

    <h2>v-bind 예제</h2>
    <input :value="name" readonly>
    <br>
    <a :href="link" target="_blank">{{ linkText }}</a>
    <br>
    <img :src="imageSrc" :width="imageSize" :title="imageTitle">

    <h2>this 접근</h2>
    <p>{{ fullMessage }}</p>
    <button @click="updateMessage">메시지 변경</button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      count: 0,
      name: 'Alice',
      link: 'https://vuejs.org',
      linkText: 'Vue.js 공식 문서',
      imageSrc: 'https://vuejs.org/images/logo.png',
      imageSize: 100,
      imageTitle: 'Vue 로고',
      message: 'Hello'
    }
  },
  computed: {
    fullMessage() {
      return `${this.message}, ${this.name}!`
    }
  },
  methods: {
    increment() { this.count++ },
    decrement() { this.count-- },
    reset() { this.count = 0 },
    handleClick(event) {
      alert(`type: ${event.type}, target: ${event.target.tagName}`)
    },
    greet(name) {
      alert(`Hello, ${name}!`)
    },
    updateMessage() {
      this.message = '안녕하세요'
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 6px 16px; cursor: pointer; }
input { padding: 4px 8px; }
img { border: 1px solid #ddd; border-radius: 4px; }
</style>
