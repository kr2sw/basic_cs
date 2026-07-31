// Express 고급: 라우터 모듈화, 미들웨어 패턴, 오류 미들웨어를
// Node.js 핵심 http 모듈로 유사 구현한 예제입니다.

const http = require('http');

// ---------- 미니 Express 프레임워크 (유사 구현) ----------

class Router {
  constructor() {
    this.layers = [];
  }
  _add(method, path, handlers) {
    this.layers.push({ method, path, handlers, isError: false });
  }
  get(path, ...handlers) { this._add('GET', path, handlers); }
  post(path, ...handlers) { this._add('POST', path, handlers); }
}

class App {
  constructor() {
    this.layers = [];
  }

  // 미들웨어 등록 (경로 생략 시 모든 경로에 적용)
  use(arg1, arg2) {
    if (arg2 instanceof Router) {
      // 라우터 모듈을 마운트 -> 라우트들을 전개(flatten)하여 등록
      for (const layer of arg2.layers) {
        this.layers.push({
          method: layer.method,
          path: joinPath(arg1, layer.path),
          handlers: layer.handlers,
          isError: false,
        });
      }
      return this;
    }
    this.layers.push({ method: null, path: '/', handlers: [arg1], isError: false });
    return this;
  }

  // 오류 미들웨어: (err, req, res, next) 4개 인자
  useError(handler) {
    this.layers.push({ method: null, path: null, handlers: [handler], isError: true });
    return this;
  }

  get(path, ...handlers) {
    this.layers.push({ method: 'GET', path, handlers, isError: false });
  }
  post(path, ...handlers) {
    this.layers.push({ method: 'POST', path, handlers, isError: false });
  }

  handle(req, res) {
    const method = req.method;
    const pathname = req.url.split('?')[0];
    let layerIndex = 0;
    let error = null;

    // res.json 헬퍼
    res.json = (status, body) => {
      if (typeof status === 'object') {
        body = status;
        status = 200;
      }
      res.writeHead(status, { 'Content-Type': 'application/json; charset=utf-8' });
      res.end(JSON.stringify(body, null, 2));
    };

    const respond = (status, body) => {
      res.writeHead(status, { 'Content-Type': 'application/json; charset=utf-8' });
      res.end(JSON.stringify(body, null, 2));
    };

    const nextLayer = (err) => {
      if (err) error = err;

      // 오류가 있으면 오류 미들웨어만 탐색
      if (error) {
        while (layerIndex < this.layers.length) {
          const layer = this.layers[layerIndex++];
          if (layer.isError) {
            try {
              layer.handlers[0](error, req, res, nextLayer);
            } catch (e) {
              error = e;
              nextLayer();
            }
            return;
          }
        }
        return respond(500, { error: error.message });
      }

      // 정상 미들웨어/라우트 탐색
      while (layerIndex < this.layers.length) {
        const layer = this.layers[layerIndex++];
        if (layer.isError) continue;
        if (!matchLayer(layer, method, pathname, req)) continue;
        runHandlers(layer.handlers, 0, req, res, nextLayer);
        return;
      }
      return respond(404, { error: 'Not Found' });
    };

    nextLayer();
  }

  listen(port, onListen) {
    return http.createServer((req, res) => this.handle(req, res)).listen(port, onListen);
  }
}

