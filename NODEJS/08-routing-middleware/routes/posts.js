const express = require('express');
const router = express.Router();

const posts = [
  { id: 1, title: '첫 글', content: '안녕하세요' },
  { id: 2, title: '두 번째 글', content: 'Node.js 공부 중' },
];

router.get('/', (req, res) => {
  res.json({ posts, requestTime: req.requestTime });
});

router.get('/:id', (req, res) => {
  const post = posts.find((p) => p.id === Number(req.params.id));
  if (!post) return res.status(404).json({ error: '게시글 없음' });
  res.json(post);
});

module.exports = router;
