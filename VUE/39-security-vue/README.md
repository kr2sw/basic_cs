# 39: 보안 — XSS, v-html 위험, CSP, 인증 가드

## XSS (Cross-Site Scripting)

공격자가 악성 스크립트를 주입해 실행시키는 공격입니다.
Vue는 기본적으로 `{{ }}` 보간에서 HTML을 이스케이프하므로 대부분 안전하지만,
`v-html`을 남용하면 취약해집니다.

## v-html 위험성

```vue
<!-- ❌ 위험: 사용자 입력을 그대로 HTML로 렌더링 -->
<div v-html="userInput"></div>
```

### 안전한 사용법

1. 서버에서 이미 정제된 신뢰 가능한 HTML만 사용
2. 출력 전 이스케이프 처리 (아래 함수 참고)

```js
function escapeHTML(str) {
  return str
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#39;')
}
```

## CSP (Content Security Policy)

브라우저에 허용할 리소스 출처를 지정합니다. XSS의 영향 범위를 크게 줄입니다.

```
# nginx/apache 응답 헤더 예시
Content-Security-Policy: default-src 'self'; script-src 'self'
```

- `'unsafe-inline'` 사용을 금지해 인라인 스크립트 차단
- Vue/Vite는 CSP 환경에 맞춘 빌드 옵션 제공 (`--inline-style` 등)

## 인증 가드 (라우터)

```js
router.beforeEach((to) => {
  const token = localStorage.getItem('token')
  if (to.meta.requiresAuth && !token) {
    return { name: 'login' }
  }
})
```

## 기타 보안 습관

- 토큰을 localStorage 대신 안전한 쿠키(HttpOnly)에 저장
- `@click` 등 이벤트에는 사용자 입력을 그대로 넣지 않기
- 의존성 정기 업데이트, 잠재적 취약점 점검

## 실행

```bash
npm install && npx vite serve .
```
