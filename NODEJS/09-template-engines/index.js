const express = require('express');
const path = require('path');

const app = express();
const PORT = 3000;

// EJS 설정
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, 'views'));

// 정적 파일
app.use(express.static(path.join(__dirname, 'public')));

// --- 라우트 ---

// 홈
app.get('/', (req, res) => {
  res.render('index', {
    title: 'EJS 템플릿 엔진',
    message: 'Node.js + EJS로 만든 페이지입니다.',
    users: [
      { name: '홍길동', age: 30, email: 'hong@test.com' },
      { name: '김철수', age: 25, email: 'kim@test.com' },
      { name: '이영희', age: 28, email: 'lee@test.com' },
    ],
  });
});

// 사용자 상세
app.get('/users/:name', (req, res) => {
  res.render('user', {
    title: '사용자 프로필',
    name: req.params.name,
  });
});

// about
app.get('/about', (req, res) => {
  res.render('about', {
    title: '소개',
    description: '이것은 EJS 학습을 위한 예제 프로젝트입니다.',
  });
});

app.listen(PORT, () => {
  console.log(`서버: http://localhost:${PORT}`);
});
