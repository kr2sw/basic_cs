// lazy로 분리되어 필요할 때만 다운로드되는 무거운 컴포넌트 예시
// 실제 앱에서는 차트/지도/에디터 같은 큰 라이브러리를 여기에 두면 좋다
export default function ExpensiveChart({ items }) {
  // 무거운 연산을 흉내 내는 300ms 블로킹
  const start = performance.now()
  while (performance.now() - start < 300) {}

  const bars = items.slice(0, 12).map(it => ({
    ...it,
    height: (it.id * 37) % 100 + 10,
  }))

  return (
    <div style={{ margin: '8px 0' }}>
      <h3>막대 차트 (lazy)</h3>
      <div style={{ display: 'flex', alignItems: 'flex-end', gap: 4, height: 120 }}>
        {bars.map(b => (
          <div
            key={b.id}
            style={{
              width: 18,
              height: `${b.height}%`,
              background: b.done ? '#2ecc71' : '#3498db',
            }}
            title={b.name}
          />
        ))}
      </div>
    </div>
  )
}
