// 테스트 대상 모듈: 계산기 + 사용자 저장소
// 테스트 파일은 index.test.js 에서 node:test 로 실행합니다.

function add(a, b) {
  return a + b;
}

function divide(a, b) {
  if (b === 0) throw new Error('0으로 나눌 수 없습니다');
  return a / b;
}

function sum(numbers) {
  return numbers.reduce((acc, n) => acc + n, 0);
}

class UserStore {
  constructor() {
    this.users = new Map();
    this.nextId = 1;
  }

  create(name) {
    if (!name || typeof name !== 'string') {
      throw new Error('이름은 필수입니다');
    }
    const user = { id: this.nextId++, name };
    this.users.set(user.id, user);
    return user;
  }

  find(id) {
    return this.users.get(id) || null;
  }

  findAll() {
    return [...this.users.values()];
  }

  remove(id) {
    return this.users.delete(id);
  }
}

async function fetchData(ms = 10, fail = false) {
  await new Promise((resolve) => setTimeout(resolve, ms));
  if (fail) throw new Error('조회 실패');
  return { ok: true, data: [1, 2, 3] };
}

module.exports = { add, divide, sum, UserStore, fetchData };
