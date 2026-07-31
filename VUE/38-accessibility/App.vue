<template>
  <div class="app">
    <h1>접근성 (ARIA, 포커스, 키보드)</h1>

    <!-- 스킵 링크: 키보드 사용자가 본문으로 바로 이동 -->
    <a href="#main-content" class="skip-link">본문으로 건너뛰기</a>

    <main id="main-content">
      <h2>키보드 네비게이션 (목록)</h2>
      <p class="hint">방향키로 이동, Enter로 선택</p>
      <ul
        class="menu"
        role="listbox"
        tabindex="0"
        @keydown="onMenuKeydown"
        @focus="menuFocused = true"
        @blur="menuFocused = false"
        :aria-activedescendant="`option-${activeIndex}`"
      >
        <li
          v-for="(option, i) in options"
          :id="`option-${i}`"
          :key="i"
          role="option"
          :aria-selected="selectedIndex === i"
          :class="{ active: activeIndex === i, selected: selectedIndex === i }"
        >
          {{ option }}
        </li>
      </ul>
      <p>선택됨: {{ options[selectedIndex] }}</p>

      <h2>포커스 트랩 모달</h2>
      <button @click="openModal">모달 열기</button>

      <!-- Teleport로 body에 렌더링 + aria 속성 -->
      <Teleport to="body">
        <div v-if="modalOpen" class="backdrop" @click.self="closeModal">
          <div
            ref="modalRef"
            class="modal"
            role="dialog"
            aria-modal="true"
            aria-labelledby="modal-title"
            tabindex="-1"
            @keydown="onModalKeydown"
          >
            <h3 id="modal-title">포커스 트랩 모달</h3>
            <p>Tab 키가 모달 안을 벗어나지 못합니다. Esc로 닫을 수 있습니다.</p>
            <button @click="closeModal">닫기</button>
            <button @click="closeModal">취소</button>
          </div>
        </div>
      </Teleport>

      <h2>ARIA 예시</h2>
      <button
        role="switch"
        :aria-checked="notifyOn"
        @click="notifyOn = !notifyOn"
        :class="{ on: notifyOn }"
      >
        알림 {{ notifyOn ? '켜짐' : '꺼짐' }}
      </button>
      <p aria-live="polite">{{ notifyOn ? '알림이 켜졌습니다.' : '알림이 꺼졌습니다.' }}</p>
    </main>
  </div>
</template>

<script setup>
import { ref, nextTick } from 'vue'

// ---- 키보드 네비게이션 ----
const options = ['사과', '바나나', '체리', '포도']
const activeIndex = ref(0)
const selectedIndex = ref(0)

function onMenuKeydown(e) {
  if (e.key === 'ArrowDown') {
    activeIndex.value = (activeIndex.value + 1) % options.length
  } else if (e.key === 'ArrowUp') {
    activeIndex.value = (activeIndex.value - 1 + options.length) % options.length
  } else if (e.key === 'Enter' || e.key === ' ') {
    selectedIndex.value = activeIndex.value
  }
}

// ---- 포커스 트랩 모달 ----
const modalOpen = ref(false)
const modalRef = ref(null)
const lastFocused = ref(null)

function openModal() {
  lastFocused.value = document.activeElement
  modalOpen.value = true
  nextTick(() => modalRef.value?.focus())
}

function closeModal() {
  modalOpen.value = false
  // 닫힌 후 이전 포커스로 복원
  nextTick(() => lastFocused.value?.focus())
}

function onModalKeydown(e) {
  if (e.key === 'Escape') {
    closeModal()
    return
  }
  if (e.key !== 'Tab') return

  // 포커스 트랩: Tab/Shift+Tab으로 모달 안 순환
  const focusables = modalRef.value.querySelectorAll(
    'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
  )
  const first = focusables[0]
  const last = focusables[focusables.length - 1]

  if (e.shiftKey && document.activeElement === first) {
    e.preventDefault()
    last.focus()
  } else if (!e.shiftKey && document.activeElement === last) {
    e.preventDefault()
    first.focus()
  }
}

// ---- ARIA switch ----
const notifyOn = ref(false)
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.hint { color: #999; font-size: 12px; }
.skip-link { position: absolute; left: -9999px; background: #42b883; color: white; padding: 8px 12px; }
.skip-link:focus { left: 8px; top: 8px; z-index: 100; }
.menu { list-style: none; padding: 0; margin: 8px 0; max-width: 300px; }
.menu li { padding: 8px 12px; cursor: pointer; }
.menu li.active { background: #e8f5e9; outline: 2px solid #42b883; }
.menu li.selected { background: #42b883; color: white; }
button { padding: 6px 14px; cursor: pointer; }
button.on { background: #42b883; color: white; border-color: #42b883; }
</style>

<style>
.backdrop {
  position: fixed; inset: 0; background: rgba(0, 0, 0, 0.5);
  display: flex; align-items: center; justify-content: center; z-index: 1000;
}
.modal { background: white; padding: 24px; border-radius: 8px; width: 340px; }
.modal:focus { outline: 2px solid #42b883; }
</style>
