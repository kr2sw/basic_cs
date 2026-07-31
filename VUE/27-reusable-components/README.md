# 27: 재사용 컴포넌트 — v-model 패턴, composable props

## v-model 패턴 (defineModel)

Vue 3.4+ 에서 `defineModel()`로 컴포넌트에 `v-model`을 직접 만들 수 있습니다.
기존의 `modelValue` prop + `update:modelValue` emit 패턴을 대체합니다.

```vue
<!-- BaseInput.vue -->
<script setup>
const model = defineModel<string>()
</script>

<template>
  <input v-model="model">
</template>
```

```vue
<!-- 사용 -->
<BaseInput v-model="searchText" />
```

여러 개의 `v-model`(예: `v-model:title`, `v-model:done`)도 각각 `defineModel('title')`로
정의할 수 있습니다.

## Composable Props

자주 쓰이는 props 조합을 `defineProps`와 함께 재사용 가능한 형태로 설계합니다.

```js
const props = defineProps({
  title: String,
  subtitle: String,
  status: { type: String, default: 'default' }
})
```

## Slots과 Attrs 상속

- 기본 slot / named slot / scoped slot으로 콘텐츠를 주입
- 컴포넌트의 루트 요소에 자동으로 `class`, `style`, `id` 등 attrs가 상속됨
- `inheritAttrs: false` + `$attrs`로 직접 배치도 가능

```vue
<!-- $attrs로 특정 요소에만 상속 -->
<script setup>
defineOptions({ inheritAttrs: false })
</script>

<template>
  <div class="wrapper">
    <input v-bind="$attrs" />
  </div>
</template>
```

## Scoped Slot (부모가 콘텐츠 구성)

자식이 데이터를 부모 slot에 넘겨 부모가 렌더링을 결정합니다.

```vue
<!-- 자식: data-list -->
<template>
  <div v-for="item in items" :key="item.id">
    <slot :item="item" /> <!-- item을 slot props로 전달 -->
  </div>
</template>
```

```vue
<!-- 부모 -->
<DataList :items="users">
  <template #default="{ item }">
    <strong>{{ item.name }}</strong>
  </template>
</DataList>
```

## v-model 수식어 (defineModel)

`defineModel`은 수식어도 처리할 수 있습니다. `v-model.trim` 같은 사용자 수식어를
`defineModel('title', { set: ... })` 형태로 커스터마이즈합니다.

```js
const model = defineModel({
  set(value) {
    return value.trim() // 저장 시 앞뒤 공백 제거
  }
})
```

## 컴포넌트 설계 원칙

| 원칙 | 설명 |
|------|------|
| 단일 책임 | 한 컴포넌트는 한 가지 역할 |
| props 위로 | 상태는 부모가 소유, 자식은 props+emit으로 통신 |
| 스타일 격리 | `scoped` 스타일 + `:deep()` 활용 |
| props 검증 | `type`, `required`, `validator`로 계약 명시 |

## 실행

```bash
npm install && npx vite serve .
```
