<template>
  <div class="app">
    <h1>리스트 렌더링</h1>

    <h2>기본 배열</h2>
    <ul>
      <li v-for="(fruit, index) in fruits" :key="index">{{ index + 1 }}. {{ fruit }}</li>
    </ul>

    <h2>객체 배열</h2>
    <table>
      <thead>
        <tr><th>ID</th><th>이름</th><th>나이</th></tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id">
          <td>{{ user.id }}</td>
          <td>{{ user.name }}</td>
          <td>{{ user.age }}</td>
        </tr>
      </tbody>
    </table>

    <h2>객체 순회</h2>
    <div v-for="(value, key, index) in person" :key="key">
      {{ index }}. {{ key }}: {{ value }}
    </div>

    <h2>범위 v-for</h2>
    <div v-for="n in 5" :key="n">{{ n }} </div>

    <h2>필터링/정렬된 결과</h2>
    <ul>
      <li v-for="n in evenNumbers" :key="n">{{ n }}</li>
    </ul>

    <h2>배열 조작</h2>
    <button @click="addFruit">추가</button>
    <button @click="removeFruit">제거</button>
    <button @click="sortFruits">정렬</button>
    <button @click="replaceFruits">변경 감지</button>

    <h2>template v-for</h2>
    <template v-for="item in items" :key="item.id">
      <p><strong>{{ item.name }}</strong></p>
      <p>₩{{ item.price }}</p>
    </template>

    <h2>v-for와 v-if 함께 사용</h2>
    <div v-for="todo in todos" :key="todo.id">
      <span v-if="!todo.done">☐ {{ todo.text }}</span>
      <span v-else>✅ {{ todo.text }}</span>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      fruits: ['Apple', 'Banana', 'Cherry'],
      users: [
        { id: 1, name: 'Alice', age: 25 },
        { id: 2, name: 'Bob', age: 30 },
        { id: 3, name: 'Charlie', age: 28 }
      ],
      person: { name: 'Alice', age: 25, city: 'Seoul' },
      numbers: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      items: [
        { id: 1, name: 'Laptop', price: 1500000 },
        { id: 2, name: 'Mouse', price: 30000 },
        { id: 3, name: 'Keyboard', price: 80000 }
      ],
      todos: [
        { id: 1, text: 'Vue 공부', done: true },
        { id: 2, text: '예제 작성', done: false },
        { id: 3, text: '커밋하기', done: false }
      ],
      nextId: 4
    }
  },
  computed: {
    evenNumbers() {
      return this.numbers.filter(n => n % 2 === 0)
    }
  },
  methods: {
    addFruit() {
      const fruits = ['Date', 'Elderberry', 'Fig', 'Grape']
      this.fruits.push(fruits[Math.floor(Math.random() * fruits.length)])
    },
    removeFruit() {
      this.fruits.pop()
    },
    sortFruits() {
      this.fruits.sort()
    },
    replaceFruits() {
      this.fruits = ['Mango', 'Orange', ...this.fruits]
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
table { width: 100%; border-collapse: collapse; }
th, td { padding: 8px; text-align: left; border-bottom: 1px solid #ddd; }
th { background: #f5f5f5; }
button { margin: 4px; padding: 6px 16px; cursor: pointer; }
</style>
