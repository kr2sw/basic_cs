const Database = require('better-sqlite3');
const path = require('path');

const db = new Database(path.join(__dirname, 'example.db'));

db.exec(`
  CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    age INTEGER
  )
`);

const insert = db.prepare('INSERT INTO users (name, email, age) VALUES (?, ?, ?)');

const { lastInsertRowid } = insert.run('홍길동', 'hong@example.com', 25);
console.log(`Inserted user with ID: ${lastInsertRowid}`);

insert.run('김철수', 'kim@example.com', 30);
insert.run('이영희', 'lee@example.com', 28);

const user = db.prepare('SELECT * FROM users WHERE id = ?').get(1);
console.log('User with id 1:', user);

const youngUsers = db.prepare('SELECT * FROM users WHERE age < ?').all(30);
console.log('Users under 30:', youngUsers);

const allUsers = db.prepare('SELECT * FROM users').all();
console.log('All users:', allUsers);

const deleteAll = db.prepare('DELETE FROM users');
deleteAll.run();
console.log('All users deleted');

db.close();
