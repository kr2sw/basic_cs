const express = require('express');
const morgan = require('morgan');
const cors = require('cors');
const path = require('path');

const app = express();
const PORT = 3000;

// --- 미들웨어 ---
app.use(morgan('dev'));     // 요청 로깅
app.use(cors());            // CORS 허용
app.use(express.json());    // JSON body 파싱
app.use(express.urlencoded({ extended: true })); // form data 파싱

// 정적 파일 제공 (public 폴더)
app.use(express.static(path.join(__dirname, 'public')));

// --- 커스텀 미들웨어 (요청 시간 기록) ---
app.use((req, res, next) => {
  req.requestTime = new Date().toISOString();
  next();
});

// --- 라우터 모듈 사용 ---
const usersRouter = require('./routes/users');
const postsRouter = require('./routes/posts');

app.use('/api/users', usersRouter);
app.use('/api/posts', postsRouter);

// --- 홈 ---
app.get('/', (req, res) => {
  res.send(`
    <h1>라우팅 & 미들웨어 예제</h1>
    <ul>
      <li><a href="/api/users">GET /api/users</a></li>
      <li><a href="/api/posts">GET /api/posts</a></li>
      <li><a href="/style.css">정적 파일 (style.css)</a></li>
    </ul>
  `);
});

// 404 처리
app.use((req, res) => {
  res.status(404).json({ error: 'Not Found' });
});

// 에러 처리 미들웨어
app.use((err, req, res, next) => {
  console.error('에러:', err);
  res.status(500).json({ error: '서버 에러' });
});

app.listen(PORT, () => {
  console.log(`서버: http://localhost:${PORT}`);
});
