import { useState, useRef, useEffect } from 'react'

// 스킵 링크: 키보드 사용자가 바로 본문으로 이동
function SkipLink() {
  return <a href="#main" style={{ position: 'absolute', left: -9999 }}>본문으로 건너뛰기</a>
}

// aria-expanded로 펼침 상태를 알리는 아코디언
function Accordion({ title, children }) {
  const [open, setOpen] = useState(false)
  return (
    <div>
      <button
        aria-expanded={open}
        aria-controls="accordion-panel"
        onClick={() => setOpen(o => !o)}
      >
        {title} {open ? '▴' : '▾'}
      </button>
      {open && (
        <div id="accordion-panel" role="region" aria-label={title}>
          {children}
        </div>
      )}
    </div>
  )
}

// role="switch"를 쓰는 커스텀 토글 (네이티브 checkbox보다 의미가 명확)
function Switch({ label, checked, onChange }) {
  return (
    <button
      role="switch"
      aria-checked={checked}
      aria-label={label}
      onClick={() => onChange(!checked)}
      style={{
        width: 64, height: 32, borderRadius: 16,
        background: checked ? '#2ecc71' : '#bbb',
        position: 'relative', cursor: 'pointer',
      }}
    >
      <span style={{
        position: 'absolute', top: 4, left: checked ? 36 : 4,
        width: 24, height: 24, borderRadius: 12, background: 'white',
        transition: 'left 0.2s',
      }} />
    </button>
  )
}

// role="dialog": 열리면 포커스 이동, Esc로 닫기
function Modal({ open, onClose }) {
  const closeRef = useRef(null)

  useEffect(() => {
    if (open) closeRef.current?.focus()   // 열릴 때 버튼으로 포커스
  }, [open])

  if (!open) return null

  return (
    <div
      role="dialog"
      aria-modal="true"
      aria-labelledby="modal-title"
      style={{ position: 'fixed', inset: 0, background: 'rgba(0,0,0,.5)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}
      onKeyDown={e => { if (e.key === 'Escape') onClose() }}
    >
      <div style={{ background: 'white', padding: 24, borderRadius: 8, width: 300 }}>
        <h2 id="modal-title">접근성 모달</h2>
        <p>초점이 모달 안으로 들어왔습니다. Esc를 누르면 닫힙니다.</p>
        <button ref={closeRef} onClick={onClose}>닫기</button>
      </div>
    </div>
  )
}

function App() {
  const [notifications, setNotifications] = useState(3)
  const [dark, setDark] = useState(false)
  const [modalOpen, setModalOpen] = useState(false)

  return (
    <div>
      <SkipLink />
      <h1>접근성 예제</h1>

      <main id="main">
        <section>
          <h2>aria-label + aria-expanded</h2>
          <button aria-label={`알림 ${notifications}개`} onClick={() => setNotifications(n => n + 1)}>
            🔔
          </button>
          <span aria-live="polite"> 현재 알림 {notifications}개</span>
        </section>

        <section>
          <h2>아코디언</h2>
          <Accordion title="접근성이 왜 중요한가요?">
            키보드만 쓰는 사용자, 스크린리더 사용자, 고령자까지 모두를 위한 디자인입니다.
          </Accordion>
        </section>

        <section>
          <h2>role="switch"</h2>
          <Switch label="다크 모드" checked={dark} onChange={setDark} />
        </section>

        <section>
          <h2>모달</h2>
          <button onClick={() => setModalOpen(true)}>모달 열기</button>
          <Modal open={modalOpen} onClose={() => setModalOpen(false)} />
        </section>
      </main>
    </div>
  )
}

export default App
