<template>
  <div class="app">
    <h1>Component Props</h1>

    <h2>기본 Props</h2>
    <UserProfile name="Alice" :age="25" :active="true" />

    <h2>다양한 타입</h2>
    <ItemDisplay
      title="Vue 가이드"
      :price="25000"
      :tags="['book', 'vue', 'frontend']"
      :in-stock="true"
      :metadata="{ author: 'Evan You', pages: 300 }"
    />

    <h2>Props 검증</h2>
    <ValidatedInput
      label="이메일"
      type="email"
      placeholder="email@example.com"
      :required="true"
    />

    <h2>Props 드릴링</h2>
    <Parent :level="1" />
  </div>
</template>

<script>
import UserProfile from './components/UserProfile.vue'
import ItemDisplay from './components/ItemDisplay.vue'
import ValidatedInput from './components/ValidatedInput.vue'
import Parent from './components/Parent.vue'

export default {
  components: { UserProfile, ItemDisplay, ValidatedInput, Parent }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
</style>
