# 26: TypeScript + Vue — script setup 타입, props 타입

## `<script setup lang="ts">`

`<script setup>`에 `lang="ts"`를 추가하면 TypeScript를 바로 사용할 수 있습니다.
Vite는 esbuild로 트랜스파일하므로 별도 설정 없이 동작합니다.

```vue
<script setup lang="ts">
import { ref } from 'vue'

interface User {
  name: string
  role: 'admin' | 'user'
}

const user = ref<User>({ name: '홍길동', role: 'admin' })
</script>
```

## Props 타입 (defineProps)

`defineProps<Type>()` 제네릭 문법으로 타입을 선언합니다.
인터페이스를 받으므로 import/export가 필요하면 `import type`과 함께 사용합니다.

```ts
// components/TodoItem.vue
interface Todo {
  id: number
  text: string
  done: boolean
}

const props = defineProps<{ todo: Todo }>()
```

## Emits 타입 (defineEmits)

이벤트 이름과 페이로드 타입을 매핑으로 선언합니다.

```ts
const emit = defineEmits<{
  toggle: []
  remove: [id: number]
}>()
```

## 반응형 값 타입 추론

```ts
const count = ref(0)          // Ref<number>
const list = ref([] as Todo[]) // Ref<Todo[]>
const text = ref('')           // Ref<string>
```

`ref<T>(...)` 제네릭을 명시하면 초기값이 추론에 맞지 않아도 지정 가능합니다.

## v-model 타입 (defineModel)

Vue 3.4+ 에서는 `defineModel<T>()`로 타입이 있는 양방향 바인딩을 만들 수 있습니다.

```ts
const model = defineModel<string>()
```

## 실행

```bash
npm install && npx vite serve .
```
