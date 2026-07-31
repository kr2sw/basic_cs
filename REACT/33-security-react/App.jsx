import { useState } from 'react'

// 간단한 정화 함수: 스크립트와 이벤트 핸들러, javascript: URL 제거
// 실무에서는 DOMPurify 같은 검증된 라이브러리를 사용할 것
function sanitize(input) {
  return input
    .replace(/<script[\s\S]*?<\/script>/gi, '')
    .replace(/\son\w+\s*=/gi, '')          // onerror=, onclick= 등 제거
    .replace(/href\s*=\s*['"]?javascript:/gi, 'href="#" ')
}

function safeHref(url) {
  // https: / # 만 허용 (javascript: 등 차단)
  return /^(https?:|\/|#)/.test(url) ? url : null
}

const XSS_PAYLOAD = '<img src="x" onerror="alert(1)"> <script>alert("xss")</script>'

function App() {
  const [rawHtml, setRawHtml] = useState(XSS_PAYLOAD)

  return (
    <div>
      <h1>React 보안</h1>

      <section>
        <h2>1. JSX 기본 이스케이프 (안전)</h2>
        {/* JSX는 문자열을 텍스트로만 렌더링 — 아무것도 실행되지 않는다 */}
        <p>{'<img src=x onerror=alert(1)>'} → <strong>{'<img src=x onerror=alert(1)>'}</strong></p>
      </section>

      <section>
        <h2>2. dangerouslySetInnerHTML (위험)</h2>
        <input value={rawHtml} onChange={e => setRawHtml(e.target.value)} style={{ width: 400 }} />
        <p style={{ color: 'red', fontSize: 12 }}>
          아래는 정화 전/후를 비교합니다. <code>onerror</code>가 살아 있으면 실행될 수 있습니다.
        </p>
        {/* ❌ 위험: 정화 없이 삽입 (이 예제는 스크립트만 포함이라 실행은 안 됨) */}
        <div dangerouslySetInnerHTML={{ __html: rawHtml }} />
        {/* ✅ 안전: sanitize 후 삽입 */}
        <div dangerouslySetInnerHTML={{ __html: sanitize(rawHtml) }} />
      </section>

      <section>
        <h2>3. javascript: URL 차단</h2>
        <p>
          <a href={safeHref('https://example.com')}>안전한 링크</a> /
          <a href={safeHref('javascript:alert(1)')}>차단된 링크 (null → 클릭 무시)</a>
        </p>
      </section>
    </div>
  )
}

export default App
