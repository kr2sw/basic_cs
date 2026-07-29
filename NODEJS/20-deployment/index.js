const express = require('express');
const cluster = require('cluster');
const os = require('os');
const crypto = require('crypto');

const PORT = process.env.PORT || 3000;

if (process.env.USE_CLUSTER === 'true' && cluster.isPrimary) {
  const cpuCount = os.cpus().length;
  console.log(`Primary process (PID: ${process.pid})`);
  console.log(`Forking ${cpuCount} workers...`);

  for (let i = 0; i < cpuCount; i++) {
    cluster.fork();
  }

  cluster.on('exit', (worker, code, signal) => {
    console.log(`Worker ${worker.process.pid} died. Forking new one...`);
    cluster.fork();
  });

  return;
}

const app = express();
app.use(express.json());

app.get('/', (req, res) => {
  res.json({
    message: 'Hello from Node.js deployment demo',
    pid: process.pid,
    platform: process.platform,
    nodeVersion: process.version,
    memoryUsage: process.memoryUsage(),
    uptime: process.uptime()
  });
});

app.get('/health', (req, res) => {
  res.json({ status: 'healthy', pid: process.pid });
});

app.get('/heavy', (req, res) => {
  const hash = crypto.pbkdf2Sync('password', 'salt', 100000, 64, 'sha512');
  res.json({
    result: hash.toString('hex').substring(0, 16) + '...',
    pid: process.pid
  });
});

app.listen(PORT, () => {
  console.log(`Worker ${process.pid} listening on port ${PORT}`);
});
