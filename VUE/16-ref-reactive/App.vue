<template>
  <div class="app">
    <h1>ref & reactive</h1>

    <h2>ref (기본형)</h2>
    <p>count: {{ count }}</p>
    <p>double: {{ double }}</p>
    <button @click="count++">+1</button>
    <button @click="count--">-1</button>

    <h2>reactive (객체)</h2>
    <p>이름: {{ user.name }}</p>
    <p>나이: {{ user.age }}</p>
    <p>직업: {{ user.job }}</p>
    <button @click="user.age++">나이 증가</button>
    <button @click="changeJob">직업 변경</button>

    <h2>reactive 배열</h2>
    <div v-for="(item, i) in items" :key="i">
      {{ item.name }} - ₩{{ item.price }}
      <button @click="removeItem(i)">삭제</button>
    </div>

    <h2>toRefs (분해)</h2>
    <p>{{ name }}님, {{ age }}세</p>
    <button @click="age++">toRefs: 나이 증가</button>

    <h2>reactive 재할당 주의</h2>
    <p>book: {{ book?.title || '없음' }}</p>
    <button @click="loadBook1">첫 번째 책</button>
    <button @click="loadBook2">두 번째 책</button>

    <h2>shallowRef</h2>
    <p>shallow: {{ shallowObj }}</p>
    <button @click="shallowObj.value.name = 'Bob'">이름 변경 (감지 안 됨)</button>
    <button @click="shallowObj.value = { name: 'Charlie' }">객체 교체 (감지)</button>
  </div>
</template>

<script>
import { ref, reactive, toRefs, shallowRef, computed, isRef, isReactive } from 'vue'

export default {
  setup() {
    const count = ref(0)
    const double = computed(() => count.value * 2)

    const user = reactive({
      name: 'Alice',
      age: 25,
      job: 'Developer'
    })

    const items = reactive([
      { name: 'Keyboard', price: 80000 },
      { name: 'Mouse', price: 30000 }
    ])

    function changeJob() {
      user.job = user.job === 'Developer' ? 'Designer' : 'Developer'
    }

    function removeItem(index) {
      items.splice(index, 1)
    }

    const { name, age } = toRefs(user)

    const book = ref(null)
    function loadBook1() {
      book.value = { title: 'Vue.js Guide', author: 'Evan You' }
    }
    function loadBook2() {
      book.value = { title: 'JavaScript Basics', author: 'Kyle Simpson' }
    }

    const shallowObj = shallowRef({ name: 'Alice' })

    console.log('isRef(count):', isRef(count))
    console.log('isReactive(user):', isReactive(user))

    return {
      count, double,
      user, changeJob,
      items, removeItem,
      name, age,
      book, loadBook1, loadBook2,
      shallowObj
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 4px 12px; cursor: pointer; }
</style>
