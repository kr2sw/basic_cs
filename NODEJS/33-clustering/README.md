# 33: 성능 — Cluster, Worker Threads, Load Balancing

멀티코어 CPU를 활용하는 방법을 학습합니다.

## 싱글 스레드의 한계

Node.js는 싱글 스레드 이벤트 루프로 동작하지만, CPU 코어가 여러 개여도 하나만 사용합니다. CPU 집약 작업은 이벤트 루프를 막아 전체 성능이 저하됩니다.

## cluster 모듈

프로세스를 복제하여 여러 CPU 코어를 사용하고 들어오는 요청을 분산(로드 밸런싱)합니다.

```js
const cluster = require('cluster');
const os = require('os');

if (cluster.isPrimary) {
  for (let i = 0; i < os.cpus().length; i++) {
    cluster.fork();
  }
  cluster.on('exit', (worker) => cluster.fork()); // 장애 시 재시작
} else {
  // 워커 프로세스: HTTP 서버 실행
  http.createServer((req, res) => res.end('ok')).listen(3000);
}
```

## worker_threads 모듈

같은 프로세스 안에서 별도 스레드를 만들어 CPU 집약 작업을 처리합니다. 메인 스레드는 다른 요청을 계속 처리할 수 있습니다.

```js
const { Worker } = require('worker_threads');
const worker = new Worker('./compute.js', { workerData: { n: 40 } });
worker.on('message', (result) => console.log(result));
```

## 언제 무엇을 쓸까?

| 상황 | 선택 |
|------|------|
| I/O 중심 웹 서버 확장 | cluster (프로세스) |
| CPU 집약 계산 (암호화, 이미지 처리) | worker_threads (스레드) |
| 무거운 모듈 분리 | worker_threads |

## 예제 실행

```bash
node index.js
```

클러스터 워커들이 요청을 분산 처리하고, worker_threads가 피보나치 계산을 수행합니다.
