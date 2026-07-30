import { ref } from 'vue'

export function useMouse() {
  const x = ref(0)
  const y = ref(0)

  function onMouseMove(event) {
    x.value = event.offsetX
    y.value = event.offsetY
  }

  return { x, y, onMouseMove }
}
