# 21: 고급 훅 패턴 — Advanced Hook Patterns

기본 훅을 넘어서, 커스텀 훅을 조합하고 메모이제이션으로 성능을 잡는 중급 패턴을 배웁니다.

## 커스텀 훅 합성 (Composition)

훅은 다른 훅을 호출해 조합할 수 있습니다. 작은 훅을 만들고 이를 다시 훅 안에서 재사용하면 로직이 모듈화됩니다.

```jsx
function usePrevious(value) {
  const ref = useRef()
  useEffect(() => { ref.current = value }, [value])
  return ref.current
}

function useDebouncedValue(value, delay = 300) {
  const [debounced, setDebounced] = useState(value)
  useEffect(() => {
    const id = setTimeout(() => setDebounced(value), delay)
    return () => clearTimeout(id)
  }, [value, delay])
  return debounced
}
```

## 훅의 규칙 (Rules of Hooks)

- 훅은 반드시 컴포넌트(또는 커스텀 훅)의 **최상위**에서만 호출합니다.
- 조건문, 반복문, 중첩 함수 안에서 호출하면 안 됩니다. 호출 순서가 항상 같아야 React가 상태를 매칭합니다.
- `use` 접두사는 선택이 아니라 규칙입니다. 린터(`eslint-plugin-react-hooks`)가 강제합니다.

```jsx
// ❌ 잘못된 예: 조건부 훅 호출
if (enabled) {
  const [x] = useState(0)  // 렌더링마다 호출 개수가 달라짐
}

// ✅ 올바른 예: 항상 같은 위치에서 호출
const [x, setX] = useState(0)
```

## useMemo / useCallback

렌더링 사이에 **값**(useMemo)과 **함수 참조**(useCallback)를 캐시합니다. 의존성 배열이 바뀔 때만 재계산되므로 비싼 연산이나 자식 리렌더링을 줄입니다.

```jsx
const filtered = useMemo(() => list.filter(f), [list, f])
const handleClick = useCallback(() => setCount(c => c + 1), [])
```

```jsx
// 리스트가 클수록 효과가 커지는 예제
const filtered = useMemo(() => WORDS.filter(w => w.includes(query)), [query])
const select = useCallback(word => setSelected(word), [])
```

## 최신 값 유지하기 (Latest Ref 패턴)

이펙트 안에서 "가장 최신" 값을 읽어야 할 때 `useRef`에 값을 계속 저장해 두는 패턴입니다. 스토퍼(stop) 같은 외부 요인과 싸울 때 유용합니다.

## 실행

```bash
npm install && npm run dev
```
