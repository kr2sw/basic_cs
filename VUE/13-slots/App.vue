<template>
  <div class="app">
    <h1>Slots</h1>

    <h2>기본 Slot</h2>
    <Card>
      <p>이것은 기본 슬롯 콘텐츠입니다.</p>
    </Card>

    <h2>Named Slots</h2>
    <Layout>
      <template #header>
        <h2>헤더 영역</h2>
      </template>
      <p>메인 콘텐츠입니다.</p>
      <p>여러 줄도 가능합니다.</p>
      <template #footer>
        <small>푸터 영역 © 2026</small>
      </template>
    </Layout>

    <h2>Fallback Content</h2>
    <Card />
    <Card>
      <p>이 카드는 커스텀 콘텐츠가 있습니다.</p>
    </Card>

    <h2>Scoped Slot</h2>
    <UserList :users="users">
      <template #default="{ user, index }">
        <div class="user-item">
          <span class="idx">{{ index + 1 }}</span>
          <strong>{{ user.name }}</strong> ({{ user.age }}세)
          <span :class="user.active ? 'active' : 'inactive'">
            {{ user.active ? 'Online' : 'Offline' }}
          </span>
        </div>
      </template>
    </UserList>
  </div>
</template>

<script>
import Card from './components/Card.vue'
import Layout from './components/Layout.vue'
import UserList from './components/UserList.vue'

export default {
  components: { Card, Layout, UserList },
  data() {
    return {
      users: [
        { id: 1, name: 'Alice', age: 25, active: true },
        { id: 2, name: 'Bob', age: 30, active: false },
        { id: 3, name: 'Charlie', age: 28, active: true }
      ]
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.user-item { padding: 8px; background: #f9f9f9; border-radius: 4px; margin: 4px 0; display: flex; gap: 12px; align-items: center; }
.idx { background: #42b883; color: white; width: 24px; height: 24px; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-size: 12px; }
.active { color: #28a745; font-size: 12px; }
.inactive { color: #999; font-size: 12px; }
</style>
