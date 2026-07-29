# 11. 데이터베이스 (SQLite)

Node.js에서 SQLite를 사용하여 데이터를 저장하고 조회하는 방법을 학습합니다.

## better-sqlite3

`better-sqlite3`는 동기 방식으로 작동하는 SQLite3 라이브러리입니다. 간단하고 빠르며 콜백 지옥이 없습니다.

### 설치

```bash
npm install better-sqlite3
```

### 데이터베이스 연결 및 테이블 생성

```js
const Database = require('better-sqlite3');
const db = new Database('example.db');

db.exec(`
  CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    age INTEGER
  )
`);
```

### INSERT - 데이터 삽입

```js
const stmt = db.prepare('INSERT INTO users (name, email, age) VALUES (?, ?, ?)');
const result = stmt.run('홍길동', 'hong@example.com', 25);
console.log(result.lastInsertRowid); // 새로 삽입된 ID
```

### SELECT - 데이터 조회

```js
const row = db.prepare('SELECT * FROM users WHERE id = ?').get(1);
const allRows = db.prepare('SELECT * FROM users').all();
```

### 파라미터화된 쿼리

파라미터화된 쿼리는 SQL 인젝션을 방지합니다.

```js
const stmt = db.prepare('SELECT * FROM users WHERE age > ? AND name = ?');
const rows = stmt.all(20, '홍길동');
```

### 트랜잭션

```js
const insert = db.prepare('INSERT INTO users (name, email, age) VALUES (?, ?, ?)');
const transaction = db.transaction((users) => {
  for (const user of users) {
    insert.run(user.name, user.email, user.age);
  }
});

transaction([
  { name: '김철수', email: 'kim@test.com', age: 30 },
  { name: '이영희', email: 'lee@test.com', age: 28 },
]);
```

## 예제 실행

```bash
node index.js
```
