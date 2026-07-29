const express = require('express');

const app = express();
app.use(express.json());

const users = [
  { id: 1, name: '홍길동' },
  { id: 2, name: '김철수' },
];

function add(a, b) {
  return a + b;
}

function getUser(id) {
  return users.find(u => u.id === id) || null;
}

app.get('/api/users', (req, res) => {
  res.json(users);
});

app.get('/api/users/:id', (req, res) => {
  const user = getUser(Number(req.params.id));
  if (!user) return res.status(404).json({ error: 'User not found' });
  res.json(user);
});

app.post('/api/users', (req, res) => {
  const { name } = req.body;
  if (!name) return res.status(400).json({ error: 'Name is required' });
  const newUser = { id: users.length + 1, name };
  users.push(newUser);
  res.status(201).json(newUser);
});

module.exports = { app, add, getUser };
