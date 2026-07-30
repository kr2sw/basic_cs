<template>
  <div class="app">
    <h1>폼 입력 바인딩</h1>

    <h2>텍스트 입력</h2>
    <input v-model="message" placeholder="텍스트 입력">
    <p>입력값: {{ message }}</p>

    <h2>Textarea</h2>
    <textarea v-model="text" placeholder="여러 줄 입력" rows="3"></textarea>
    <p>{{ text }}</p>

    <h2>Checkbox</h2>
    <label><input type="checkbox" v-model="agree"> 동의합니다</label>
    <p>agree: {{ agree }}</p>

    <h2>Checkbox (배열)</h2>
    <label><input type="checkbox" v-model="hobbies" value="Reading"> 독서</label>
    <label><input type="checkbox" v-model="hobbies" value="Gaming"> 게임</label>
    <label><input type="checkbox" v-model="hobbies" value="Coding"> 코딩</label>
    <p>취미: {{ hobbies }}</p>

    <h2>Radio</h2>
    <label><input type="radio" v-model="gender" value="male"> 남성</label>
    <label><input type="radio" v-model="gender" value="female"> 여성</label>
    <p>성별: {{ gender }}</p>

    <h2>Select</h2>
    <select v-model="city">
      <option value="" disabled>선택하세요</option>
      <option value="seoul">서울</option>
      <option value="busan">부산</option>
      <option value="jeju">제주</option>
    </select>
    <p>선택: {{ city }}</p>

    <h2>Multiple Select</h2>
    <select v-model="selectedCities" multiple>
      <option value="seoul">서울</option>
      <option value="busan">부산</option>
      <option value="jeju">제주</option>
    </select>
    <p>선택: {{ selectedCities }}</p>

    <h2>v-model 수식어</h2>
    <p>.lazy: <input v-model.lazy="lazyMsg" placeholder="변경 시 동기화"></p>
    <p>lazyMsg: {{ lazyMsg }}</p>
    <p>.number: <input v-model.number="num" type="number" placeholder="자동 숫자 변환"></p>
    <p>num: {{ num }} (type: {{ typeof num }})</p>
    <p>.trim: <input v-model.trim="trimmed" placeholder="공백 제거"></p>
    <p>trimmed: '{{ trimmed }}'</p>

    <h2>전체 폼 예제</h2>
    <form @submit.prevent="submitForm">
      <p>이름: <input v-model="form.name" placeholder="이름"></p>
      <p>이메일: <input v-model="form.email" type="email" placeholder="이메일"></p>
      <p>나이: <input v-model.number="form.age" type="number"></p>
      <p>
        성별:
        <label><input type="radio" v-model="form.gender" value="male"> 남</label>
        <label><input type="radio" v-model="form.gender" value="female"> 여</label>
      </p>
      <p><label><input type="checkbox" v-model="form.terms"> 약관 동의</label></p>
      <button type="submit" :disabled="!form.terms">제출</button>
    </form>
    <pre>{{ JSON.stringify(form, null, 2) }}</pre>
  </div>
</template>

<script>
export default {
  data() {
    return {
      message: '',
      text: '',
      agree: false,
      hobbies: [],
      gender: '',
      city: '',
      selectedCities: [],
      lazyMsg: '',
      num: '',
      trimmed: '',
      form: {
        name: '',
        email: '',
        age: null,
        gender: '',
        terms: false
      }
    }
  },
  methods: {
    submitForm() {
      alert(`제출됨:\n${JSON.stringify(this.form, null, 2)}`)
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
input, textarea, select { padding: 4px 8px; margin: 4px 0; }
textarea { width: 100%; }
label { margin-right: 12px; cursor: pointer; }
button { padding: 8px 24px; cursor: pointer; }
pre { background: #f5f5f5; padding: 12px; border-radius: 4px; margin-top: 8px; }
</style>
