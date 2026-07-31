// worker_threads 워커: CPU 집약 작업(피보나치) 수행
const { parentPort, workerData } = require('worker_threads');

function fibonacci(n) {
  return n <= 1 ? n : fibonacci(n - 1) + fibonacci(n - 2);
}

const start = Date.now();
const result = fibonacci(workerData.n);
const timeMs = Date.now() - start;

// 결과를 메인 스레드로 전송
parentPort.postMessage({ result, timeMs });
