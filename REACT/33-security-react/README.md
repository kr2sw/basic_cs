# 33: 보안 — React Security

XSS 방어, `dangerouslySetInnerHTML` 사용법, CSRF 대응을 배웁니다.

## React의 기본 방어: 자동 이스케이프

JSX에서 표현식으로 렌더링하면 React가 문자열을 자동으로 이스케이프합니다. 사용자 입력은 기본적으로 안전합니다.

```jsx
const userInput = '<img src=x onerror=alert(1)>'
;<p>{userInput}</p>   // ❌ 실행 안 됨. 텍스트로만 표시됨
```

## dangerouslySetInnerHTML의 위험

HTML을 그대로 삽입하는 이 속성은 **이스케이프를 끕니다**. 외부 입력을 그대로 넣으면 XSS에 노출됩니다. 반드시 **샌드박스/정화(sanitize)** 과정을 거쳐야 합니다.

```jsx
// ❌ 사용자 입력을 그대로 넣으면 XSS
<div dangerouslySetInnerHTML={{ __html: userInput }} />

// ✅ DOMPurify로 정화 후 삽입
import DOMPurify from 'dompurify'
<div dangerouslySetInnerHTML={{ __html: DOMPurify.sanitize(userInput) }} />
```

## javascript: URL

`<a href="javascript:...">`는 사각지대 공격 경로입니다. 링크를 만들 때 프로토콜을 검증하세요.

```jsx
function safeHref(url) {
  return /^(https?:|\/|#)/.test(url) ? url : null
}
```

## CSRF

CSRF는 로그인된 사용자의 브라우저로 위조 요청을 보내는 공격입니다. 대응법:

- SameSite 쿠키 설정 (`SameSite=Lax/Strict`)
- CSRF 토큰 검증 (폼 요청마다 서버 발급 토큰 확인)
- 요청 헤더 커스텀 값 검증 (`X-Requested-With`)

## 기타 실무 체크

- API 키 등 시크릿을 `import.meta.env.VITE_*`로 노출하지 않기 (프론트엔드에 두면 전부 공개됩니다)
- `jsonwebtoken` 같은 보안 검증은 서버에서만
- 의존성 취약점 점검: `npm audit`

## 실행

```bash
npm install dompurify && npm run dev
```
