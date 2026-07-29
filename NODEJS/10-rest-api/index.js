const express = require('express');
const morgan = require('morgan');

const app = express();
const PORT = 3000;

app.use(morgan('dev'));
app.use(express.json());

// --- In-memory 데이터 저장 ---
let items = [
  { id: 1, title: 'Node.js 공부', completed: false },
  { id: 2, title: 'REST API 만들기', completed: true },
];
let nextId = 3;

// --- 라우트 핸들러 ---

// GET /api/items - 전체 조회
app.get('/api/items', (req, res) => {
  res.json(items);
});

// GET /api/items/:id - 단일 조회
app.get('/api/items/:id', (req, res) => {
  const item = items.find((i) => i.id === Number(req.params.id));
  if (!item) return res.status(404).json({ error: '항목을 찾을 수 없습니다' });
  res.json(item);
});

// POST /api/items - 생성
app.post('/api/items', (req, res) => {
  const { title, completed = false } = req.body;
  if (!title) return res.status(400).json({ error: 'title은 필수입니다' });

  const newItem = { id: nextId++, title, completed };
  items.push(newItem);
  res.status(201).json(newItem);
});

// PUT /api/items/:id - 전체 수정
app.put('/api/items/:id', (req, res) => {
  const index = items.findIndex((i) => i.id === Number(req.params.id));
  if (index === -1) return res.status(404).json({ error: '항목을 찾을 수 없습니다' });

  const { title, completed } = req.body;
  items[index] = { id: items[index].id, title, completed };
  res.json(items[index]);
});

// PATCH /api/items/:id - 일부 수정
app.patch('/api/items/:id', (req, res) => {
  const index = items.findIndex((i) => i.id === Number(req.params.id));
  if (index === -1) return res.status(404).json({ error: '항목을 찾을 수 없습니다' });

  items[index] = { ...items[index], ...req.body };
  res.json(items[index]);
});

// DELETE /api/items/:id - 삭제
app.delete('/api/items/:id', (req, res) => {
  const index = items.findIndex((i) => i.id === Number(req.params.id));
  if (index === -1) return res.status(404).json({ error: '항목을 찾을 수 없습니다' });

  items.splice(index, 1);
  res.status(204).send();
});

// 홈
app.get('/', (req, res) => {
  res.send(`
    <h1>REST API 서버</h1>
    <p>엔드포인트: <code>/api/items</code></p>
    <p>Postman이나 curl로 테스트해보세요.</p>
    <pre>
GET    /api/items      - 전체 목록
GET    /api/items/:id  - 단일 조회
POST   /api/items      - 생성 (body: { title })
PUT    /api/items/:id  - 전체 수정
PATCH  /api/items/:id  - 일부 수정
DELETE /api/items/:id  - 삭제
    </pre>
  `);
});

app.listen(PORT, () => {
  console.log(`REST API 서버: http://localhost:${PORT}`);
  console.log(`엔드포인트: http://localhost:${PORT}/api/items`);
});
