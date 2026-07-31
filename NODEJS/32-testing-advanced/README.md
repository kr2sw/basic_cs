# 32: 고급 테스팅 — node:test and Assert Structure

내장 테스트 러너 `node:test`를 사용한 테스트 구조를 학습합니다.

## node:test란?

Node.js에 내장된 테스트 러너입니다. 별도 패키지(Jest 등) 설치 없이 사용할 수 있습니다.

```js
const { describe, it } = require('node:test');
const assert = require('node:assert');

describe('add 함수', () => {
  it('두 숫자를 더한다', () => {
    assert.strictEqual(add(1, 2), 3);
  });
});
```

## assert 메서드

| 메서드 | 의미 |
|--------|------|
| `assert.strictEqual(a, b)` | 엄격 동등 비교 |
| `assert.deepStrictEqual(a, b)` | 객체/배열 깊은 비교 |
| `assert.throws(fn, /regex/)` | 예외 발생 확인 |
| `assert.ok(value)` | 참(true) 확인 |

## 라이프사이클 훅

```js
beforeEach(() => { store = new UserStore(); });
afterEach(() => { /* 정리 */ });
```

## 비동기 테스트

```js
it('비동기 작업', async () => {
  const result = await fetchData();
  assert.strictEqual(result, 'ok');
});
```

## 테스트 실행

```bash
node --test
# 특정 파일만 실행
node --test index.test.js
# 특정 테스트만
node --test --test-name-pattern="UserStore"
```

## 예제 실행

```bash
node --test
```
