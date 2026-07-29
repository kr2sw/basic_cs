const express = require('express');
const router = express.Router();

const users = [
  { id: 1, name: '홍길동' },
  { id: 2, name: '김철수' },
];

// GET /api/users
router.get('/', (req, res) => {
  res.json({ users, requestTime: req.requestTime });
});

// GET /api/users/:id
router.get('/:id', (req, res) => {
  const user = users.find((u) => u.id === Number(req.params.id));
  if (!user) return res.status(404).json({ error: '사용자 없음' });
  res.json(user);
});

// POST /api/users
router.post('/', (req, res) => {
  const newUser = {
    id: users.length + 1,
    name: req.body.name,
  };
  users.push(newUser);
  res.status(201).json(newUser);
});

module.exports = router;
