<template>
  <div class="app">
    <h1>템플릿 문법</h1>

    <h2>텍스트 보간</h2>
    <p>{{ message }}</p>
    <p>1 + 1 = {{ 1 + 1 }}</p>
    <p>{{ message }}의 길이: {{ message.length }}</p>
    <p>{{ isActive ? '활성' : '비활성' }}</p>

    <h2>v-html (raw HTML)</h2>
    <div v-html="rawHtml"></div>

    <h2>JavaScript 표현식</h2>
    <p>{{ number.toFixed(2) }}</p>
    <p>{{ fruits.join(', ') }}</p>

    <h2>v-bind (속성 바인딩)</h2>
    <input :type="inputType" :placeholder="placeholder" :disabled="isDisabled">
    <br><br>
    <button :disabled="!isActive">클릭 가능? {{ isActive }}</button>

    <h2>v-once</h2>
    <p v-once>초기값: {{ counter }}</p>
    <button @click="counter++">증가</button>
    <p>현재 counter: {{ counter }} (v-once는 변하지 않음)</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      message: 'Hello Vue!',
      isActive: true,
      isDisabled: false,
      rawHtml: '<span style="color:red">빨간색 텍스트</span>',
      number: 3.14159,
      fruits: ['Apple', 'Banana', 'Cherry'],
      inputType: 'text',
      placeholder: '이름을 입력하세요',
      counter: 100
    }
  }
}
</script>

<style>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 24px; border-bottom: 1px solid #eee; padding-bottom: 4px; }
input, button { padding: 6px 12px; margin: 4px 0; }
</style>
