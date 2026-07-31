# 21: Express 고급 — Router Modularization, Middleware Patterns, Error Middleware

Express를 대규모 애플리케이션 구조로 확장하는 방법을 학습합니다.

## 라우터 모듈화

`express.Router()`로 기능별 라우터를 분리하고 app에 마운트합니다.

```js
// routes/users.js
const router = require('express').Router();

router.get('/', (req, res) => {
  res.json([{ id: 1, name: '홍길동' }]);
});

module.exports = router;
```

```js
// app.js
const usersRouter = require('./routes/users');
app.use('/api/users', usersRouter);
```

라우터를 파일로 분리하면 기능별로 코드를 독립적으로 관리할 수 있습니다.

## 미들웨어 패턴

공통 기능을 미들웨어로 분리하면 재사용할 수 있습니다.

```js
app.use((req, res, next) => {
  console.log(`${req.method} ${req.url}`);
  next();
});

function authenticate(req, res, next) {
  const token = req.headers.authorization;
  if (!token) return res.status(401).json({ error: '인증 필요' });
  req.user = { id: 1, name: '홍길동' };
  next();
}
```

## 오류 미들웨어

인자가 4개인 `(err, req, res, next)` 미들웨어는 오류 처리 전용입니다. 반드시 라우트 등록 뒤에 둡니다.

```js
app.use((err, req, res, next) => {
  const status = err.statusCode || 500;
  res.status(status).json({
    error: err.isOperational ? err.message : '서버 오류',
  });
});
```

## async 핸들러 래퍼

async 함수에서 발생한 오류를 자동으로 next에 전달하는 래퍼입니다.

```js
const wrap = (fn) => (req, res, next) =>
  fn(req, res, next).catch(next);

app.get('/users/:id', wrap(async (req, res) => {
  const user = await findUser(req.params.id);
  res.json(user);
}));
```

## 예제 실행

예제는 Node.js 핵심 `http` 모듈로 Express 구조를 유사 구현합니다.

```bash
node index.js
```

브라우저 또는 curl로 테스트합니다.

```bash
curl http://localhost:3000/api/users
curl -X POST -H "Content-Type: application/json" \
  -d '{"title":"새 글"}' http://localhost:3000/api/posts
curl http://localhost:3000/boom
```
