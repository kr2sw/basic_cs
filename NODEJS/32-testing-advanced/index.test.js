// node:test 기반 테스트 예제
// 실행: node --test

const { describe, it, beforeEach } = require('node:test');
const assert = require('node:assert');

const { add, divide, sum, UserStore, fetchData } = require('./index');

describe('add 함수', () => {
  it('두 숫자를 더한다', () => {
    assert.strictEqual(add(1, 2), 3);
  });

  it('음수도 처리한다', () => {
    assert.strictEqual(add(-1, -5), -6);
  });

  it('0을 더하면 변화가 없다', () => {
    assert.strictEqual(add(10, 0), 10);
  });
});

describe('divide 함수', () => {
  it('정상 나눗셈', () => {
    assert.strictEqual(divide(10, 2), 5);
  });

  it('0으로 나누면 예외가 발생한다', () => {
    assert.throws(() => divide(1, 0), /0으로 나눌 수 없습니다/);
  });
});

describe('sum 함수', () => {
  it('배열의 합을 구한다', () => {
    assert.strictEqual(sum([1, 2, 3, 4]), 10);
  });

  it('빈 배열은 0', () => {
    assert.strictEqual(sum([]), 0);
  });
});

describe('UserStore', () => {
  let store;

  beforeEach(() => {
    store = new UserStore(); // 테스트마다 초기화
  });

  it('사용자를 생성하고 조회한다', () => {
    const user = store.create('홍길동');
    assert.deepStrictEqual(store.find(user.id), { id: 1, name: '홍길동' });
  });

  it('id는 자동 증가한다', () => {
    store.create('홍길동');
    const second = store.create('김철수');
    assert.strictEqual(second.id, 2);
  });

  it('빈 이름은 오류를 던진다', () => {
    assert.throws(() => store.create(''), /이름은 필수/);
  });

  it('존재하지 않는 id는 null', () => {
    assert.strictEqual(store.find(99), null);
  });

  it('사용자를 삭제한다', () => {
    const user = store.create('이영희');
    assert.strictEqual(store.remove(user.id), true);
    assert.strictEqual(store.findAll().length, 0);
  });
});

describe('비동기 함수', () => {
  it('비동기 데이터를 가져온다 (async/await)', async () => {
    const result = await fetchData();
    assert.strictEqual(result.ok, true);
    assert.deepStrictEqual(result.data, [1, 2, 3]);
  });

  it('실패 시 예외를 던진다', async () => {
    await assert.rejects(() => fetchData(5, true), /조회 실패/);
  });
});
