const express = require('express');
const app = express();
const PORT = 3000;

// GET /
app.get('/', (req, res) => {
  res.send('<h1>Express 서버</h1><p>Node.js + Express</p>');
});

// GET /hello/:name - URL 파라미터
app.get('/hello/:name', (req, res) => {
  const { name } = req.params;
  res.send(`<h1>안녕하세요, ${name}님!</h1>`);
});

// GET /search - 쿼리 스트링
app.get('/search', (req, res) => {
  const { q, page = 1 } = req.query;
  res.json({ query: q, page, results: [`${q} 검색 결과`] });
});

// GET /users - JSON 응답
app.get('/users', (req, res) => {
  const users = [
    { id: 1, name: '홍길동', age: 30 },
    { id: 2, name: '김철수', age: 25 },
    { id: 3, name: '이영희', age: 28 },
  ];
  res.json(users);
});

// POST /users - body 파싱을 위해 express.json() 필요
app.use(express.json());
app.post('/users', (req, res) => {
  const newUser = req.body;
  console.log('새 사용자:', newUser);
  res.status(201).json({ message: '사용자 생성됨', user: newUser });
});

// 404 처리
app.use((req, res) => {
  res.status(404).send('<h1>404 Not Found</h1>');
});

app.listen(PORT, () => {
  console.log(`Express 서버: http://localhost:${PORT}`);
});
