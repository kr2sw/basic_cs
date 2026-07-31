import { useState, useEffect, useMemo, useCallback, useRef } from 'react'

// 1. 이전 렌더링의 값을 기억하는 훅 (Latest Ref 패턴의 응용)
function usePrevious(value) {
  const ref = useRef()
  useEffect(() => { ref.current = value }, [value])
  return ref.current
}

// 2. 첫 렌더링 여부를 알려주는 훅
function useIsFirstRender() {
  const isFirst = useRef(true)
  useEffect(() => { isFirst.current = false }, [])
  return isFirst.current
}

// 3. 미디어 쿼리 감지 훅 (반응형 로직을 훅으로 추출)
function useMediaQuery(query) {
  const [matches, setMatches] = useState(() => window.matchMedia(query).matches)
  useEffect(() => {
    const mql = window.matchMedia(query)
    const onChange = () => setMatches(mql.matches)
    mql.addEventListener('change', onChange)
    return () => mql.removeEventListener('change', onChange)
  }, [query])
  return matches
}

// 4. 디바운스 훅: 입력이 멈추고 delay 후에만 값이 갱신됨 (검색/자동저장에 유용)
function useDebouncedValue(value, delay = 400) {
  const [debounced, setDebounced] = useState(value)
  useEffect(() => {
    const id = setTimeout(() => setDebounced(value), delay)
    return () => clearTimeout(id)   // cleanup: 이전 타이머 취소
  }, [value, delay])
  return debounced
}

const WORDS = ['react', 'hooks', 'memo', 'callback', 'context', 'reducer', 'lazy', 'suspense', 'query']

function App() {
  const [query, setQuery] = useState('')
  const [selected, setSelected] = useState('react')
  const [count, setCount] = useState(0)

  const isFirst = useIsFirstRender()
  const prevCount = usePrevious(count)
  const isDesktop = useMediaQuery('(min-width: 768px)')
  const debouncedQuery = useDebouncedValue(query, 400)

  // useMemo: 의존값(debouncedQuery)이 바뀔 때만 재계산
  const filtered = useMemo(() => {
    console.log('[useMemo] 필터 재계산 실행')
    return WORDS.filter(w => w.includes(debouncedQuery.toLowerCase()))
  }, [debouncedQuery])

  // useCallback: 함수 참조를 고정 -> 자식 컴포넌트 props 비교가 안정적
  const select = useCallback(word => setSelected(word), [])

  return (
    <div>
      <h1>고급 훅 패턴</h1>
      {isFirst && <p style={{ color: 'gray' }}>(첫 렌더링입니다)</p>}

      <section>
        <h2>usePrevious + 카운터</h2>
        <p>현재: {count} / 이전: {String(prevCount)}</p>
        <button onClick={() => setCount(c => c + 1)}>증가</button>
      </section>

      <section>
        <h2>useMediaQuery</h2>
        <p>데스크톱 너비(768px+) 입니까? <strong>{isDesktop ? '예' : '아니오'}</strong></p>
      </section>

      <section>
        <h2>useDebouncedValue + useMemo 필터링</h2>
        <input value={query} onChange={e => setQuery(e.target.value)} placeholder="검색어 입력 (예: hook)" />
        <p>디바운스된 검색어: <code>{debouncedQuery}</code></p>
        <ul>
          {filtered.map(word => (
            <li key={word}>
              <button onClick={() => select(word)}>{word}</button>
              {word === selected && ' ✓'}
            </li>
          ))}
        </ul>
      </section>
    </div>
  )
}

export default App
