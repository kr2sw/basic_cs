import { useState } from 'react'
import { motion, AnimatePresence } from 'framer-motion'

let nextId = 4

const listVariants = {
  hidden: { opacity: 0 },
  show: {
    opacity: 1,
    transition: { staggerChildren: 0.08 }, // 아이템마다 0.08초씩 지연
  },
}

const itemVariants = {
  hidden: { opacity: 0, y: 12 },
  show: { opacity: 1, y: 0 },
}

function App() {
  const [visible, setVisible] = useState(true)
  const [items, setItems] = useState([
    { id: 1, text: '애니메이션 1' },
    { id: 2, text: '애니메이션 2' },
    { id: 3, text: '애니메이션 3' },
  ])

  function addItem() {
    setItems(list => [...list, { id: nextId++, text: `애니메이션 ${nextId}` }])
  }

  function removeItem(id) {
    setItems(list => list.filter(i => i.id !== id))
  }

  return (
    <div>
      <h1>애니메이션</h1>

      <section>
        <h2>진입/퇴장 (AnimatePresence)</h2>
        <button onClick={() => setVisible(v => !v)}>{visible ? '숨기기' : '보이기'}</button>
        {/* AnimatePresence가 exit 애니메이션을 처리한다 */}
        <AnimatePresence>
          {visible && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, x: -100 }}
              transition={{ duration: 0.3 }}
              style={{ marginTop: 8, padding: 12, background: '#eef' }}
            >
              페이드 인 + 슬라이드
            </motion.div>
          )}
        </AnimatePresence>
      </section>

      <section>
        <h2>리스트 스테거 (variants)</h2>
        <button onClick={addItem}>아이템 추가</button>
        {/* layout prop으로 재정렬/삭제 시 레이아웃 애니메이션 자동 적용 */}
        <motion.ul
          variants={listVariants}
          initial="hidden"
          animate="show"
          layout
        >
          <AnimatePresence>
            {items.map(item => (
              <motion.li
                key={item.id}
                variants={itemVariants}
                exit={{ opacity: 0, x: -80 }}
                layout
                whileHover={{ scale: 1.03 }}
                whileTap={{ scale: 0.97 }}
              >
                {item.text}{' '}
                <button onClick={() => removeItem(item.id)}>삭제</button>
              </motion.li>
            ))}
          </AnimatePresence>
        </motion.ul>
      </section>
    </div>
  )
}

export default App
