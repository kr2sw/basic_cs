// vitest + RTL + MSW + user-event 통합 테스트 예제
import { describe, it, expect, beforeAll, afterEach, afterAll } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import '@testing-library/jest-dom/vitest'
import { setupServer } from 'msw/node'
import { http, HttpResponse } from 'msw'
import App from './App'

// 1. MSW 핸들러: /api/todos 요청을 목킹
const server = setupServer(
  http.get('/api/todos', () => HttpResponse.json([
    { id: 1, title: 'MSW 학습', done: false },
    { id: 2, title: 'user-event 연습', done: true },
  ]))
)

beforeAll(() => server.listen())
afterEach(() => server.resetHandlers())
afterAll(() => server.close())

describe('할일 앱', () => {
  it('서버에서 불러온 할일을 표시한다', async () => {
    render(<App />)
    // 로딩 상태 먼저 확인
    expect(screen.getByRole('status')).toHaveTextContent('불러오는 중')
    // 비동기 결과는 findBy로 대기
    expect(await screen.findByText('MSW 학습')).toBeInTheDocument()
  })

  it('user-event로 체크박스를 토글한다', async () => {
    const user = userEvent.setup()
    render(<App />)
    const checkbox = await screen.findByRole('checkbox', { name: 'MSW 학습 완료 처리' })
    expect(checkbox).not.toBeChecked()
    await user.click(checkbox)
    expect(checkbox).toBeChecked()
  })

  it('새 할일을 추가한다', async () => {
    const user = userEvent.setup()
    render(<App />)
    await screen.findByText('MSW 학습')

    const input = screen.getByPlaceholderText('새 할일')
    await user.type(input, '테스트 작성')
    await user.click(screen.getByRole('button', { name: '추가' }))

    await waitFor(() => expect(screen.getByText('테스트 작성')).toBeInTheDocument())
  })

  it('서버 에러가 발생하면 에러 메시지를 보여준다', async () => {
    // 핸들러를 에러로 덮어쓰기
    server.use(http.get('/api/todos', () => HttpResponse.error()))
    render(<App />)
    expect(await screen.findByText('불러오기 실패')).toBeInTheDocument()
  })
})
