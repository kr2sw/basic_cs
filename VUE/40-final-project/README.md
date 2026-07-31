# 40: 종합 프로젝트 — 대시보드 앱 (전체 통합)

## 프로젝트 개요

지금까지 배운 내용을 모두 결합한 대시보드 앱입니다.

| 기능 | 사용 기술 |
|------|----------|
| 라우팅 | vue-router (lazy loading, meta, 404) |
| 상태 관리 | Pinia store (actions + getters) |
| 서버 통신 | axios + API 레이어 + 인터셉터 |
| 컴포넌트 | `<script setup>`, props/emits, slots |
| 스타일 | scoped CSS |

## 설치

```bash
npm install pinia vue-router axios
```

## 프로젝트 구조

```
main.js               → 앱 부트스트랩 (pinia + router)
router.js             → 라우트 정의 + 전역 가드
App.vue               → 레이아웃 (nav + router-view)
api/
  client.js           → axios 인스턴스 + 인터셉터
  todos.js            → todo API 레이어
store/
  todos.js            → Pinia 스토어 (setup store)
views/
  Dashboard.vue       → 통계 대시보드
  TodoList.vue        → 할 일 CRUD
```

## 핵심 흐름

```
TodoList.vue → store actions → api 모듈 → axios(인터셉터) → 서버
                     ↓ 응답
              store 상태 갱신 → Dashboard getters로 통계 표시
```

## 컴포넌트에서 사용 예시

```vue
<!-- TodoList.vue -->
<script setup>
import { ref } from 'vue'
import { useTodosStore } from '../store/todos'

const store = useTodosStore() // App.vue에서 로드한 상태 공유

async function addTodo() {
  await store.addTodo(newTodo.value.trim())
  newTodo.value = ''
}
</script>

<template>
  <li v-for="todo in store.todos" :key="todo.id">
    <input type="checkbox" :checked="todo.completed" @change="store.toggleTodo(todo)">
    <span :class="{ done: todo.completed }">{{ todo.title }}</span>
  </li>
</template>
```

## 레이아웃 구성

`App.vue`가 전체 레이아웃(헤더 + 네비게이션 + `<router-view>`)을 담당하고,
각 뷰는 `store`만 참조합니다. 상태 흐름이 한 방향이라 유지보수가 쉽습니다.

## 구현 체크리스트

- [ ] 인터셉터로 요청/에러 공통 처리
- [ ] 스토어에서 API 호출 (컴포넌트에서 직접 호출 금지)
- [ ] getter로 파생 상태(완료율 등) 계산
- [ ] lazy loading으로 뷰 분리
- [ ] not-found 라우트 처리

## 실행

```bash
npm install && npx vite serve .
```
