<template>
  <div class="app">
    <h1>폼 검증 (VeeValidate)</h1>

    <form @submit="onSubmit" novalidate>
      <div class="field">
        <label>이메일</label>
        <input v-model="email.value" type="email" placeholder="user@example.com">
        <p v-if="email.errorMessage" class="error">{{ email.errorMessage }}</p>
      </div>

      <div class="field">
        <label>비밀번호</label>
        <input v-model="password.value" type="password" placeholder="6자 이상">
        <p v-if="password.errorMessage" class="error">{{ password.errorMessage }}</p>
      </div>

      <div class="field">
        <label>닉네임 (커스텀 규칙: 한글 2~10자)</label>
        <input v-model="nickname.value" placeholder="예: 홍길동">
        <p v-if="nickname.errorMessage" class="error">{{ nickname.errorMessage }}</p>
      </div>

      <div class="field">
        <label>나이 (18~100)</label>
        <input v-model="age.value" type="number">
        <p v-if="age.errorMessage" class="error">{{ age.errorMessage }}</p>
      </div>

      <button type="submit" :disabled="isSubmitting">제출</button>
    </form>

    <div v-if="submitted" class="result">
      <h3>제출 성공</h3>
      <pre>{{ JSON.stringify(submitted, null, 2) }}</pre>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useForm, useField } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/yup'
import * as yup from 'yup'

// yup 스키마: 한 곳에서 모든 규칙 정의
const schema = toTypedSchema(
  yup.object({
    email: yup.string().required('이메일을 입력하세요').email('이메일 형식이 아닙니다'),
    password: yup.string().required('비밀번호를 입력하세요').min(6, '비밀번호는 6자 이상이어야 합니다'),
    nickname: yup
      .string()
      .required('닉네임을 입력하세요')
      // 커스텀 규칙: .test(name, message, fn)
      .test('korean-only', '한글 2~10자만 허용됩니다', (v) => /^[가-힣]{2,10}$/.test(v || '')),
    age: yup.number().required('나이를 입력하세요').min(18, '18세 이상만 가입 가능').max(100, '나이를 확인하세요')
  })
)

// useForm을 먼저 호출해 폼 컨텍스트를 만들고,
// useField로 필드를 연결 (v-model로 value/errorMessage 바인딩)
const { handleSubmit, isSubmitting } = useForm({ validationSchema: schema })
const email = useField('email')
const password = useField('password')
const nickname = useField('nickname')
const age = useField('age')

const submitted = ref(null)

// 모든 검증을 통과했을 때만 실행됨
const onSubmit = handleSubmit((values) => {
  submitted.value = values
  console.log('제출 성공:', values)
})
</script>

<style scoped>
.app { font-family: Arial; max-width: 520px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
.field { margin: 12px 0; }
.field label { display: block; margin-bottom: 4px; font-weight: bold; }
.field input { width: 100%; padding: 8px; box-sizing: border-box; }
.error { color: #dc3545; font-size: 12px; margin: 4px 0 0; }
button { margin-top: 12px; padding: 8px 24px; cursor: pointer; }
.result { margin-top: 20px; padding: 12px; background: #f5f5f5; border-radius: 4px; }
pre { font-size: 12px; }
</style>
