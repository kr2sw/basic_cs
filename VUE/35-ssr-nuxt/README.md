# 35: SSR과 Nuxt 기초 — SSR 개념, hydration, Nuxt 시작

## SSR (Server-Side Rendering)

SSR은 서버에서 Vue 컴포넌트를 렌더링해 완성된 HTML을 클라이언트에 보냅니다.

- 초기 로딩 속도 개선 (첫 화면 즉시 표시)
- SEO에 유리 (크롤러가 HTML을 바로 읽음)

## Hydration (하이드레이션)

서버가 보낸 HTML 위에서 클라이언트가 다시 반응성을 붙이는 과정입니다.
DOM을 새로 만들지 않고 기존 HTML에 이벤트/상태를 연결합니다.

```
서버: 컴포넌트 → HTML 문자열
클라이언트: HTML 표시 → hydrate → 반응성 활성화
```

### hydration mismatch 주의

서버/클라이언트 렌더링 결과가 다르면 경고가 발생합니다.

```js
// ❌ 서버 시각과 클라이언트 시각이 달라 불일치 발생 가능
// const time = new Date().toLocaleTimeString()

// ✅ onMounted 이후에만 계산해 클라이언트에서 채움
const time = ref('')
onMounted(() => { time.value = new Date().toLocaleTimeString() })
```

## Nuxt 시작

```bash
npx nuxi init my-app
cd my-app && npm install && npm run dev
```

Nuxt는 파일 기반 라우팅(`pages/` 폴더), 서버 라우트(`server/` 폴더),
자동 import 등을 제공합니다.

### nuxt.config.ts

```ts
export default defineNuxtConfig({
  ssr: true, // SSR 활성화
  devtools: { enabled: true }
})
```

## 클라이언트 전용 코드 주의

SSR에서는 `window`, `document` 같은 브라우저 전역이 서버에서 존재하지 않습니다.
반드시 `onMounted` 안에서 접근해야 합니다.

```js
// ❌ setup 최상위에서 접근하면 서버에서 에러
// const w = window.innerWidth

// ✅ 마운트 이후에만 접근
onMounted(() => {
  console.log(window.innerWidth)
})
```

## 렌더링 방식 비교

| 방식 | 설명 | 장점 | 단점 |
|------|------|------|------|
| SPA | 클라이언트에서 전부 렌더링 | 인터랙션 빠름 | 초기 로딩/SEO 불리 |
| SSR | 서버에서 HTML 생성 | 초기 화면/SEO 유리 | 서버 비용 필요 |
| SSG | 빌드 시 HTML 생성 | 가장 빠름, 정적 호스팅 | 동적 데이터 어려움 |

Nuxt는 `ssr`, `ssg`(nitro), `csr`을 라우트 단위로 혼합할 수 있습니다.

## 실행

```bash
npm install && npx vite serve .
```
