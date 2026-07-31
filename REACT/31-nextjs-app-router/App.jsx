// NOTE: App Router는 Next.js 프로젝트 전용입니다.
// 이 App.jsx는 "서버 컴포넌트 vs 클라이언트 컴포넌트"의 역할 구분을
// vite 환경에서 시각적으로 이해하기 위한 개념 설명 컴포넌트입니다.
// 실제 동작 코드는 README.md를 참고하세요.

import { useState } from 'react'

// 클라이언트 컴포넌트의 예: 상태와 이벤트를 사용한다 ("use client"에 해당)
function ClientCounter({ label }) {
  const [count, setCount] = useState(0)
  return (
    <div>
      <strong>{label}</strong> — count: {count}{' '}
      <button onClick={() => setCount(c => c + 1)}>+1</button>
      <p style={{ color: 'gray', fontSize: 12 }}>클라이언트에서 실행: 상태·이벤트·브라우저 API 사용 가능</p>
    </div>
  )
}

// "서버 컴포넌트처럼 동작한다"고 가정하는 프레젠테이션 컴포넌트:
// 상태가 없고, 받은 데이터를 그대로 표시만 한다
function ServerRendered({ title, description, items }) {
  return (
    <div style={{ borderTop: '1px solid #ddd', paddingTop: 8 }}>
      <h3>{title}</h3>
      <p>{description}</p>
      <ul>{items.map(i => <li key={i}>{i}</li>)}</ul>
      <p style={{ color: 'gray', fontSize: 12 }}>
        서버에서 렌더링됨 (실제 Next.js에서는 데이터 접근·비밀 로직을 여기에)
      </p>
    </div>
  )
}

function App() {
  // 실제 Next.js에서 서버 컴포넌트는 async로 직접 fetch할 수 있지만,
  // 이 예제는 vite용이라 props로 데이터를 전달받는 형태로 시뮬레이션한다.
  const posts = ['App Router 소개', 'Server Component 패턴', '캐싱과 revalidate']

  return (
    <div>
      <h1>App Router — 컴포넌트 역할 구분</h1>

      <ServerRendered
        title="게시글 목록 (서버 컴포넌트 시뮬레이션)"
        description="fetch 결과를 그대로 렌더링. JS 번들에 포함되지 않는다."
        items={posts}
      />

      <ClientCounter label="조회수 카운터 (클라이언트 컴포넌트 시뮬레이션)" />
    </div>
  )
}

export default App
