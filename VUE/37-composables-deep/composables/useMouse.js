import { ref, onMounted, onUnmounted } from 'vue'

// 마우스 위치 추적 composable
export function useMouse() {
  const x = ref(0)
  const y = ref(0)

  function update(e) {
    x.value = e.clientX
    y.value = e.clientY
  }

  // 리스너 등록/해제를 생명주기로 관리
  onMounted(() => window.addEventListener('mousemove', update))
  onUnmounted(() => window.removeEventListener('mousemove', update))

  return { x, y }
}
