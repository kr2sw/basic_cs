<template>
  <div class="app">
    <h1>전환과 애니메이션</h1>

    <h2>Transition (fade)</h2>
    <button @click="show = !show">토글</button>
    <Transition name="fade">
      <p v-if="show" class="box">fade 전환</p>
    </Transition>

    <h2>Transition (named: slide-left/right)</h2>
    <button @click="changeIndex(-1)">◀</button>
    <button @click="changeIndex(1)">▶</button>
    <Transition :name="direction" mode="out-in">
      <p :key="currentIndex" class="box">{{ slides[currentIndex] }}</p>
    </Transition>

    <h2>TransitionGroup (리스트)</h2>
    <button @click="addItem">추가</button>
    <button @click="removeItem">제거</button>
    <TransitionGroup name="list" tag="ul">
      <li v-for="item in items" :key="item.id" class="list-item">
        {{ item.text }}
      </li>
    </TransitionGroup>

    <h2>state transition (숫자 애니메이션)</h2>
    <p class="big-num">{{ animatedNumber }}</p>
    <button @click="animateTo(1000)">1000까지</button>
    <button @click="animateTo(42)">42까지</button>
  </div>
</template>

<script setup>
import { ref } from 'vue'

// fade 전환
const show = ref(true)

// named transition (방향 전환)
const slides = ref(['첫 번째', '두 번째', '세 번째'])
const currentIndex = ref(0)
const direction = ref('slide-left')

function changeIndex(delta) {
  direction.value = delta > 0 ? 'slide-left' : 'slide-right'
  currentIndex.value = (currentIndex.value + delta + slides.value.length) % slides.value.length
}

// TransitionGroup
const items = ref([
  { id: 1, text: '첫 번째 아이템' },
  { id: 2, text: '두 번째 아이템' },
  { id: 3, text: '세 번째 아이템' }
])
let nextId = 4

function addItem() {
  items.value.push({ id: nextId++, text: `아이템 ${nextId - 1}` })
}
function removeItem() {
  items.value.pop()
}

// state transition: 숫자 보간
const animatedNumber = ref(0)

function animateTo(target, duration = 800) {
  const from = animatedNumber.value
  const start = performance.now()
  const step = (now) => {
    const t = Math.min((now - start) / duration, 1)
    animatedNumber.value = Math.round(from + (target - from) * t)
    if (t < 1) requestAnimationFrame(step)
  }
  requestAnimationFrame(step)
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 6px 14px; cursor: pointer; }
.box { padding: 12px; background: #e8f5e9; border-radius: 6px; margin: 8px 0; }
.list-item { padding: 8px 12px; margin: 4px 0; background: #f5f5f5; border-radius: 4px; }
.big-num { font-size: 40px; font-weight: bold; color: #42b883; margin: 8px 0; }

/* fade */
.fade-enter-active, .fade-leave-active { transition: opacity 0.4s; }
.fade-enter-from, .fade-leave-to { opacity: 0; }

/* slide (named) */
.slide-left-enter-active, .slide-left-leave-active { transition: all 0.3s ease; }
.slide-left-enter-from { transform: translateX(60px); opacity: 0; }
.slide-left-leave-to { transform: translateX(-60px); opacity: 0; }
.slide-right-enter-active, .slide-right-leave-active { transition: all 0.3s ease; }
.slide-right-enter-from { transform: translateX(-60px); opacity: 0; }
.slide-right-leave-to { transform: translateX(60px); opacity: 0; }

/* TransitionGroup: 등장/퇴장 + 이동 */
.list-enter-active, .list-leave-active { transition: all 0.4s; }
.list-enter-from, .list-leave-to { opacity: 0; transform: translateY(20px); }
.list-move { transition: transform 0.4s; }
</style>
