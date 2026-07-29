# 15. 에러 처리 (Error Handling)

Node.js/Express 애플리케이션에서 체계적으로 에러를 처리하는 방법을 학습합니다.

## 커스텀 에러 클래스

표준 Error를 확장하여 상태 코드 등의 정보를 추가합니다.

```js
class AppError extends Error {
  constructor(message, statusCode) {
    super(message);
    this.statusCode = statusCode;
    this.isOperational = true;
  }
}
```

## Express 에러 미들웨어

4개의 인자 `(err, req, res, next)`를 가진 미들웨어는 에러 처리 전용입니다.

```js
app.use((err, req, res, next) => {
  const status = err.statusCode || 500;
  res.status(status).json({ error: err.message });
});
```

## 처리되지 않은 예외/거부

```js
process.on('uncaughtException', (err) => {
  console.error('Uncaught Exception:', err);
  process.exit(1);
});

process.on('unhandledRejection', (reason) => {
  console.error('Unhandled Rejection:', reason);
  process.exit(1);
});
```

## Winston 로깅

```js
const winston = require('winston');
const logger = winston.createLogger({
  level: 'info',
  format: winston.format.json(),
  transports: [
    new winston.transports.File({ filename: 'error.log', level: 'error' }),
    new winston.transports.Console({ format: winston.format.simple() })
  ]
});
```

## 예제 실행

```bash
node index.js
```
