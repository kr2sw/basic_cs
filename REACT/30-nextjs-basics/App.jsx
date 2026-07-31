// NOTE: 이 챕터는 Next.js 개념 학습용입니다.
// App.jsx는 vite에서 실행되는 "개념 설명판"이며, 실제 Next.js 코드는 README에 있습니다.
// Next.js 서버 기능(SSR/SSG/ISR)은 vite 환경에서는 실행할 수 없습니다.

const STRATEGIES = [
  {
    name: 'CSR',
    where: '브라우저(클라이언트)',
    best: '로그인 이후 대시보드, 상호작용이 많은 화면',
    desc: '빈 HTML + JS 번들. 초기 로딩이 느리고 SEO에 불리하지만 서버 부하가 없다.',
  },
  {
    name: 'SSR',
    where: '요청할 때마다 서버',
    best: '매 요청 최신 데이터가 필요한 화면',
    desc: '서버가 HTML을 만들어 내려주므로 SEO에 유리하다. 요청마다 서버가 일한다.',
  },
  {
    name: 'SSG',
    where: '빌드할 때 한 번',
    best: '블로그, 문서, 마케팅 페이지',
    desc: '정적 HTML이라 어떤 서버/CDN에서든 매우 빠르다.',
  },
  {
    name: 'ISR',
    where: '빌드 + revalidate 주기마다',
    best: '자주 바뀌지만 최신성이 중요한 상품 페이지',
    desc: 'SSG의 속도에 일정 주기 백그라운드 재생성을 더한 전략이다.',
  },
]

function App() {
  return (
    <div>
      <h1>Next.js 렌더링 전략</h1>
      <p>아래는 개념 표입니다. 실제 코드는 README.md의 Pages Router 예제를 참고하세요.</p>

      <table border="1" cellPadding="8">
        <thead>
          <tr><th>전략</th><th>렌더링 위치</th><th>추천 사용처</th><th>특징</th></tr>
        </thead>
        <tbody>
          {STRATEGIES.map(s => (
            <tr key={s.name}>
              <td><strong>{s.name}</strong></td>
              <td>{s.where}</td>
              <td>{s.best}</td>
              <td>{s.desc}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <pre>{`// Pages Router 예시 (Next.js 프로젝트에서만 동작)
export async function getStaticProps() { /* SSG */ }
export async function getServerSideProps() { /* SSR */ }
export const getStaticPaths = () => []; /* 동적 경로 */`}</pre>
    </div>
  )
}

export default App
