const dotenv = require('dotenv');
const path = require('path');

const env = process.env.NODE_ENV || 'development';
const envFile = path.join(__dirname, `.env.${env}`);

dotenv.config({ path: envFile });
dotenv.config(); // .env 공통 설정 (덮어쓰지 않음)

const config = {
  port: process.env.PORT || 3000,
  dbHost: process.env.DB_HOST || 'localhost',
  dbUser: process.env.DB_USER || 'root',
  dbPassword: process.env.DB_PASSWORD || '',
  jwtSecret: process.env.JWT_SECRET || 'fallback-secret',
  nodeEnv: process.env.NODE_ENV || 'development',
  logLevel: process.env.LOG_LEVEL || 'debug'
};

console.log('=== Environment Configuration ===');
console.log(`NODE_ENV: ${config.nodeEnv}`);
console.log(`Port: ${config.port}`);
console.log(`DB Host: ${config.dbHost}`);
console.log(`DB User: ${config.dbUser}`);
console.log(`DB Password: ${config.dbPassword ? '***' : '(empty)'}`);
console.log(`JWT Secret: ${config.jwtSecret ? '***' : '(empty)'}`);
console.log(`Log Level: ${config.logLevel}`);
console.log('================================');

const express = require('express');
const app = express();

app.get('/config', (req, res) => {
  res.json({
    environment: config.nodeEnv,
    port: config.port,
    logLevel: config.logLevel
  });
});

app.listen(config.port, () => {
  console.log(`Server running in ${config.nodeEnv} mode on port ${config.port}`);
});
