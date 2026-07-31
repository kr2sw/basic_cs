# 35: TypeScript + React — Generic Components & Event Types

타입 안전한 컴포넌트를 작성합니다. 이 챕터는 `.jsx` 대신 **`.tsx` + `tsconfig.json`** 을 사용합니다.

## Props 타입 정의

컴포넌트의 계약(API)을 인터페이스로 선언합니다. IDE 자동 완성과 호출부 타입 검사가 따라옵니다.

```tsx
interface ButtonProps {
  label: string
  onClick?: () => void
  disabled?: boolean
}

function Button({ label, onClick, disabled }: ButtonProps) {
  return <button onClick={onClick} disabled={disabled}>{label}</button>
}
```

## 이벤트 타입

핸들러 매개변수에 구체적인 이벤트 타입을 지정합니다.

```tsx
function Form() {
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    console.log(e.target.value)   // e.target.value는 string으로 추론
  }
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()
  }
  return <form onSubmit={handleSubmit}><input onChange={handleChange} /></form>
}
```

## 제네릭 컴포넌트

`T`로 값을 매개화하면 **컴포넌트가 받는 props에 따라 타입이 자동 결정**됩니다.

```tsx
interface SelectProps<T> {
  value: T
  options: { value: T; label: string }[]
  onChange: (value: T) => void
}

function Select<T>({ value, options, onChange }: SelectProps<T>) { ... }
```

`useState`도 타입으로 초기화할 수 있습니다: `const [list, setList] = useState<string[]>([])`. `useRef<HTMLInputElement>(null)`은 DOM 접근에 사용합니다.

## 실행

```bash
npm install -D typescript @types/react @types/react-dom && npm run dev
# 타입 검사: npx tsc --noEmit
```
