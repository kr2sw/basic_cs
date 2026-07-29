# 04. 파일 시스템 (fs 모듈)

Node.js의 `fs` 모듈로 파일과 디렉토리를 다룹니다.

## fs 모듈 불러오기

```javascript
const fs = require('fs');
// 또는 프로미스 API
const fsPromises = require('fs/promises');
```

## 파일 읽기 (readFile)

```javascript
// 동기식
const data = fs.readFileSync('file.txt', 'utf-8');

// 콜백식 (비동기)
fs.readFile('file.txt', 'utf-8', (err, data) => {
  if (err) throw err;
  console.log(data);
});

// 프로미스식 (비동기)
const data = await fsPromises.readFile('file.txt', 'utf-8');
```

## 파일 쓰기 (writeFile)

```javascript
fs.writeFileSync('file.txt', 'Hello World');

fs.writeFile('file.txt', 'Hello World', (err) => {
  if (err) throw err;
});

await fsPromises.writeFile('file.txt', 'Hello World');
```

## 디렉토리 읽기 (readdir)

```javascript
const files = fs.readdirSync('.');
const files = await fsPromises.readdir('.');
```

## 파일 존재 확인 (existsSync)

```javascript
if (fs.existsSync('file.txt')) {
  console.log('파일이 존재합니다');
}
```

## 프로미스 API (fs/promises)

Node.js 14+에서 안정화된 프로미스 기반 API로, `async/await`과 함께 사용합니다.

```javascript
import fs from 'fs/promises';

async function main() {
  await fs.writeFile('test.txt', 'Hello');
  const data = await fs.readFile('test.txt', 'utf-8');
  console.log(data);
}
```
