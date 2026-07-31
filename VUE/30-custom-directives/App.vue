<template>
  <div class="app">
    <h1>커스텀 디렉티브</h1>

    <h2>v-focus (마운트 시 자동 포커스)</h2>
    <input v-focus placeholder="자동으로 포커스됩니다" class="wide" />

    <h2>v-click-outside (바깥 클릭 감지)</h2>
    <div class="outside-area" v-click-outside="onClickOutside">
      <button @click="panelOpen = !panelOpen">패널 토글</button>
      <div v-if="panelOpen" class="panel">
        이 패널 바깥을 클릭하면 닫힙니다.
      </div>
    </div>

    <h2>v-tooltip (arg + value 활용)</h2>
    <button v-tooltip:top="'위쪽 툴팁 메시지'">마우스를 올려보세요</button>
    <button v-tooltip:bottom="'아래쪽 툴팁 메시지'">여기도요</button>

    <p class="log">{{ log }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const panelOpen = ref(false)
const log = ref('')

// 1) v-focus: 요소가 마운트될 때 focus()
const vFocus = {
  mounted: (el) => el.focus()
}

// 2) v-click-outside: 요소 바깥 클릭 감지
//    unmounted에서 리스너를 반드시 해제
const vClickOutside = {
  mounted(el, binding) {
    el._onClickOutside = (e) => {
      if (!el.contains(e.target)) binding.value(e)
    }
    document.addEventListener('click', el._onClickOutside)
  },
  unmounted(el) {
    document.removeEventListener('click', el._onClickOutside)
  }
}

// 3) v-tooltip: arg(방향) + value(내용) 사용
const vTooltip = {
  mounted(el, binding) {
    el.addEventListener('mouseenter', () => showTooltip(binding.value, binding.arg))
    el.addEventListener('mouseleave', removeTooltip)
  },
  unmounted(el) {
    el.removeEventListener('mouseenter', showTooltip)
    el.removeEventListener('mouseleave', removeTooltip)
  }
}

let tipEl = null
function showTooltip(text, position) {
  removeTooltip()
  tipEl = document.createElement('div')
  tipEl.className = 'tooltip ' + position
  tipEl.textContent = text
  document.body.appendChild(tipEl)
  setTimeout(removeTooltip, 2000)
}
function removeTooltip() {
  if (tipEl) {
    tipEl.remove()
    tipEl = null
  }
}

function onClickOutside() {
  panelOpen.value = false
  log.value = '바깥 클릭 감지! 패널이 닫혔습니다.'
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.wide { width: 100%; padding: 8px; box-sizing: border-box; }
.outside-area { border: 1px dashed #ccc; padding: 12px; border-radius: 6px; }
.panel { margin-top: 8px; padding: 12px; background: #fff3cd; border-radius: 6px; }
.log { margin-top: 16px; font-size: 12px; color: #666; background: #f5f5f5; padding: 8px; border-radius: 4px; }
</style>

<style>
/* body에 붙는 툴팁은 전역 스타일 */
.tooltip {
  position: fixed; z-index: 9999;
  background: #333; color: white;
  padding: 6px 12px; border-radius: 4px; font-size: 12px;
  transform: translate(-50%, -130%);
}
.tooltip.bottom { transform: translate(-50%, 30%); }
</style>
