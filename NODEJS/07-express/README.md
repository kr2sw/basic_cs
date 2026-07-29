# 07. Express

Express는 Node.js의 대표적인 웹 프레임워크로, 라우팅, 미들웨어 등을 쉽게 처리합니다.

## 설치

```bash
npm init -y
npm install express
```

## 기본 서버

```javascript
const express = require('express');
const app = express();
const port = 3000;

app.get('/', (req, res) => {
  res.send('Hello Express!');
});

app.listen(port, () => {
  console.log(`서버: http://localhost:${port}`);
});
```

## 라우트 (app.get/post/put/delete)

```javascript
app.get('/users', (req, res) => {
  res.json([{ id: 1, name: '홍길동' }]);
});

app.post('/users', (req, res) => {
  res.status(201).json({ message: '생성됨' });
});
```

## req / res 기본

### req 객체
- `req.params`: URL 파라미터 (`/users/:id`)
- `req.query`: 쿼리 스트링 (`?name=john`)
- `req.body`: POST body (미들웨어 필요)
- `req.headers`: 요청 헤더

### res 객체
- `res.send(data)`: 문자열/HTML 응답
- `res.json(data)`: JSON 응답
- `res.status(code)`: 상태 코드 설정
- `res.redirect(path)`: 리다이렉트
- `res.render(view)`: 템플릿 렌더링
