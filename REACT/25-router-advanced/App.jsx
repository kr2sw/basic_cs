import { useState, createContext, useContext } from 'react'
import {
  BrowserRouter, Routes, Route, Link, Outlet,
  Navigate, useNavigate, useLocation, useSearchParams,
} from 'react-router-dom'

// 가짜 인증 컨텍스트 (실무에서는 서버 검증이 필요)
const AuthContext = createContext(null)

function AuthProvider({ children }) {
  const [user, setUser] = useState(null)
  return <AuthContext.Provider value={{ user, login: () => setUser({ name: '김리액트' }), logout: () => setUser(null) }}>{children}</AuthContext.Provider>
}

const useAuth = () => useContext(AuthContext)

// 레이아웃 라우트: 자식들이 <Outlet /> 위치에 렌더링된다
function Layout() {
  const { user, logout } = useAuth()
  return (
    <div>
      <nav>
        <Link to="/">홈</Link> |{' '}
        <Link to="/account">내 계정</Link> |{' '}
        <Link to="/account/profile">프로필</Link> |{' '}
        <Link to="/account/settings">설정</Link>
        {user && <button onClick={logout}>로그아웃</button>}
      </nav>
      <hr />
      <Outlet />
    </div>
  )
}

// 인증 가드: 로그인 안 했으면 로그인 페이지로 리다이렉트
function RequireAuth({ children }) {
  const { user } = useAuth()
  const location = useLocation()
  if (!user) return <Navigate to="/login" state={{ from: location }} replace />
  return children
}

function Home() {
  const navigate = useNavigate()
  const [params] = useSearchParams()
  const greeting = params.get('greeting')
  return (
    <div>
      <h2>홈</h2>
      {greeting && <p style={{ color: 'green' }}>인사말: {greeting}</p>}
      <button onClick={() => navigate('/account/profile')}>프로필로 이동 (useNavigate)</button>
    </div>
  )
}

// 중첩 라우트: account 레이아웃 아래 profile/settings가 Outlet에 그려짐
function AccountLayout() {
  return (
    <div>
      <h2>내 계정</h2>
      <Outlet />
    </div>
  )
}

function Profile() {
  const { user } = useAuth()
  return <p>프로필 — {user.name}님 안녕하세요!</p>
}

function Settings() {
  return <p>설정 페이지</p>
}

function Login() {
  const { user, login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const from = location.state?.from?.pathname || '/'  // 원래 가려던 곳 복원

  function handleLogin() {
    login()
    navigate(from, { replace: true })  // 로그인 후 이전 위치로
  }

  if (user) return <Navigate to="/" replace />
  return (
    <div>
      <h2>로그인</h2>
      <button onClick={handleLogin}>로그인하기</button>
      <p>로그인 후 {from}로 이동합니다.</p>
    </div>
  )
}

function NotFound() {
  const navigate = useNavigate()
  return (
    <div>
      <h2>404 — 페이지를 찾을 수 없습니다</h2>
      <button onClick={() => navigate(-1)}>뒤로 가기</button>
    </div>
  )
}

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route index element={<Home />} />
            <Route path="login" element={<Login />} />
            {/* 가드로 보호된 중첩 라우트 그룹 */}
            <Route path="account" element={<RequireAuth><AccountLayout /></RequireAuth>}>
              <Route path="profile" element={<Profile />} />
              <Route path="settings" element={<Settings />} />
            </Route>
            <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App
