# 10. RESTful API

## REST API란?

REST(Representational State Transfer)는 HTTP 프로토콜을 기반으로 한 API 설계 방식입니다.

### RESTful 규칙

| HTTP 메서드 | 동작 | URL 패턴 |
|------------|------|---------|
| GET | 조회 (Read) | `/api/users` |
| POST | 생성 (Create) | `/api/users` |
| PUT / PATCH | 수정 (Update) | `/api/users/:id` |
| DELETE | 삭제 (Delete) | `/api/users/:id` |

### CRUD 예시

```javascript
const express = require('express');
const router = express.Router();

let items = [];
let nextId = 1;

// CREATE - POST /api/items
router.post('/', (req, res) => {
  const item = { id: nextId++, ...req.body };
  items.push(item);
  res.status(201).json(item);
});

// READ all - GET /api/items
router.get('/', (req, res) => {
  res.json(items);
});

// READ one - GET /api/items/:id
router.get('/:id', (req, res) => {
  const item = items.find(i => i.id === Number(req.params.id));
  if (!item) return res.status(404).json({ error: 'Not found' });
  res.json(item);
});

// UPDATE - PUT /api/items/:id
router.put('/:id', (req, res) => {
  const index = items.findIndex(i => i.id === Number(req.params.id));
  if (index === -1) return res.status(404).json({ error: 'Not found' });
  items[index] = { ...items[index], ...req.body };
  res.json(items[index]);
});

// DELETE - DELETE /api/items/:id
router.delete('/:id', (req, res) => {
  const index = items.findIndex(i => i.id === Number(req.params.id));
  if (index === -1) return res.status(404).json({ error: 'Not found' });
  items.splice(index, 1);
  res.status(204).send();
});
```

## JSON 응답

항상 `res.json()`을 사용하여 JSON 형식으로 응답합니다. 적절한 HTTP 상태 코드를 함께 반환하는 것이 중요합니다.

## Postman으로 테스트

Postman을 사용하면 GUI로 API를 쉽게 테스트할 수 있습니다:
1. `GET http://localhost:3000/api/items`
2. `POST http://localhost:3000/api/items` (Body → raw → JSON)
3. `PUT http://localhost:3000/api/items/1`
4. `DELETE http://localhost:3000/api/items/1`
