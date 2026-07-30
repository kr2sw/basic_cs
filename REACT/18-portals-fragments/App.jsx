import { useState } from 'react'
import { createPortal } from 'react-dom'

function Modal({ open, onClose, children }) {
  if (!open) return null
  return createPortal(
    <div style={{
      position: 'fixed', top: 0, left: 0, width: '100%', height: '100%',
      background: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center'
    }} onClick={onClose}>
      <div style={{
        background: '#fff', padding: 24, borderRadius: 8, minWidth: 300,
        position: 'relative'
      }} onClick={e => e.stopPropagation()}>
        {children}
        <button onClick={onClose} style={{ marginTop: 12 }}>Close</button>
      </div>
    </div>,
    document.getElementById('modal-root')
  )
}

function App() {
  const [open, setOpen] = useState(false)

  return (
    <>
      <h1>Portals & Fragments</h1>
      <p>Using Fragment (no extra DOM node)</p>
      <button onClick={() => setOpen(true)}>Open Modal (Portal)</button>

      <Modal open={open} onClose={() => setOpen(false)}>
        <h2>Modal Title</h2>
        <p>This modal is rendered via createPortal to #modal-root</p>
      </Modal>
    </>
  )
}

export default App
