// 공유 메모리를 사용하는 워커 스크립트
let exports = null;
let iterations = 0;

self.addEventListener('message', async (e) => {
  const data = e.data;

  if (data.action === 'init') {
    const { module, memory, iterations: it } = data;
    iterations = it;
    // 메인 스레드에서 받은 같은 Memory 객체로 인스턴스 생성
    const instance = new WebAssembly.Instance(module, { env: { memory } });
    exports = instance.exports;
    self.postMessage({ type: 'ready' });

  } else if (data.action === 'run') {
    const fn = data.atomic ? exports.increment : exports.incrementNaive;
    const start = performance.now();
    for (let i = 0; i < iterations; i++) {
      fn(0);                     // 주소 0의 카운터 증가
    }
    const elapsed = performance.now() - start;
    self.postMessage({ type: 'done', elapsed });

  } else if (data.action === 'wait') {
    // 메인 스레드가 notify할 때까지 주소 4에서 대기 (값 0이면 대기)
    const r = exports.waitOn(4, 0);
    self.postMessage({ type: 'woke', r });
  }
});
