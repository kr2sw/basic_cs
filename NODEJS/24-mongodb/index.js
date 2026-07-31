// MongoDB/Mongoose 개념을 in-memory로 시뮬레이션한 예제입니다.
// 실제 Mongoose는 npm install mongoose 필요하며, 구조는 동일합니다.

const crypto = require('crypto');

// ---------- ObjectId 유사 ID 생성 ----------
function objectId() {
  return crypto.randomBytes(12).toString('hex');
}

// ---------- 필터 매칭 ($gt, $lt, $in, $regex 등 지원) ----------
function compare(value, condition) {
  if (condition && typeof condition === 'object') {
    for (const [op, expected] of Object.entries(condition)) {
      if (op === '$gt' && !(value > expected)) return false;
      if (op === '$gte' && !(value >= expected)) return false;
      if (op === '$lt' && !(value < expected)) return false;
      if (op === '$lte' && !(value <= expected)) return false;
      if (op === '$ne' && value === expected) return false;
      if (op === '$in' && !expected.includes(value)) return false;
      if (op === '$regex' && !new RegExp(expected).test(String(value))) return false;
    }
    return true;
  }
  return value === condition;
}

function matchFilter(doc, filter) {
  return Object.entries(filter).every(([key, cond]) => {
    if (key.includes('.')) {
      const [parent, child] = key.split('.');
      return doc[parent] ? compare(doc[parent][child], cond) : false;
    }
    return compare(doc[key], cond);
  });
}

// ---------- 컬렉션 (MongoDB Collection) ----------
class Collection {
  constructor(name) {
    this.name = name;
    this.docs = [];
  }

  insertOne(doc) {
    const document = { _id: objectId(), ...doc };
    this.docs.push(document);
    return document;
  }

  find(filter = {}) {
    return this.docs.filter((d) => matchFilter(d, filter));
  }

  findOne(filter = {}) {
    return this.find(filter)[0] || null;
  }

  updateOne(filter, update) {
    const doc = this.findOne(filter);
    if (!doc) return { matchedCount: 0, modifiedCount: 0 };
    const { $set = {}, $inc = {} } = update;
    Object.assign(doc, $set);
    for (const [key, delta] of Object.entries($inc)) {
      doc[key] = (doc[key] || 0) + delta;
    }
    return { matchedCount: 1, modifiedCount: 1, doc };
  }

  deleteOne(filter) {
    const idx = this.docs.findIndex((d) => matchFilter(d, filter));
    if (idx === -1) return { deletedCount: 0 };
    this.docs.splice(idx, 1);
    return { deletedCount: 1 };
  }

  countDocuments(filter = {}) {
    return this.find(filter).length;
  }
}

// ---------- 데이터베이스 (MongoDB 서버 유사) ----------
class MongoSimulator {
  constructor() {
    this.collections = new Map();
  }
  collection(name) {
    if (!this.collections.has(name)) {
      this.collections.set(name, new Collection(name));
    }
    return this.collections.get(name);
  }
}

const db = new MongoSimulator();

// ---------- Mongoose Schema/Model 유사 구현 ----------
class Schema {
  constructor(fields) {
    this.fields = fields;
  }

  validate(doc) {
    const errors = [];
    for (const [key, rule] of Object.entries(this.fields)) {
      if (rule.required && doc[key] === undefined) {
        errors.push(`${key} 필드는 필수입니다`);
      }
      if (doc[key] !== undefined && rule.type) {
        const types = Array.isArray(rule.type) ? rule.type : [rule.type];
        if (!types.some((t) => typeof doc[key] === t)) {
          errors.push(`${key} 필드는 ${types.join(' 또는 ')} 타입이어야 합니다`);
        }
      }
      if (doc[key] !== undefined && rule.minLength && String(doc[key]).length < rule.minLength) {
        errors.push(`${key} 필드는 최소 ${rule.minLength}자 이상이어야 합니다`);
      }
      if (doc[key] !== undefined && rule.enum && !rule.enum.includes(doc[key])) {
        errors.push(`${key} 필드는 ${rule.enum.join(', ')} 중 하나여야 합니다`);
      }
    }
    return errors;
  }
}

class Model {
  constructor(name, schema, database) {
    this.name = name;
    this.schema = schema;
    this.collection = database.collection(name);
  }

  create(doc) {
    const errors = this.schema.validate(doc);
    if (errors.length) throw new Error('검증 실패: ' + errors.join(', '));
    return this.collection.insertOne(doc);
  }

  find(filter) {
    return this.collection.find(filter);
  }

  findById(id) {
    return this.collection.findOne({ _id: id });
  }

  findOne(filter) {
    return this.collection.findOne(filter);
  }

  update(filter, update) {
    return this.collection.updateOne(filter, update);
  }

  remove(filter) {
    return this.collection.deleteOne(filter);
  }
}

// ---------- 사용자 모델 정의 (Mongoose와 동일 구조) ----------
const User = new Model(
  'users',
  new Schema({
    name: { type: 'string', required: true, minLength: 2 },
    email: { type: 'string', required: true },
    age: { type: 'number' },
    role: { type: 'string', enum: ['admin', 'editor', 'viewer'] },
  }),
  db
);

const Post = new Model(
  'posts',
  new Schema({
    title: { type: 'string', required: true },
    authorId: { type: 'string', required: true },
  }),
  db
);

console.log('=== Create ===');
const user1 = User.create({ name: '홍길동', email: 'hong@example.com', age: 30, role: 'admin' });
const user2 = User.create({ name: '김철수', email: 'kim@example.com', age: 25, role: 'editor' });
const user3 = User.create({ name: '이영희', email: 'lee@example.com', age: 28, role: 'viewer' });
console.log('user1:', user1);

try {
  User.create({ name: '짧', email: 'x@example.com' }); // 검증 실패 (minLength 2)
} catch (err) {
  console.log('검증 오류:', err.message);
}

console.log('\n=== Read: find({ age: { $gt: 26 } }) ===');
console.log(User.find({ age: { $gt: 26 } }).map((u) => `${u.name}(${u.age}세)`));

console.log('\n=== Read: findById ===');
const found = User.findById(user2._id);
console.log(found);

console.log('\n=== Read: findOne({ name: { $regex: "김" } }) ===');
console.log(User.findOne({ name: { $regex: '김' } }));

console.log('\n=== Update: age +1, role 변경 ===');
console.log(User.update({ _id: user1._id }, { $inc: { age: 1 }, $set: { role: 'editor' } }).doc);

console.log('\n=== Delete ===');
console.log('삭제 결과:', User.remove({ _id: user3._id }));
console.log('남은 사용자 수:', User.collection.countDocuments({}));

console.log('\n=== 관계형 데이터 (참조 개념) ===');
const post = Post.create({ title: 'Node.js 중급 강좌', authorId: user1._id });
const author = User.findById(post.authorId);
console.log(`게시글 "${post.title}"의 작성자: ${author.name}`);
