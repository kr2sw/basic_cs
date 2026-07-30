<template>
  <div class="app">
    <h1>Computed & Watch (Composition)</h1>

    <h2>Computed 기본</h2>
    <input v-model="text" placeholder="입력">
    <p>원본: {{ text }}</p>
    <p>뒤집기: {{ reversed }}</p>
    <p>길이: {{ len }}</p>

    <h2>Computed getter/setter</h2>
    <input v-model="fullName" placeholder="전체 이름">
    <p>first: {{ first }}, last: {{ last }}</p>

    <h2>watch (단일 값)</h2>
    <input v-model="keyword" placeholder="검색어">
    <p>검색: {{ searchResult }}</p>

    <h2>watch (다중 값)</h2>
    <p>x: <input v-model.number="x" type="number"></p>
    <p>y: <input v-model.number="y" type="number"></p>
    <p class="log">watch 로그: {{ watchLog }}</p>

    <h2>watch (객체 깊은 감시)</h2>
    <p>user: {{ user }}</p>
    <button @click="user.name = 'Bob'">이름 변경</button>
    <button @click="user.address.city = 'Busan'">도시 변경</button>
    <p class="log">deep log: {{ deepLog }}</p>

    <h2>watchEffect</h2>
    <p>count: {{ count }}</p>
    <button @click="count++">+1</button>
    <p class="log">watchEffect 로그: {{ effectLog }}</p>

    <h2>watchPostEffect (DOM 업데이트 후)</h2>
    <p ref="elRef">이 텍스트: {{ count }}</p>
    <button @click="count++">변경 후 DOM 읽기 (콘솔 확인)</button>
  </div>
</template>

<script>
import { ref, computed, watch, watchEffect, watchPostEffect, nextTick } from 'vue'

export default {
  setup() {
    const text = ref('Hello')
    const reversed = computed(() => text.value.split('').reverse().join(''))
    const len = computed(() => text.value.length)

    const first = ref('John')
    const last = ref('Doe')
    const fullName = computed({
      get: () => `${first.value} ${last.value}`,
      set: (val) => {
        const parts = val.split(' ')
        first.value = parts[0] || ''
        last.value = parts.slice(1).join(' ') || ''
      }
    })

    const keyword = ref('')
    const searchResult = ref('검색어를 입력하세요')

    watch(keyword, (newVal, oldVal) => {
      if (newVal) {
        searchResult.value = `"${newVal}" 검색 중...`
        setTimeout(() => {
          searchResult.value = `"${newVal}"에 대한 결과: ${newVal.length}건`
        }, 500)
      } else {
        searchResult.value = '검색어를 입력하세요'
      }
    })

    const x = ref(0)
    const y = ref(0)
    const watchLog = ref('')

    watch([x, y], ([newX, newY], [oldX, oldY]) => {
      watchLog.value = `x: ${oldX}→${newX}, y: ${oldY}→${newY}, 합: ${newX + newY}`
    })

    const user = ref({ name: 'Alice', address: { city: 'Seoul' } })
    const deepLog = ref('')

    watch(user, (newVal) => {
      deepLog.value = `user 변경: ${JSON.stringify(newVal)}`
    }, { deep: true })

    const count = ref(0)
    const effectLog = ref('')

    watchEffect(() => {
      effectLog.value = `watchEffect: count = ${count.value}`
    })

    watchPostEffect(() => {
      console.log(`DOM 업데이트 후 count: ${count.value}`)
    })

    return {
      text, reversed, len,
      first, last, fullName,
      keyword, searchResult,
      x, y, watchLog,
      user, deepLog,
      count, effectLog
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
input { padding: 4px 8px; margin: 4px 0; }
button { margin: 4px; padding: 4px 12px; cursor: pointer; }
.log { background: #f5f5f5; padding: 8px; border-radius: 4px; font-size: 13px; color: #666; }
</style>
