const express = require('express');
const winston = require('winston');

const logger = winston.createLogger({
  level: 'info',
  format: winston.format.combine(
    winston.format.timestamp(),
    winston.format.json()
  ),
  transports: [
    new winston.transports.File({ filename: 'error.log', level: 'error' }),
    new winston.transports.Console({ format: winston.format.simple() })
  ]
});

class AppError extends Error {
  constructor(message, statusCode) {
    super(message);
    this.statusCode = statusCode;
    this.isOperational = true;
    Error.captureStackTrace(this, this.constructor);
  }
}

class NotFoundError extends AppError {
  constructor(resource = 'Resource') {
    super(`${resource} not found`, 404);
  }
}

class ValidationError extends AppError {
  constructor(message) {
    super(message, 400);
  }
}

const app = express();
app.use(express.json());

app.get('/user/:id', (req, res, next) => {
  const id = Number(req.params.id);
  if (id !== 1) {
    return next(new NotFoundError('User'));
  }
  res.json({ id: 1, name: '홍길동' });
});

app.post('/user', (req, res, next) => {
  if (!req.body.name) {
    return next(new ValidationError('Name is required'));
  }
  res.json({ message: 'User created' });
});

app.get('/crash', () => {
  throw new Error('Intentional crash');
});

app.use((err, req, res, next) => {
  logger.error({
    message: err.message,
    stack: err.stack,
    url: req.originalUrl,
    method: req.method,
    isOperational: err.isOperational || false
  });

  const statusCode = err.statusCode || 500;
  res.status(statusCode).json({
    error: err.isOperational ? err.message : 'Internal server error'
  });
});

process.on('uncaughtException', (err) => {
  logger.error({ message: 'UNCAUGHT EXCEPTION', stack: err.stack });
  process.exit(1);
});

process.on('unhandledRejection', (reason) => {
  logger.error({ message: 'UNHANDLED REJECTION', reason });
  process.exit(1);
});

const server = app.listen(3000, () => {
  logger.info('Server running on http://localhost:3000');
});

process.on('SIGTERM', () => {
  logger.info('SIGTERM received, shutting down');
  server.close(() => process.exit(0));
});
