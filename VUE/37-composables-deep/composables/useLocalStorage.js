import { ref, computed } from 'vue'

// localStorage 연동 composable
export function useLocalStorage(key, initialValue) {
  const value = ref(localStorage.getItem(key) ?? initialValue)

  const stored = computed({
    get: () => value.value,
    set: (v) => {
      value.value = v
      localStorage.setItem(key, v)
    }
  })

  return { value: stored }
}
