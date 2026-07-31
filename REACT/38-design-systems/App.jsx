// 1. 디자인 토큰: 디자인 결정을 한 곳에 모은다
const tokens = {
  colors: { primary: '#3b82f6', danger: '#ef4444', success: '#22c55e', text: '#1f2937', muted: '#6b7280' },
  radius: { sm: 6, md: 10 },
  space: { sm: 8, md: 16 },
}

// 2. 재사용 가능한 Button: variant를 props로 선언
export function Button({ variant = 'primary', disabled = false, children, ...rest }) {
  const styles = {
    primary: { background: tokens.colors.primary, color: '#fff' },
    outline: { background: '#fff', border: `1px solid ${tokens.colors.primary}`, color: tokens.colors.primary },
    danger: { background: tokens.colors.danger, color: '#fff' },
  }
  return (
    <button
      style={{ padding: `${tokens.space.sm}px ${tokens.space.md}px`, borderRadius: tokens.radius.sm, cursor: disabled ? 'not-allowed' : 'pointer', opacity: disabled ? 0.6 : 1, border: 'none', ...styles[variant] }}
      disabled={disabled}
      {...rest}
    >
      {children}
    </button>
  )
}

// 3. 재사용 가능한 Badge
export function Badge({ tone = 'primary', children }) {
  const tones = {
    primary: { background: '#dbeafe', color: '#1d4ed8' },
    success: { background: '#dcfce7', color: '#15803d' },
    danger: { background: '#fee2e2', color: '#b91c1c' },
  }
  return (
    <span style={{ padding: '2px 8px', borderRadius: 999, fontSize: 12, ...tones[tone] }}>
      {children}
    </span>
  )
}

// 4. Input
export function TextField({ label, ...rest }) {
  return (
    <label style={{ display: 'block', marginBottom: tokens.space.sm }}>
      <span style={{ display: 'block', fontSize: 13, color: tokens.colors.muted }}>{label}</span>
      <input
        style={{ padding: `${tokens.space.sm}px`, borderRadius: tokens.radius.sm, border: '1px solid #cbd5e1', width: 240 }}
        {...rest}
      />
    </label>
  )
}

// 5. 카탈로그 페이지: 각 컴포넌트의 모든 변형을 한눈에
function App() {
  return (
    <div>
      <h1>디자인 시스템 카탈로그</h1>
      <p style={{ color: tokens.colors.muted }}>
        Storybook이 아니더라도 이런 페이지 하나면 컴포넌트 상태를 검토할 수 있습니다.
      </p>

      <section>
        <h2>Button</h2>
        <Button>저장</Button>{' '}
        <Button variant="outline">취소</Button>{' '}
        <Button variant="danger">삭제</Button>{' '}
        <Button disabled>비활성</Button>
      </section>

      <section>
        <h2>Badge</h2>
        <Badge>기본</Badge>{' '}
        <Badge tone="success">완료</Badge>{' '}
        <Badge tone="danger">오류</Badge>
      </section>

      <section>
        <h2>TextField</h2>
        <TextField label="이메일" type="email" placeholder="you@example.com" />
        <TextField label="비밀번호" type="password" />
      </section>
    </div>
  )
}

export default App
