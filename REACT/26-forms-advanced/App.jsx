import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'

// 1. Zod 스키마: 값의 형태 + 검증 메시지를 한 번에 정의
const signupSchema = z.object({
  username: z.string().min(2, '닉네임은 2자 이상이어야 합니다'),
  email: z.string().email('올바른 이메일 형식이 아닙니다'),
  password: z.string().min(8, '비밀번호는 8자 이상이어야 합니다'),
  confirm: z.string(),
  age: z.coerce.number().min(13, '13세 이상만 가입할 수 있습니다'),
  terms: z.boolean().refine(v => v === true, '약관에 동의해야 합니다'),
}).refine(data => data.password === data.confirm, {
  message: '비밀번호가 일치하지 않습니다',
  path: ['confirm'],
})

// 2. 타입은 스키마에서 자동 추론
const init = { username: '', email: '', password: '', confirm: '', age: '', terms: false }

function SignupForm() {
  // 3. zodResolver로 RHF와 Zod 연결
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm({
    resolver: zodResolver(signupSchema),
    defaultValues: init,
  })

  function onValid(data) {
    alert('가입 완료!\n' + JSON.stringify(data, null, 2))
    reset(init)
  }

  return (
    <form onSubmit={handleSubmit(onValid)} noValidate>
      <div>
        <label htmlFor="username">닉네임</label>
        <input id="username" {...register('username')} />
        {errors.username && <p style={{ color: 'red' }}>{errors.username.message}</p>}
      </div>

      <div>
        <label htmlFor="email">이메일</label>
        <input id="email" type="email" {...register('email')} />
        {errors.email && <p style={{ color: 'red' }}>{errors.email.message}</p>}
      </div>

      <div>
        <label htmlFor="password">비밀번호</label>
        <input id="password" type="password" {...register('password')} />
        {errors.password && <p style={{ color: 'red' }}>{errors.password.message}</p>}
      </div>

      <div>
        <label htmlFor="confirm">비밀번호 확인</label>
        <input id="confirm" type="password" {...register('confirm')} />
        {errors.confirm && <p style={{ color: 'red' }}>{errors.confirm.message}</p>}
      </div>

      <div>
        <label htmlFor="age">나이</label>
        <input id="age" type="number" {...register('age')} />
        {errors.age && <p style={{ color: 'red' }}>{errors.age.message}</p>}
      </div>

      <div>
        <label>
          <input type="checkbox" {...register('terms')} />
          이용약관에 동의합니다
        </label>
        {errors.terms && <p style={{ color: 'red' }}>{errors.terms.message}</p>}
      </div>

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? '제출 중...' : '가입하기'}
      </button>
    </form>
  )
}

function App() {
  return (
    <div>
      <h1>회원가입 (React Hook Form + Zod)</h1>
      <SignupForm />
    </div>
  )
}

export default App
