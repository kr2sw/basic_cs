# 32: 테스팅 — Vitest, Vue Test Utils, e2e 개념

## 설치

```bash
npm install -D vitest @vue/test-utils @vitejs/plugin-vue jsdom
```

## Vitest 설정 (vitest.config.js)

```js
import { defineConfig } from 'vitest/config'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  test: { environment: 'jsdom' }
})
```

## 단위 테스트 (Vue Test Utils)

`mount()`로 컴포넌트를 실제로 렌더링하고 상호작용을 검증합니다.

```js
import { mount } from '@vue/test-utils'
import Counter from '../components/Counter.vue'

it('버튼 클릭 시 카운트가 증가한다', async () => {
  const wrapper = mount(Counter)
  await wrapper.get('button').trigger('click')
  expect(wrapper.text()).toContain('1')
})
```

## 실행

```bash
npx vitest
```

## 테스트 계층

| 계층 | 도구 | 검증 대상 |
|------|------|----------|
| 단위(Unit) | Vitest + Vue Test Utils | 개별 컴포넌트 로직 |
| 통합(Integration) | Vue Test Utils + Pinia/Router | 컴포넌트 간 상호작용 |
| E2E | Playwright / Cypress | 실제 브라우저 사용자 흐름 |

## E2E 개념

E2E는 실제 브라우저에서 전체 흐름을 검증합니다.

```js
// Playwright 예제
await page.goto('http://localhost:5173/')
await page.click('button')
await expect(page.locator('p')).toContainText('1')
```

## 실행

```bash
npm install && npx vite serve .
```
