# 06. HTTP 서버 만들기

Node.js 내장 `http` 모듈로 웹 서버를 만들 수 있습니다.

## 기본 서버

```javascript
const http = require('http');

const server = http.createServer((req, res) => {
  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.end('Hello World');
});

server.listen(3000, () => {
  console.log('서버가 http://localhost:3000 에서 실행 중입니다');
});
```

## request(req) 객체

- `req.url`: 요청 URL (예: `/users?id=1`)
- `req.method`: HTTP 메서드 (GET, POST 등)
- `req.headers`: 요청 헤더 객체
- `req.on('data', callback)`: POST body 데이터 수신

## response(res) 객체

- `res.statusCode`: 상태 코드 설정
- `res.setHeader(name, value)`: 헤더 설정
- `res.write(chunk)`: 응답 데이터 청크 쓰기
- `res.end([data])`: 응답 종료

## URL 기반 라우팅

```javascript
const server = http.createServer((req, res) => {
  if (req.url === '/') {
    res.end('홈페이지');
  } else if (req.url === '/about') {
    res.end('소개 페이지');
  } else {
    res.statusCode = 404;
    res.end('Not Found');
  }
});
```
