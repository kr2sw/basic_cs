import { defineStore } from 'pinia'

// Option 스토어: state / getters / actions 객체 문법
export const useCounterStore = defineStore('counter', {
  state: () => ({
    count: 0
  }),

  // getters: 파생 상태 (computed와 동일하게 캐싱됨)
  getters: {
    double: (state) => state.count * 2,
    isPositive: (state) => state.count > 0
  },

  // actions: 상태 변경 로직
  actions: {
    increment() {
      this.count++
    },
    decrement() {
      this.count--
    },
    incrementBy(n) {
      this.count += n
    }
  }
})
