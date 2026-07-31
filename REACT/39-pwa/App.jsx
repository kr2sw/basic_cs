import { useState, useEffect } from 'react'

// 온라인/오프라인 상태를 추적하는 훅
function useOnlineStatus() {
  const [online, setOnline] = useState(navigator.onLine)
  useEffect(() => {
    const goOnline = () => setOnline(true)
    const goOffline = () => setOnline(false)
    window.addEventListener('online', goOnline)
    window.addEventListener('offline', goOffline)
    return () => {
      window.removeEventListener('online', goOnline)
      window.removeEventListener('offline', goOffline)
    }
  }, [])
  return online
}

function App() {
  const online = useOnlineStatus()
  const [swState, setSwState] = useState('미등록')
  const [text, setText] = useState('')
  const [saved, setSaved] = useState('')

  // Service Worker 등록: 페이지 로드 후에 실행해 초기 로딩을 방해하지 않는다
  useEffect(() => {
    if (!('serviceWorker' in navigator)) {
      setSwState('지원 안 함')
      return
    }
    navigator.serviceWorker.register('/sw.js')
      .then(() => setSwState('등록됨'))
      .catch(() => setSwState('등록 실패'))
  }, [])

  // localStorage에 저장하면 오프라인에서도 읽을 수 있다 (캐시 전략 시연)
  useEffect(() => {
    setSaved(localStorage.getItem('pwa-draft') || '(저장된 내용 없음)')
  }, [text])

  function save() {
    localStorage.setItem('pwa-draft', text)
    setSaved(text)
    setText('')
  }

  return (
    <div>
      <h1>PWA 데모</h1>

      <p>
        네트워크: <strong>{online ? '온라인 ✅' : '오프라인 ⛔'}</strong>
        <br />
        Service Worker: <strong>{swState}</strong>
      </p>

      {!online && (
        <p style={{ color: 'orange' }}>
          오프라인 상태입니다. 이 화면과 저장된 메모는 캐시/로컬스토리지에서 동작합니다.
        </p>
      )}

      <section>
        <h2>오프라인 메모장</h2>
        <textarea
          value={text}
          onChange={e => setText(e.target.value)}
          rows={3}
          style={{ width: 320 }}
          placeholder="메모를 입력하세요"
        />
        <br />
        <button onClick={save}>저장</button>
        <p>저장된 메모: {saved}</p>
      </section>

      <p style={{ fontSize: 12, color: 'gray' }}>
        DevTools → Application → Service Workers에서 오프라인(Offline)을 켜고 새로고침해 보세요.
      </p>
    </div>
  )
}

export default App
