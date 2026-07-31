// GraphQL 스키마/리졸버 개념을 미니 구현한 예제입니다.
// 실제 사용 시: npm install graphql @apollo/server

// ---------- 1. 미니 GraphQL 엔진 ----------
function tokenize(queryString) {
  // { } ( ) : , 를 기준으로 토큰 분리
  return queryString
    .replace(/[{}():,]/g, ' $& ')
    .trim()
    .split(/\s+/)
    .filter(Boolean);
}

function parseField(tokens, state) {
  const field = { name: tokens[state.i++] };

  // 인자 파싱: field(arg: value)
  if (tokens[state.i] === '(') {
    state.i++;
    field.args = {};
    while (tokens[state.i] !== ')') {
      const key = tokens[state.i++];
      state.i++; // ':' 건너뜀
      let value = tokens[state.i++];
      // 숫자는 Number로 변환, 따옴표 제거
      field.args[key] = Number.isNaN(Number(value)) ? value.replace(/['"]/g, '') : Number(value);
    }
    state.i++; // ')' 건너뜀
  }

  // 하위 필드 파싱: field { sub1 sub2 }
  if (tokens[state.i] === '{') {
    state.i++;
    field.children = [];
    while (tokens[state.i] !== '}') {
      field.children.push(parseField(tokens, state));
    }
    state.i++; // '}' 건너뜀
  }
  return field;
}

function parseQuery(queryString) {
  const tokens = tokenize(queryString);
  const state = { i: 0 };
  const root = {};
  if (tokens[state.i] === '{') state.i++;
  while (state.i < tokens.length && tokens[state.i] !== '}') {
    const field = parseField(tokens, state);
    root[field.name] = field;
  }
  return root;
}

class MiniGraphQL {
  constructor({ query }) {
    this.query = query; // { fieldName: { resolve(args) } }
  }

  execute(queryString) {
    const ast = parseQuery(queryString);
    const result = {};
    for (const [name, node] of Object.entries(ast)) {
      const fieldDef = this.query[name];
      if (!fieldDef) throw new Error(`알 수 없는 쿼리 필드: ${name}`);
      const data = fieldDef.resolve(node.args || {});
      result[name] = this._shape(data, node.children);
    }
    return result;
  }

  // 하위 필드만 추려내기 (선택된 필드만 응답)
  _shape(data, children) {
    if (!children) return data;
    if (data === null || data === undefined) return null;
    if (Array.isArray(data)) return data.map((item) => this._shape(item, children));
    const out = {};
    for (const child of children) {
      if (!(child.name in data)) {
        throw new Error(`알 수 없는 필드: ${child.name}`);
      }
      out[child.name] = this._shape(data[child.name], child.children);
    }
    return out;
  }
}

// ---------- 2. 데이터 ----------
const users = [
  {
    id: 1,
    name: '홍길동',
    email: 'hong@example.com',
    posts: [
      { id: 10, title: 'Node.js 입문', views: 1200 },
      { id: 11, title: '스트림 파헤치기', views: 800 },
    ],
  },
  {
    id: 2,
    name: '김철수',
    email: 'kim@example.com',
    posts: [
      { id: 12, title: '데이터베이스 설계', views: 450 },
    ],
  },
];

// ---------- 3. 스키마 + 리졸버 정의 ----------
const schema = new MiniGraphQL({
  query: {
    user: {
      // 리졸버: 인자를 받아 데이터 반환
      resolve: ({ id }) => users.find((u) => u.id === id),
    },
    users: {
      resolve: () => users,
    },
    postCount: {
      resolve: ({ userId }) => users.find((u) => u.id === userId)?.posts.length ?? 0,
    },
  },
});

// ---------- 4. 쿼리 실행 ----------
console.log('=== 쿼리 1: 선택 필드만 조회 (REST의 오버페칭 해결) ===');
const q1 = `{
  user(id: 1) {
    name
    email
  }
}`;
console.log(JSON.stringify(schema.execute(q1), null, 2));

console.log('\n=== 쿼리 2: 중첩 객체 조회 ===');
const q2 = `{
  user(id: 2) {
    name
    posts {
      title
      views
    }
  }
}`;
console.log(JSON.stringify(schema.execute(q2), null, 2));

console.log('\n=== 쿼리 3: 리스트 + 커스텀 필드 ===');
const q3 = `{
  users {
    id
    name
  }
  postCount(userId: 1)
}`;
console.log(JSON.stringify(schema.execute(q3), null, 2));

console.log('\n=== 쿼리 4: 존재하지 않는 필드 -> 오류 ===');
try {
  schema.execute('{ user(id: 1) { password } }');
} catch (err) {
  console.log('오류 발생:', err.message);
}
