import { useState, useMemo, useCallback, memo, lazy, Suspense, Profiler } from 'react'

// lazy: 이 파일은 별도 청크로 분리되어 필요할 때만 로드된다 (코드 스플리팅)
const ExpensiveChart = lazy(() => import('./ExpensiveChart'))

// memo: props가 같으면 리렌더하지 않는다
const ListItem = memo(function ListItem({ item, onToggle }) {
  return (
    <li>
      <button onClick={() => onToggle(item.id)}>{item.name}</button>
      {item.done ? ' ✓' : ''}
    </li>
  )
})

function App() {
  const [items, setItems] = useState(() =>
    Array.from({ length: 5000 }, (_, i) => ({ id: i, name: `항목 ${i}`, done: i % 3 === 0 }))
  )
  const [count, setCount] = useState(0)
  const [showChart, setShowChart] = useState(false)

  // useCallback: onToggle 참조를 고정해야 ListItem의 memo가 효과를 발휘한다
  const toggle = useCallback(id => {
    setItems(list => list.map(it => it.id === id ? { ...it, done: !it.done } : it))
  }, [])

  // useMemo: 전체 항목 수가 바뀔 때만 재계산 (비싼 계산 시뮬레이션)
  const stats = useMemo(() => {
    const done = items.filter(i => i.done).length
    return { total: items.length, done }
  }, [items])

  return (
    <div>
      <h1>성능 최적화</h1>

      <section>
        <h2>Profiler — 렌더링 시간 측정</h2>
        <button onClick={() => setCount(c => c + 1)}>App 리렌더 (count: {count})</button>
        <button onClick={() => setShowChart(s => !s)}>
          {showChart ? '차트 숨기기' : '차트 lazy 로드'}
        </button>
      </section>

      {/* Profiler: 자식 트리의 렌더링 시간을 콜백으로 수집 */}
      <Profiler
        id="ItemList"
        onRender={(id, phase, actualDuration) => {
          console.log(`[Profiler] ${id} / ${phase} / ${actualDuration.toFixed(2)}ms`)
        }}
      >
        {/* lazy 컴포넌트는 Suspense로 감싸야 한다 */}
        {showChart && (
          <Suspense fallback={<p>차트 로딩 중...</p>}>
            <ExpensiveChart items={items} />
          </Suspense>
        )}
        <ul>
          {items.map(item => <ListItem key={item.id} item={item} onToggle={toggle} />)}
        </ul>
      </Profiler>

      <p>통계: {stats.done} / {stats.total} 완료</p>
    </div>
  )
}

export default App
