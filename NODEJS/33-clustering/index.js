// cluster(프로세스 분산) + worker_threads(스레드) 활용 예제
// 실행: node index.js

const cluster = require('cluster');
const os = require('os');
const http = require('http');

const PORT = 3000;

if (cluster.isPrimary) {
  // ---------- Primary: 워커 프로세스 생성 ----------
  const numWorkers = Math.min(2, os.cpus().length);
  let readyCount = 0;

  console.log(`[Primary] PID ${process.pid} | 워커 ${numWorkers}개 생성 시작`);

  for (let i = 0; i < numWorkers; i++) {
    cluster.fork();
  }

  cluster.on('online', (worker) => {
    console.log(`[Primary] 워커 ${worker.process.pid} 온라인`);
  });

  // 워커가 준비되면 로드 밸런싱 테스트 시작
  cluster.on('message', (worker, msg) => {
    if (msg === 'ready') {
      readyCount += 1;
      if (readyCount === numWorkers) runLoadBalanceTest();
    }
  });

  // 워커가 죽으면 자동 재시작
  cluster.on('exit', (worker, code) => {
    console.log(`[Primary] 워커 ${worker.process.pid} 종료 (code ${code}), 재시작합니다`);
    cluster.fork();
  });

  // 로드 밸런싱 테스트: 연속 요청이 여러 워커에 분산되는지 확인
  function runLoadBalanceTest() {
    console.log('\n[Primary] 로드 밸런싱 테스트: 6회 연속 요청');
    let done = 0;
    const pids = new Set();
    for (let i = 0; i < 6; i++) {
      http
        .get(`http://127.0.0.1:${PORT}/`, (res) => {
          let body = '';
          res.on('data', (c) => (body += c));
          res.on('end', () => {
            const parsed = JSON.parse(body);
            pids.add(parsed.pid);
            console.log(`  요청 ${i + 1} -> 처리 워커 PID ${parsed.pid}`);
            done += 1;
            if (done === 6) {
              console.log(`  [결과] ${pids.size}개의 워커에 분산됨 (로드 밸런싱 동작)`);
              runWorkerThreadsDemo();
            }
          });
        })
        .on('error', (err) => {
          console.log('요청 실패:', err.message);
        });
    }
  }
} else {
  // ---------- 워커 프로세스: HTTP 서버 실행 ----------
  const server = http.createServer((req, res) => {
    const pid = process.pid;
    // CPU 작업 시뮬레이션 (0~5ms)
    const busyUntil = Date.now() + Math.floor(Math.random() * 5);
    while (Date.now() < busyUntil) {}

    res.writeHead(200, { 'Content-Type': 'application/json; charset=utf-8' });
    res.end(JSON.stringify({
      message: '클러스터 요청 처리 완료',
      pid,
      workerId: cluster.worker.id,
      at: new Date().toISOString(),
    }));
  });

  server.listen(PORT, () => {
    console.log(`[Worker ${cluster.worker.id}] PID ${process.pid} 서버 시작 (포트 ${PORT})`);
    process.send('ready'); // Primary에 준비 알림
  });
}

// ---------- worker_threads 데모 (Primary에서 실행) ----------
const { Worker } = require('worker_threads');

function runWorkerThreadsDemo() {
  console.log('\n[Primary] worker_threads 데모: CPU 집약 작업(피보나치)을 별도 스레드에서 처리');

  // 메인 스레드와 병렬로 실행되는 타이머
  const timer = setInterval(() => {
    console.log(`  [메인 스레드 생존] 이벤트 루프가 막히지 않았습니다 (${Date.now() % 100000})`);
  }, 100);
  setTimeout(() => clearInterval(timer), 600);

  const worker = new Worker('./compute-worker.js', { workerData: { n: 38 } });

  worker.on('message', (msg) => {
    console.log(`\n  [결과 수신] fibonacci(38) = ${msg.result} (${msg.timeMs}ms)`);
    console.log('  (동일 계산을 메인 스레드에서 하면 이벤트 루프가 멈춥니다)');
  });

  worker.on('error', (err) => console.error('[워커 스레드 오류]', err));

  worker.on('exit', () => {
    console.log('\n[Primary] 데모 완료. 서버는 실행 중입니다. 종료하려면 Ctrl+C');
  });
}
