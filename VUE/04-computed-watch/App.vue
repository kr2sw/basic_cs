<template>
  <div class="app">
    <h1>Computed & Watch</h1>

    <h2>Computed 기본</h2>
    <input v-model="message" placeholder="메시지 입력">
    <p>원본: {{ message }}</p>
    <p>뒤집기: {{ reversedMessage }}</p>
    <p>길이: {{ messageLength }}</p>

    <h2>Computed vs Method</h2>
    <p>computed: {{ computedNow }}</p>
    <p>method: {{ methodNow() }}</p>
    <p>※ computed는 캐싱되어 재계산 안 함, method는 매번 실행</p>

    <h2>Computed getter/setter</h2>
    <p>first: <input v-model="first"></p>
    <p>last: <input v-model="last"></p>
    <p>fullName: <input v-model="fullName"></p>

    <h2>Watch 예제</h2>
    <p>
      질문: <input v-model="question" placeholder="?로 끝나는 질문">
    </p>
    <p>답변: {{ answer }}</p>

    <h2>Watch 옵션</h2>
    <p>deep watch: {{ user }}</p>
    <button @click="user.name = 'Bob'">이름 변경</button>
    <button @click="user.age++">나이 증가</button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      message: 'Hello',
      first: 'John',
      last: 'Doe',
      question: '',
      answer: '질문을 입력하세요',
      user: { name: 'Alice', age: 25 }
    }
  },
  computed: {
    reversedMessage() {
      return this.message.split('').reverse().join('')
    },
    messageLength() {
      return this.message.length
    },
    computedNow() {
      return Date.now()
    },
    fullName: {
      get() { return `${this.first} ${this.last}` },
      set(val) {
        const parts = val.split(' ')
        this.first = parts[0] || ''
        this.last = parts[1] || ''
      }
    }
  },
  methods: {
    methodNow() {
      return Date.now()
    },
    getAnswer() {
      this.answer = '생각 중...'
      setTimeout(() => {
        this.answer = '그것은 좋은 질문입니다!'
      }, 1000)
    }
  },
  watch: {
    question(newVal) {
      if (newVal.includes('?')) {
        this.getAnswer()
      }
    },
    user: {
      handler(newVal) {
        console.log('user 변경:', JSON.stringify(newVal))
      },
      deep: true
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
input { padding: 4px 8px; margin: 4px 0; width: 200px; }
button { margin: 4px; padding: 6px 16px; cursor: pointer; }
</style>
