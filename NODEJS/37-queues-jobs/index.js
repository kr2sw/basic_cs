// 작업 큐: BullMQ 개념을 Node.js 핵심 모듈로 구현한 예제입니다.
// 실제 사용 시: npm install bullmq (Redis 필요)

function delay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

// ---------- 작업 큐 구현 ----------
class Queue {
  constructor(name, options = {}) {
    this.name = name;
    this.waiting = [];      // 대기 중인 작업
    this.active = new Set(); // 실행 중인 작업
    this.completed = [];    // 성공한 작업
    this.failed = [];       // 최종 실패한 작업
    this.nextId = 1;
    this.concurrency = options.concurrency || 1;
    this.running = 0;
    this.pending = 0; // 아직 완료되지 않은 전체 작업 수
    this.processor = null;
  }

  // 작업 추가 (BullMQ: queue.add())
  add(data, options = {}) {
    const job = {
      id: this.nextId++,
      data,
      options,
      attempts: 0,
      maxAttempts: options.attempts || 1,
      status: 'waiting',
      addedAt: new Date().toISOString(),
    };
    this.pending++;

    const enqueue = () => {
      this.waiting.push(job);
      this._pump();
    };

    if (options.delay) {
      console.log(`[${this.name}] #${job.id} 작업 ${options.delay}ms 지연 예약`);
      setTimeout(enqueue, options.delay);
    } else {
      enqueue();
    }
    return job;
  }

  // 작업 처리 함수 등록 (BullMQ: new Worker(name, handler))
  process(handler) {
    this.processor = handler;
  }

  _pump() {
    while (this.running < this.concurrency && this.waiting.length) {
      const job = this.waiting.shift();
      this.running++;
      job.status = 'active';
      this.active.add(job.id);
      this._run(job);
    }
  }

  async _run(job) {
    job.attempts++;
    try {
      const result = await this.processor(job);
      job.status = 'completed';
      job.finishedAt = new Date().toISOString();
      job.result = result;
      this.completed.push(job);
      this.pending--;
      console.log(`[${this.name}] #${job.id} 완료 (${job.data.recipient})`);
    } catch (err) {
      job.error = err.message;
      if (job.attempts < job.maxAttempts) {
        // 실패 -> 재시도 (지수 백오프)
        const backoff = (job.options.backoff || 300) * job.attempts;
        console.log(
          `[${this.name}] #${job.id} 실패 (${job.attempts}/${job.maxAttempts}) -> ${backoff}ms 후 재시도`
        );
        setTimeout(() => {
          job.status = 'waiting';
          this.waiting.push(job);
          this._pump();
        }, backoff);
      } else {
        // 최종 실패 (Dead Letter Queue 개념)
        job.status = 'failed';
        job.finishedAt = new Date().toISOString();
        this.failed.push(job);
        this.pending--;
        console.log(`[${this.name}] #${job.id} 최종 실패: ${err.message}`);
      }
    } finally {
      this.running--;
      this.active.delete(job.id);
      this._pump();
    }
  }

  get isDone() {
    return this.pending === 0 && this.running === 0;
  }
}

// ---------- 이메일 큐 사용 ----------
const emailQueue = new Queue('email', { concurrency: 2 });

// 워커: 메일 발송 작업 처리 (BullMQ Worker 유사)
emailQueue.process(async (job) => {
  // 실패를 유발하는 수신자
  if (job.data.recipient.includes('@fail.com')) {
    throw new Error('메일 전송 실패 (수신자 오류)');
  }
  await delay(150);
  return { sentTo: job.data.recipient };
});

// 정상 작업 5건
for (let i = 1; i <= 5; i++) {
  emailQueue.add(
    { recipient: `user${i}@example.com`, subject: `안내 메일 ${i}` },
    { attempts: 3, backoff: 200 }
  );
}

// 실패 후 재시도 -> 최종 실패하는 작업
emailQueue.add(
  { recipient: 'bad@fail.com', subject: '실패 테스트' },
  { attempts: 2, backoff: 200 }
);

// 1500ms 후 실행되는 지연 작업
emailQueue.add(
  { recipient: 'delay@example.com', subject: '지연 발송' },
  { attempts: 1, delay: 1500 }
);

console.log('작업 큐 시작 (동시성 2)\n');

// 상태 모니터링
const timer = setInterval(() => {
  console.log(
    `[상태] 대기 ${emailQueue.waiting.length} | 실행중 ${emailQueue.running} | 성공 ${emailQueue.completed.length} | 실패 ${emailQueue.failed.length}`
  );
  if (emailQueue.isDone) {
    clearInterval(timer);
    console.log('\n=== 최종 결과 ===');
    console.log('성공:', emailQueue.completed.map((j) => `#${j.id}`).join(', '));
    console.log('실패:', emailQueue.failed.map((j) => `#${j.id} (${j.error})`).join(', '));
    process.exit(0);
  }
}, 500);
