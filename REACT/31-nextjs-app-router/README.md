# 31: Next.js App Router — Server & Client Components

App Router에서 **서버 컴포넌트(RSC)** 와 **클라이언트 컴포넌트**를 구분하는 법을 배웁니다.

## App Router 구조

`app/` 폴더의 파일 이름이 라우트를 결정합니다.

```
app/
  layout.tsx        # 모든 페이지의 공통 레이아웃
  page.tsx          # /
  about/page.tsx    # /about
  products/[id]/page.tsx  # /products/1 (동적 경로)
```

## 서버 컴포넌트 (기본값)

App Router의 컴포넌트는 **기본이 서버 컴포넌트**입니다. 서버에서 렌더링되어 데이터 접근과 키가 노출되지 않는 로직을 안전하게 처리할 수 있습니다.

```tsx
// app/page.tsx — 서버 컴포넌트 (async 사용 가능)
export default async function Page() {
  const res = await fetch('https://api.example.com/posts')  // 서버에서 직접 fetch
  const posts = await res.json()
  return <ul>{posts.map(p => <li key={p.id}>{p.title}</li>)}</ul>
}
```

## 클라이언트 컴포넌트

상태, 이벤트, 훅을 쓰려면 파일 상단에 `"use client"` 지시어를 씁니다.

```tsx
"use client"
import { useState } from 'react'

export default function Counter() {
  const [count, setCount] = useState(0)
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>
}
```

## 언제 무엇을?

- 상태/이벤트/브라우저 API 사용 → 클라이언트 컴포넌트
- 데이터 읽기, 비밀 로직, 큰 의존성 → 서버 컴포넌트 (번들에 포함 안 됨)
- 둘을 조합: 서버 컴포넌트가 클라이언트 컴포넌트를 props로 품을 수 있습니다.

## 실행

```bash
# App Router는 Next.js 프로젝트 전용입니다. 개념 확인용 예제는 vite로 실행합니다.
npm install next react react-dom && npm run dev
```
