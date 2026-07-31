import { useState } from 'react'
import { createMachine, assign } from 'xstate'
import { useMachine } from '@xstate/react'

// 1. 신호등 FSM: 상태 전이 구조가 명시적으로 정의된다
const trafficMachine = createMachine({
  id: 'traffic',
  initial: 'red',
  states: {
    red:    { on: { NEXT: 'green' } },
    green:  { on: { NEXT: 'yellow' } },
    yellow: { on: { NEXT: 'red' } },
  },
})

const COLOR = { red: '#e74c3c', green: '#2ecc71', yellow: '#f1c40f' }

// 2. 비동기 상태 머신: idle -> loading -> success/failure + context 데이터
const fetchMachine = createMachine({
  id: 'fetch',
  initial: 'idle',
  context: { data: null, error: null },
  states: {
    idle:    { on: { FETCH: 'loading' } },
    loading: { on: { SUCCESS: 'success', FAIL: 'failure' } },
    success: {
      entry: assign({ data: ({ event }) => event.data, error: null }),
      on: { FETCH: 'loading' },
    },
    failure: {
      entry: assign({ error: ({ event }) => event.error, data: null }),
      on: { FETCH: 'loading' },
    },
  },
})

function TrafficLight() {
  const [state, send] = useMachine(trafficMachine)
  const color = COLOR[state.value]

  return (
    <section>
      <h2>신호등 FSM</h2>
      <div style={{ width: 80, height: 80, borderRadius: 40, background: color, margin: '8px auto' }} />
      <p>현재 상태: <code>{state.value}</code></p>
      <button onClick={() => send({ type: 'NEXT' })}>다음 신호 (NEXT)</button>
      <p style={{ fontSize: 12, color: 'gray' }}>
        red → green → yellow → red 순서만 가능합니다.
      </p>
    </section>
  )
}

function AsyncStateMachine() {
  const [state, send] = useMachine(fetchMachine)

  // 마치 서버 요청처럼 비동기로 성공/실패를 흉내 낸다
  function doFetch() {
    send({ type: 'FETCH' })
    setTimeout(() => {
      Math.random() > 0.3
        ? send({ type: 'SUCCESS', data: `요청 결과 ${Date.now()}` })
        : send({ type: 'FAIL', error: '서버 오류 발생' })
    }, 800)
  }

  return (
    <section>
      <h2>비동기 상태 머신</h2>
      <p>상태: <code>{state.value}</code></p>
      {state.matches('success') && <p>✓ {state.context.data}</p>}
      {state.matches('failure') && <p style={{ color: 'red' }}>✗ {state.context.error}</p>}
      <button onClick={doFetch} disabled={state.matches('loading')}>
        {state.matches('loading') ? '요청 중...' : '요청 보내기'}
      </button>
    </section>
  )
}

function App() {
  return (
    <div>
      <h1>XState 상태 머신</h1>
      <TrafficLight />
      <hr />
      <AsyncStateMachine />
    </div>
  )
}

export default App
