# 15: Composition API — Composition API

## Options API vs Composition API

### Options API
```js
export default {
  data() { return { count: 0 } },
  computed: { double() { return this.count * 2 } },
  methods: { increment() { this.count++ } },
  mounted() { console.log('mounted') }
}
```

### Composition API
```js
import { ref, computed, onMounted } from 'vue'
export default {
  setup() {
    const count = ref(0)
    const double = computed(() => count.value * 2)
    function increment() { count.value++ }
    onMounted(() => console.log('mounted'))
    return { count, double, increment }
  }
}
```

## setup() 함수

컴포넌트가 생성되기 전에 실행됩니다.
- `props`와 `context`를 파라미터로 받습니다.
- 반환된 값은 템플릿에서 사용 가능합니다.
- `this`로 접근할 수 없습니다.
