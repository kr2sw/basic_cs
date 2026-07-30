<template>
  <div>
    <h2>👤 User {{ $route.params.id }}</h2>
    <p>사용자 ID: {{ $route.params.id }}</p>
    <router-link :to="{ name: 'user', params: { id: Number($route.params.id) + 1 } }">
      다음 사용자
    </router-link>
  </div>
</template>
