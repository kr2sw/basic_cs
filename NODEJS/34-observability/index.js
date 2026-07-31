// 관찰 가능성: 구조화된 로깅, 요청 추적(requestId), 성능 메트릭 예제

const http = require('http');
const crypto = require('crypto');

// ---------- 1. 구조화된 로거 (JSON) ----------
const LEVELS = { debug: 0, info: 1, warn: 2, error: 3 };

class Logger {
  constructor(level = 'info') {
    this.level = level;
  }

  log(level, message, meta = {}) {
    if (LEVELS[level] < LEVELS[this.level]) return;
    const entry = {
      level,
      message,
      timestamp: new Date().toISOString(),
      pid: process.pid,
      ...meta,
    };
    const line = JSON.stringify(entry);
    if (level === 'error' || level === 'warn') console.error(line);
    else console.log(line);
  }

  debug(message, meta) { this.log('debug', message, meta); }
  info(message, meta) { this.log('info', message, meta); }
  warn(message, meta) { this.log('warn', message, meta); }
  error(message, meta) { this.log('error', message, meta); }
}

const logger = new Logger('debug');

// ---------- 2. 메트릭 수집 ----------
class Metrics {
  constructor() {
    this.counters = new Map();   // name -> count
    this.histograms = new Map(); // name -> [값들]
  }

  increment(name) {
    this.counters.set(name, (this.counters.get(name) || 0) + 1);
  }

  observe(name, value) {
    if (!this.histograms.has(name)) this.histograms.set(name, []);
    this.histograms.get(name).push(value);
  }

  snapshot() {
    const out = {};
    for (const [k, v] of this.counters) out[`counter_${k}`] = v;
    for (const [k, values] of this.histograms) {
      const sum = values.reduce((a, b) => a + b, 0);
      const avg = sum / values.length;
      const max = Math.max(...values);
      out[`hist_${k}_count`] = values.length;
      out[`hist_${k}_avg_ms`] = Math.round(avg * 10) / 10;
      out[`hist_${k}_max_ms`] = max;
    }
    return out;
  }
}

const metrics = new Metrics();

// ---------- 3. 추적 + 로깅 + 메트릭이 적용된 서버 ----------
function json(res, status, body) {
  res.writeHead(status, { 'Content-Type': 'application/json; charset=utf-8' });
  res.end(JSON.stringify(body, null, 2));
}

const server = http.createServer((req, res) => {
  const start = Date.now();
  const url = new URL(req.url, 'http://localhost');

  // 요청 추적 ID 부여
  const requestId = req.headers['x-request-id'] || crypto.randomUUID();
  res.setHeader('X-Request-Id', requestId);

  logger.info('요청 시작', { requestId, method: req.method, path: url.pathname });
  metrics.increment('requests_total');

  const finish = (status) => {
    const durationMs = Date.now() - start;
    metrics.observe('http_request_duration', durationMs);
    metrics.increment(`http_status_${status}`);
    if (status >= 500) metrics.increment('errors_total');
    logger.info('요청 종료', { requestId, status, durationMs });
  };

  if (url.pathname === '/metrics') {
    const snapshot = metrics.snapshot();
    finish(200);
    return json(res, 200, { metrics: snapshot, timestamp: new Date().toISOString() });
  }

  if (url.pathname === '/') {
    finish(200);
    return json(res, 200, { message: '관찰 가능성 데모', requestId });
  }

  if (url.pathname === '/slow') {
    setTimeout(() => {
      finish(200);
      json(res, 200, { message: '느린 응답 완료', durationMs: Date.now() - start });
    }, 300);
    return;
  }

  if (url.pathname === '/error') {
    const err = new Error('의도적인 서버 오류');
    logger.error('처리되지 않은 오류', { requestId, stack: err.stack });
    finish(500);
    return json(res, 500, { error: '서버 오류', requestId });
  }

  finish(404);
  json(res, 404, { error: 'Not Found', requestId });
});

// ---------- 4. 시작 ----------
const PORT = 3000;
server.listen(PORT, () => {
  logger.info('서버 시작', { port: PORT });
  console.log('\n테스트 명령:');
  console.log('  curl http://localhost:3000/');
  console.log('  curl http://localhost:3000/slow');
  console.log('  curl http://localhost:3000/error');
  console.log('  curl http://localhost:3000/metrics');
});
