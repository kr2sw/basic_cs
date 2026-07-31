# 25: 라우터 고급 — Advanced React Router

중첩 라우트, 인증 가드(Guard), 그리고 `useNavigate` 기반의 프로그래매틱 내비게이션을 배웁니다.

## 레이아웃 라우트와 중첩 라우트

부모 라우트가 공통 레이아웃(내비게이션)을 담당하고, 자식 라우트는 `<Outlet />` 위치에 렌더링됩니다.

```jsx
<Routes>
  <Route path="/" element={<Layout />}>
    <Route index element={<Home />} />
    <Route path="profile" element={<Profile />} />
    <Route path="settings" element={<Settings />} />
  </Route>
</Routes>
```

```jsx
function Layout() {
  return (
    <div>
      <nav>...</nav>
      <Outlet />   {/* 자식 라우트가 여기에 그려진다 */}
    </div>
  )
}
```

## 인증 가드 (Protected Route)

권한이 없으면 `Navigate`로 로그인 페이지를 리다이렉트하는 래퍼 컴포넌트입니다. 이전 위치를 `useLocation`으로 기억해 로그인 후 복귀시킵니다.

```jsx
function RequireAuth({ children }) {
  const { user } = useAuth()
  const location = useLocation()
  if (!user) return <Navigate to="/login" state={{ from: location }} replace />
  return children
}
```

## useNavigate

버튼 클릭, 폼 제출, 타이머 등 이벤트 코드에서 화면을 이동합니다.

```jsx
const navigate = useNavigate()
navigate('/profile')                 // 이동
navigate(-1)                         // 뒤로 가기
navigate('/login', { state: {...} }) // 상태와 함께 이동
```

## 실행

```bash
npm install react-router-dom && npm run dev
```
