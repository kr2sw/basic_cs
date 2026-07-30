<template>
  <div class="app">
    <h1>{{ title }}</h1>
    <p>{{ message }}</p>
    <p>카운트: {{ count }}</p>
    <p>뒤집은 메시지: {{ message.split('').reverse().join('') }}</p>
    <p>현재 시간: {{ new Date().toLocaleString() }}</p>

    <hr>
    <h2>v-html (raw HTML)</h2>
    <div v-html="rawHtml"></div>

    <h2>v-bind</h2>
    <a v-bind:href="linkUrl" target="_blank">Vue.js 공식 사이트</a>
    <br>
    <img v-bind:src="imageSrc" :alt="imageAlt" width="200">

    <h2>v-once (한 번만 렌더링)</h2>
    <p v-once>{{ onceMessage }}</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      title: 'Vue.js 소개',
      message: 'Hello, Vue 3!',
      count: 42,
      rawHtml: '<strong>굵은 글씨</strong>와 <em>기울임</em>',
      linkUrl: 'https://vuejs.org',
      imageSrc: 'https://vuejs.org/images/logo.png',
      imageAlt: 'Vue 로고',
      onceMessage: '이 메시지는 한 번만 렌더링됩니다.'
    }
  }
}
</script>

<style>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
img { border: 1px solid #ddd; border-radius: 4px; }
</style>
