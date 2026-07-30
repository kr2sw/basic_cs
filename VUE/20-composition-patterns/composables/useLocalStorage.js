import { ref, watch } from 'vue'

export function useLocalStorage(key, defaultValue = '') {
  const value = ref(localStorage.getItem(key) || defaultValue)

  watch(value, (newVal) => {
    if (newVal === '' || newVal === null) {
      localStorage.removeItem(key)
    } else {
      localStorage.setItem(key, newVal)
    }
  })

  return { value }
}
