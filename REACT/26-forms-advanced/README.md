# 26: 폼 처리 — React Hook Form + Zod 검증

성능 좋고 간결한 폼 처리 라이브러리와, 타입 안전한 스키마 검증 라이브러리를 조합합니다.

## 왜 React Hook Form인가?

- 불필요한 리렌더링 없이 **uncontrolled 방식**으로 빠릅니다.
- `register`로 입력을 연결하기만 하면 값과 에러를 관리합니다.
- 에러가 바뀐 필드만 리렌더링됩니다.

```jsx
const { register, handleSubmit, formState: { errors } } = useForm()

<form onSubmit={handleSubmit(onValid)}>
  <input {...register('email')} />
  {errors.email && <p>{errors.email.message}</p>}
</form>
```

## Zod 스키마 검증

Zod로 "값이 어떤 모양이어야 하는지"를 선언합니다. 스키마 하나로 런타임 검증 + TypeScript 타입 추론을 모두 얻을 수 있습니다.

```jsx
import { z } from 'zod'

const signupSchema = z.object({
  email: z.string().email('올바른 이메일이 아닙니다'),
  password: z.string().min(8, '8자 이상 입력하세요'),
  age: z.coerce.number().min(13, '13세 이상만 가입할 수 있습니다'),
})
```

## Resolver 연결

`zodResolver(schema)`를 `useForm`에 전달하면 제출 시 Zod가 검증하고 그 결과가 `errors`로 전달됩니다. 사용자 정의 메시지가 그대로 표시됩니다.

```jsx
useForm({ resolver: zodResolver(signupSchema) })
```

## Controller로 외부 라이브러리 연동

`register`는 네이티브 입력 전용입니다. 날짜 피커나 셀렉트 UI 같은 커스텀 컴포넌트는 `Controller`로 감싸 RHF에 연결합니다.

```jsx
import { Controller } from 'react-hook-form'

<Controller
  name="role"
  control={control}
  render={({ field }) => (
    <MySelect value={field.value} onChange={field.onChange} onBlur={field.onBlur} />
  )}
/>
```

## watch로 실시간 감시

`watch('email')`는 입력이 변할 때마다 해당 값을 반환합니다. 프리뷰나 활성/비활성 토글에 유용합니다.

```jsx
const email = watch('email')
<p>입력 중: {email}</p>
```

## formState 활용

`formState`는 `errors`, `isSubmitting`, `isDirty`, `isValid` 등을 제공합니다. 제출 버튼의 비활성 조건과 로딩 표시에 사용합니다.

```jsx
<button disabled={!isValid || isSubmitting}>가입하기</button>
```

## 실행

```bash
npm install react-hook-form zod @hookform/resolvers && npm run dev
```
