import { useState, useEffect, useRef } from 'react'

// 이 예제는 서버 없이도 학습할 수 있도록 두 가지 모드를 제공합니다.
// 1) 로컬 데모 모드(기본): 봇이 메시지에 응답하는 시뮬레이터
// 2) 실제 WebSocket 모드: 아래 WS_URL로 연결 (예: wss://example.com/ws)
const WS_URL = ''

function createChatBot(onMessage, onStatus) {
  if (!WS_URL) {
    // ---- 데모 모드: 가짜 상대방 봇 ----
    const replies = ['안녕하세요! 👋', 'React로 채팅 만들고 계신가요?', 'WebSocket은 연결이 열려 있을 때 양방향 통신이 가능합니다.', '실전에서는 Socket.IO를 많이 씁니다.']
    let index = 0
    return {
      connect() { onStatus('데모 모드 (가상 상대)'); onMessage({ from: 'bot', text: '데모 모드입니다. 메시지를 보내 보세요!' }) },
      send(text) {
        onMessage({ from: 'me', text })
        setTimeout(() => {
          const reply = replies[index++ % replies.length]
          onMessage({ from: 'bot', text: reply })
        }, 600)
      },
      close() {},
    }
  }

  // ---- 실제 WebSocket 모드 ----
  let ws
  return {
    connect() {
      ws = new WebSocket(WS_URL)
      ws.onopen = () => onStatus('연결됨')
      ws.onclose = () => onStatus('연결 끊김')
      ws.onmessage = e => {
        try { onMessage({ from: 'server', text: JSON.parse(e.data).text }) }
        catch { onMessage({ from: 'server', text: String(e.data) }) }
      }
    },
    send(text) { ws.send(JSON.stringify({ type: 'message', text })) },
    close() { ws?.close() },
  }
}

function App() {
  const [messages, setMessages] = useState([])
  const [input, setInput] = useState('')
  const [status, setStatus] = useState('연결 대기')
  const chatRef = useRef(null)

  // useEffect cleanup: 언마운트 시 연결 종료 (메모리 누수 방지)
  useEffect(() => {
    const chat = createChatBot(
      msg => setMessages(m => [...m, msg]),
      setStatus
    )
    chatRef.current = chat
    chat.connect()
    return () => chat.close()
  }, [])

  function send(e) {
    e.preventDefault()
    if (!input.trim()) return
    chatRef.current.send(input)
    setInput('')
  }

  return (
    <div style={{ maxWidth: 480, margin: '0 auto' }}>
      <h1>실시간 채팅</h1>
      <p style={{ fontSize: 12, color: 'gray' }}>상태: {status}</p>

      <div style={{ border: '1px solid #ccc', height: 320, overflow: 'auto', padding: 8, background: '#fafafa' }}>
        {messages.map((m, i) => (
          <div key={i} style={{ textAlign: m.from === 'me' ? 'right' : 'left', margin: 4 }}>
            <span style={{ display: 'inline-block', background: m.from === 'me' ? '#cfe8ff' : '#e9e9e9', padding: '4px 8px', borderRadius: 8 }}>
              <strong>{m.from}: </strong>{m.text}
            </span>
          </div>
        ))}
      </div>

      <form onSubmit={send} style={{ display: 'flex', gap: 4, marginTop: 8 }}>
        <input
          value={input}
          onChange={e => setInput(e.target.value)}
          placeholder="메시지 입력"
          style={{ flex: 1 }}
        />
        <button type="submit">전송</button>
      </form>
    </div>
  )
}

export default App
