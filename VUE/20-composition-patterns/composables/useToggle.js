import { ref } from 'vue'

export function useToggle(initial = false) {
  const value = ref(initial)

  function toggle() { value.value = !value.value }
  function setTrue() { value.value = true }
  function setFalse() { value.value = false }

  return { value, toggle, setTrue, setFalse }
}