function joinPath(base, sub) {
  return (base.replace(/\/$/, '') + '/' + sub.replace(/^\//, '')).replace(/\/+$/, '') || '/';
}

function compilePath(path) {
  const re = path.replace(/:[a-zA-Z]+/g, '([^/]+)');
  const names = [...path.matchAll(/:([a-zA-Z]+)/g)].map((m) => m[1]);
  return { re: new RegExp('^' + re + '$'), names };
}

function matchLayer(layer, method, pathname, req) {
  if (layer.method && layer.method !== method) return false;
  if (layer.path === '/') return true;
  if (!layer.path.includes(':')) {
    if (layer.path === pathname) return true;
    return pathname.startsWith(layer.path + '/');
  }
  const { re, names } = compilePath(layer.path);
  const m = pathname.match(re);
  if (!m) return false;
  req.params = {};
  names.forEach((name, i) => {
    req.params[name] = m[i + 1];
  });
  return true;
}

function runHandlers(handlers, i, req, res, done) {
  if (i >= handlers.length) return done();
  const handler = handlers[i];
  let called = false;
  const next = (err) => {
    if (called) return;
    called = true;
    if (err) return done(err);
    runHandlers(handlers, i + 1, req, res, done);
  };
  try {
    handler(req, res, next);
  } catch (err) {
    done(err);
  }
}

// ---------- 라우터 모듈화: users ----------

const usersRouter = new Router();
const users = [
  { id: 1, name: '홍길동', email: 'hong@example.com' },
  { id: 2, name: '김철수', email: 'kim@example.com' },
];

usersRouter.get('/', (req, res) => {
  res.json(users);
});

usersRouter.get('/:id', (req, res) => {
  const user = users.find((u) => u.id === Number(req.params.id));
  if (!user) {
    return res.json(404, { error: '사용자를 찾을 수 없습니다' });
  }
  res.json(user);
});

// ---------- 라우터 모듈화: posts ----------

const postsRouter = new Router();
const posts = [
  { id: 1, title: '첫 번째 글', author: '홍길동' },
  { id: 2, title: 'Node.js 중급 과정', author: '김철수' },
];

// 미들웨어 체인: 게시글 존재 확인 -> 응답
postsRouter.get(
  '/:id',
  (req, res, next) => {
    const post = posts.find((p) => p.id === Number(req.params.id));
    if (!post) {
      const err = new Error('게시글을 찾을 수 없습니다');
      err.statusCode = 404;
      return next(err); // 오류 미들웨어로 전달
    }
    req.post = post; // 다음 핸들러로 데이터 전달
    next();
  },
  (req, res) => {
    res.json(req.post);
  }
);

postsRouter.post('/', (req, res, next) => {
  try {
    const { title } = JSON.parse(req.body || '{}');
    if (!title) {
      const err = new Error('title 필드는 필수입니다');
      err.statusCode = 400;
      return next(err);
    }
    const post = { id: posts.length + 1, title, author: '익명' };
    posts.push(post);
    res.json(201, post);
  } catch (e) {
    const err = new Error('JSON 형식이 올바르지 않습니다');
    err.statusCode = 400;
    next(err);
  }
});

// ---------- 앱 조립 ----------

const app = new App();

// 전역 미들웨어 1: 요청 로깅
app.use((req, res, next) => {
  console.log(`[${new Date().toISOString()}] ${req.method} ${req.url}`);
  next();
});

// 전역 미들웨어 2: body 파서
app.use((req, res, next) => {
  let data = '';
  req.on('data', (chunk) => (data += chunk));
  req.on('end', () => {
    req.body = data;
    next();
  });
});

// 라우터 마운트 (모듈화된 라우터를 경로에 연결)
app.use('/api/users', usersRouter);
app.use('/api/posts', postsRouter);

// 오류를 유발하는 예제 라우트
app.get('/boom', (req, res, next) => {
  next(new Error('의도적으로 발생시킨 오류입니다'));
});

// 오류 미들웨어 (반드시 가장 마지막에 등록)
app.useError((err, req, res, next) => {
  const status = err.statusCode || 500;
  console.error(`[ERROR] ${status} ${err.message}`);
  res.json(status, {
    error: err.message,
    status,
    timestamp: new Date().toISOString(),
  });
});

// ---------- 서버 시작 ----------

const PORT = 3000;
app.listen(PORT, () => {
  console.log(`미니 Express 서버 시작: http://localhost:${PORT}`);
  console.log('  GET  /api/users        사용자 목록');
  console.log('  GET  /api/users/1      사용자 조회');
  console.log('  GET  /api/posts/1      게시글 조회 (미들웨어 체인)');
  console.log('  POST /api/posts        게시글 생성 (입력 검증)');
  console.log('  GET  /boom             오류 미들웨어 테스트');
});
