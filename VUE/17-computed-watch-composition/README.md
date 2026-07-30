# 17: Computed & Watch (Composition) — 계산과 감시

## computed

```js
import { computed } from 'vue'

const double = computed(() => count.value * 2)

// getter/setter
const fullName = computed({
  get: () => `${first.value} ${last.value}`,
  set: (val) => { /* ... */ }
})
```

## watch

```js
import { watch } from 'vue'

watch(count, (newVal, oldVal) => { /* ... */ })
watch([count, name], ([newC, newN], [oldC, oldN]) => { /* ... */ })
watch(() => obj.value.property, (newVal) => { /* ... */ })
```

## watchEffect

의존성을 자동으로 추적하여 실행됩니다.
```js
watchEffect(() => {
  console.log(count.value)  // count가 변경될 때마다 실행
})
```
