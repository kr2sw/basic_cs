// React.lazy로 분리되는 컴포넌트.
// 실제 앱에서는 무거운 라이브러리를 import 하는 컴포넌트를 넣어 번들을 분할한다.
export default function SlowProfile() {
  // 로드 후 렌더링 전 1.5초 추가 지연을 흉내 (데이터 로딩 시뮬레이션)
  const start = performance.now()
  while (performance.now() - start < 300) {}

  return (
    <div style={{ border: '1px solid #ccc', padding: 12, marginTop: 8 }}>
      <h3>프로필 (lazy 컴포넌트)</h3>
      <p>이 컴포넌트는 별도 청크로 분리되어 탭을 열 때 로드됩니다.</p>
      <p>동기 렌더링이라면 Suspense fallback이 아닌 <code>useEffect</code>로 처리했겠지만,
         lazy는 로딩 중 상태를 선언적으로 표현합니다.</p>
    </div>
  )
}
