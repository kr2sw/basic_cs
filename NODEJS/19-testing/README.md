# 19. 테스팅 (Testing)

Jest와 Supertest를 사용한 Node.js 애플리케이션 테스트를 학습합니다.

## Jest

가장 널리 사용되는 JavaScript 테스트 프레임워크입니다.

### 설치

```bash
npm install --save-dev jest supertest
```

## 기본 테스트 구조

```js
// math.js
function add(a, b) { return a + b; }
module.exports = { add };

// math.test.js
const { add } = require('./math');
describe('add function', () => {
  it('adds two numbers', () => {
    expect(add(1, 2)).toBe(3);
  });
});
```

## describe / it / expect

- `describe`: 테스트 그룹화
- `it` (또는 `test`): 개별 테스트 케이스
- `expect`: 값 검증 (matcher 사용)

## Supertest로 HTTP 테스트

```js
const request = require('supertest');
const app = require('./app');

it('GET / returns 200', async () => {
  const res = await request(app).get('/');
  expect(res.status).toBe(200);
});
```

## 예제 실행

```bash
npm test
```
