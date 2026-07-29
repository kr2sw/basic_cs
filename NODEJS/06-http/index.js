const http = require('http');

const PORT = 3000;

const server = http.createServer((req, res) => {
  const { url, method } = req;

  console.log(`[${method}] ${url}`);

  // URL 경로에 따른 라우팅
  if (url === '/') {
    res.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8' });
    res.end('<h1>홈페이지</h1><p><a href="/about">소개</a> | <a href="/api">API</a></p>');
  } else if (url === '/about') {
    res.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8' });
    res.end('<h1>소개</h1><p>Node.js HTTP 서버 학습 중입니다.</p>');
  } else if (url === '/api') {
    res.writeHead(200, { 'Content-Type': 'application/json; charset=utf-8' });
    const data = { message: '안녕하세요', timestamp: Date.now() };
    res.end(JSON.stringify(data));
  } else {
    res.writeHead(404, { 'Content-Type': 'text/html; charset=utf-8' });
    res.end('<h1>404 Not Found</h1><p>페이지를 찾을 수 없습니다.</p>');
  }
});

server.listen(PORT, () => {
  console.log(`서버 실행 중: http://localhost:${PORT}`);
});
