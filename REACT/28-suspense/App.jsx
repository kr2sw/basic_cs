import { useState, lazy, Suspense, useTransition, useDeferredValue, useMemo } from 'react'

// lazy 컴포넌트: 로드 완료 전까지 Suspense의 fallback이 표시된다
const SlowProfile = lazy(() => import('./SlowProfile'))

const BIG_LIST = Array.from({ length: 10000 }, (_, i) => `아이템 ${i} ${i % 2 ? 'react' : 'query'}`)

function App() {
  const [tab, setTab] = useState('home')      // home | profile | list
  const [count, setCount] = useState(0)
  const [query, setQuery] = useState('')

  // useTransition: 탭 전환을 비긴급 업데이트로 처리해 입력이 끊기지 않게 한다
  const [isPending, startTransition] = useTransition()

  // useDeferredValue: 무거운 필터링에 "지연된 값"을 사용
  const deferredQuery = useDeferredValue(query)
  const filtered = useMemo(
    () => BIG_LIST.filter(item => item.includes(deferredQuery.toLowerCase())),
    [deferredQuery]
  )

  const busy = tab === 'list' && deferredQuery !== query

  return (
    <div>
      <h1>Suspense & 동시성</h1>

      <section>
        <h2>useTransition — 탭 전환</h2>
        <div>
          <button onClick={() => startTransition(() => setTab('home'))}>홈</button>
          <button onClick={() => startTransition(() => setTab('profile'))}>프로필(lazy)</button>
          <button onClick={() => startTransition(() => setTab('list'))}>대형 리스트</button>
        </div>
        {isPending && <p style={{ color: 'gray' }}>전환 준비 중...</p>}

        {tab === 'home' && (
          <div>
            <p>카운터: {count}</p>
            <button onClick={() => setCount(c => c + 1)}>증가</button>
          </div>
        )}

        {tab === 'profile' && (
          <Suspense fallback={<p>프로필 로딩 중...</p>}>
            <SlowProfile />
          </Suspense>
        )}

        {tab === 'list' && (
          <div>
            <input value={query} onChange={e => setQuery(e.target.value)} placeholder="검색어 입력" />
            {busy && <p style={{ color: 'gray' }}>백그라운드 필터링 중...</p>}
            <ul style={{ maxHeight: 200, overflow: 'auto' }}>
              {filtered.slice(0, 50).map(item => <li key={item}>{item}</li>)}
            </ul>
          </div>
        )}
      </section>
    </div>
  )
}

export default App
