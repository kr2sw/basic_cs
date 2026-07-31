<template>
  <div class="app">
    <h1>재사용 컴포넌트</h1>

    <h2>v-model 패턴 (defineModel)</h2>
    <BaseInput v-model="searchText" placeholder="검색어 입력" class="search" />
    <p>검색어: {{ searchText }}</p>

    <ToggleSwitch v-model="notify" />
    <p>알림: {{ notify ? '켜짐' : '꺼짐' }}</p>

    <h2>Slots을 활용한 재사용</h2>
    <BaseCard>
      <template #title>공지사항</template>
      <template #action><button @click="counter++">좋아요 {{ counter }}</button></template>
      <p>이 본문은 기본 slot으로 주입된 콘텐츠입니다.</p>
      <template #footer>footer slot · 작성일 2026-07-31</template>
    </BaseCard>

    <BaseCard>
      <template #title>두 번째 카드</template>
      <p>같은 컴포넌트를 다른 콘텐츠로 재사용했습니다.</p>
    </BaseCard>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import BaseInput from './components/BaseInput.vue'
import BaseCard from './components/BaseCard.vue'
import ToggleSwitch from './components/ToggleSwitch.vue'

// 부모가 상태를 소유 (props 위로 / 이벤트 아래로)
const searchText = ref('')
const notify = ref(false)
const counter = ref(0)
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.search { width: 100%; padding: 8px; box-sizing: border-box; }
p { margin: 6px 0; }
button { padding: 4px 12px; cursor: pointer; }
</style>
