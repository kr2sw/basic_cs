<template>
  <div class="user-list">
    <div v-for="(user, index) in users" :key="user.id">
      <slot :user="user" :index="index"></slot>
    </div>
  </div>
</template>

<script>
export default {
  props: { users: { type: Array, required: true } }
}
</script>
