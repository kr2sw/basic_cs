# 30: Next.js 기초 — Next.js Basics

Next.js의 렌더링 전략인 CSR, SSR, SSG, ISR의 개념과 차이를 배웁니다.

## Next.js란?

React 위에서 **풀스택 프레임워크**를 제공합니다. 라우팅, 데이터 페칭, SEO, 이미지 최적화가 기본 내장되어 있고 Vercel과 긴밀하게 통합됩니다.

## 렌더링 전략 비교

| 전략 | 렌더링 시점 | 장점 | 단점 |
|------|------------|------|------|
| CSR | 브라우저 | 빠른 인터랙션 | 초기 로딩 느림, SEO 불리 |
| SSR | 요청마다 서버 | 항상 최신 데이터, SEO 좋음 | 요청마다 서버 부하 |
| SSG | 빌드 타임 | 가장 빠름, 캐시 유리 | 데이터 갱신에 재빌드 필요 |
| ISR | 빌드 + 주기적 재생성 | SSG + 자동 재검증 | 갱신 지연 가능 |

## Pages Router 예제 (SSG + ISR)

```jsx
// pages/posts/[id].js — 빌드 시 정적 생성
export async function getStaticProps({ params }) {
  const post = await fetch(`https://api.example.com/posts/${params.id}`).then(r => r.json())
  return {
    props: { post },
    revalidate: 60,   // ISR: 60초마다 백그라운드에서 재생성
  }
}
```

## SSR 예제

```jsx
export async function getServerSideProps() {
  const data = await fetch('https://api.example.com/me').then(r => r.json())
  return { props: { data } }   // 요청마다 서버에서 실행
}
```

## 언제 무엇을?

- 마케팅/블로그 → SSG
- 로그인 상태 기반 개인화 → SSR 또는 클라이언트 페칭
- 개인화 + 동적 데이터 → ISR (revalidate 시간 설정)

## 실행

```bash
# Next.js 프로젝트는 vite와 별개입니다. 예제 App.jsx는 vite로 개념 확인용입니다.
npm install next react react-dom && npm run dev
```
