# 24: 폼 검증 — VeeValidate, 커스텀 규칙

## 설치

```bash
npm install vee-validate yup @vee-validate/yup
```

## 기본 사용 (useField)

```js
import { useField, useForm } from 'vee-validate'

const { handleSubmit } = useForm()
const email = useField('email')
```

`useField`가 반환하는 `value`, `errorMessage`, `handleChange` 등을
`v-model`로 바인딩하면 자동으로 검증이 실행됩니다.

## yup 스키마 검증

```js
import * as yup from 'yup'
import { toTypedSchema } from '@vee-validate/yup'

const schema = toTypedSchema(yup.object({
  email: yup.string().required('이메일을 입력하세요').email('이메일 형식이 아닙니다'),
  password: yup.string().required('비밀번호를 입력하세요').min(6, '6자 이상이어야 합니다')
}))

const { handleSubmit } = useForm({ validationSchema: schema })
```

## 커스텀 규칙

`yup`의 `.test()`로 특정 도메인 규칙을 추가합니다.

```js
nickname: yup.string().test(
  'korean',
  '한글 2~10자만 허용됩니다',
  (v) => /^[가-힣]{2,10}$/.test(v || '')
)
```

## 검증 흐름

```
입력 → 실시간 검증 (validateOnModelUpdate)
  → 성공: 제출 실행
  → 실패: errorMessage 표시, 제출 차단
```

## 필드 단독 검증 (커스텀 규칙 전역 등록)

`@vee-validate/rules`의 규칙을 전역에 등록하면 모든 폼에서 사용할 수 있습니다.

```js
import { defineRule } from 'vee-validate'
import { required, email, min } from '@vee-validate/rules'

defineRule('required', required)
defineRule('email', email)
defineRule('min', min)
```

```js
const password = useField('password', 'required|min:6')
```

### 자체 커스텀 규칙 등록

```js
defineRule('koreanNick', (value) => {
  if (!value) return true
  return /^[가-힣]{2,10}$/.test(value) || '한글 2~10자만 허용됩니다'
})
```

## 검증 시점 옵션

`validateOnBlur`, `validateOnChange`, `validateOnModelUpdate` 등
폼/필드 옵션으로 검증 시점을 조절할 수 있습니다.

```js
const email = useField('email', 'required|email', {
  validateOnBlur: true // 포커스가 벗어날 때 검증
})
```

## 에러 요약 표시

```js
const { errors } = useForm() // { email: '...', password: '...' }
```

모든 필드의 에러를 한 번에 취합해 상단에 요약으로 보여줄 수 있습니다.

## 실행

```bash
npm install && npx vite serve .
```
