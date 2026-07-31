// Docker 데모 앱: 컨테이너 내부에서 실행될 Node.js 서버
// - 컨테이너 ID(hostname), 환경변수, 헬스체크 엔드포인트 제공

const http = require('http');
const os = require('os');

const PORT = Number(process.env.PORT || 3000);
const NODE_ENV = process.env.NODE_ENV || 'development';

const server = http.createServer((req, res) => {
  const url = new URL(req.url, `http://localhost:${PORT}`);

  // 컨테이너 헬스체크용 엔드포인트
  if (url.pathname === '/health') {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ status: 'ok', uptime: process.uptime() }));
    return;
  }

  res.writeHead(200, { 'Content-Type': 'application/json; charset=utf-8' });
  res.end(
    JSON.stringify(
      {
        app: 'docker-demo',
        message: 'Docker 컨테이너에서 실행 중입니다',
        containerId: os.hostname(), // Docker 컨테이너 ID
        node: process.version,
        env: NODE_ENV,
        port: PORT,
        uptimeSec: Math.round(process.uptime()),
      },
      null,
      2
    )
  );
});

server.listen(PORT, () => {
  console.log(`[docker-demo] 서버 시작: http://localhost:${PORT}`);
  console.log(`[docker-demo] NODE_ENV=${NODE_ENV}`);
  console.log(`[docker-demo] containerId=${os.hostname()}`);
  console.log(`[docker-demo] health: http://localhost:${PORT}/health`);

  if (NODE_ENV === 'production') {
    console.log('[docker-demo] 프로덕션 모드로 실행됨');
  }
});
