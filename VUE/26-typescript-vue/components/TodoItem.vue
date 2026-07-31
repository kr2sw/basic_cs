<template>
  <li class="todo">
    <input
      type="checkbox"
      :checked="todo.done"
      @change="emit('toggle')"
      :aria-label="'완료 처리: ' + todo.text"
    >
    <span :class="{ done: todo.done }">{{ todo.text }}</span>
    <button @click="emit('remove', todo.id)">x</button>
  </li>
</template>

<script setup lang="ts">
// 공용 인터페이스는 별도 파일(types.ts)로 분리하는 것이 일반적
interface Todo {
  id: number
  text: string
  done: boolean
}

// props 타입: 제네릭 문법으로 선언
const props = defineProps<{ todo: Todo }>()

// emits 타입: 이벤트명 → payload 타입 매핑
const emit = defineEmits<{
  toggle: []
  remove: [id: number]
}>()

// props는 선언 후 바로 사용 가능 (템플릿에서는 자동 노출)
console.log('렌더링:', props.todo.text)
</script>

<style scoped>
.todo { padding: 4px 0; display: flex; align-items: center; gap: 8px; }
.done { text-decoration: line-through; color: #999; }
button { cursor: pointer; }
</style>
