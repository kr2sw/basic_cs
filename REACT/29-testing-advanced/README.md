# 29: 고급 테스팅 — Advanced Testing

MSW로 API를 목킹하고, `user-event`로 사용자 행동을 재현하며, e2e 테스트의 개념을 배웁니다.

## MSW — 네트워크 계층 목킹

MSW(Mock Service Worker)는 실제 HTTP 요청을 가로채서 지정한 응답을 돌려줍니다. 테스트마다 서버를 띄울 필요 없이 API 계약대로 테스트합니다.

```js
import { setupServer } from 'msw/node'
import { http, HttpResponse } from 'msw'

const server = setupServer(
  http.get('/api/todos', () => HttpResponse.json([{ id: 1, title: 'MSW 학습', done: false }]))
)
beforeAll(() => server.listen())
afterEach(() => server.resetHandlers())
afterAll(() => server.close())
```

## user-event — 실제 사용자 행동

`fireEvent`보다 `user-event`가 실제 브라우저에 가깝습니다. 입력, 포커스, 키보드 순서까지 반영합니다.

```jsx
import userEvent from '@testing-library/user-event'

const user = userEvent.setup()
await user.click(screen.getByRole('button', { name: /완료/ }))
await user.type(screen.getByPlaceholderText('새 할일'), '테스트 작성')
```

## 비동기 + 인터랙션 테스트

`findBy*` / `waitFor`로 로딩이 끝난 뒤의 UI를 검증합니다.

```jsx
expect(await screen.findByText('MSW 학습')).toBeInTheDocument()
```

## e2e 개념

단위/통합 테스트는 브라우저 밖에서 DOM을 검증합니다. 진짜 브라우저에서 전체 흐름을 검증하는 e2e(Playwright, Cypress)는 배포 직전에 돌립니다. "로그인 → 작성 → 반영" 같은 사용자 시나리오를 코드로 남깁니다.

## 테스트 피라미드

- **단위 테스트**: 훅/함수 단위 (가장 많고 빠름)
- **통합 테스트**: 컴포넌트 + API(MSW) (이 예제)
- **e2e 테스트**: 브라우저 전체 흐름 (가장 적고 느림)

아래 계층일수록 실행이 빠르므로, 가능한 한 아래쪽 계층에 테스트를 모으는 것이 좋습니다.

## 실행

```bash
npm install -D vitest @testing-library/react @testing-library/user-event @testing-library/jest-dom msw && npm run dev
# 테스트만 실행하려면: npx vitest run
```
