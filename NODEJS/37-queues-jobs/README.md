# 37: 작업 큐 — BullMQ Concepts and Core-Based Queue

백그라운드 작업을 처리하는 작업 큐의 개념을 학습합니다.

## 작업 큐가 필요한 이유

메일 발송, 이미지 리사이징, 리포트 생성 같은 무거운 작업을 요청 처리와 분리하면 응답 속도가 빨라집니다.

## BullMQ 개념

```js
const { Queue, Worker } = require('bullmq');

// 큐: 작업이 쌓이는 곳
const emailQueue = new Queue('email', { connection: redisConnection });

// 작업 추가
await emailQueue.add('send', { to: 'user@example.com', subject: '안내' });

// 워커: 큐에서 작업을 꺼내 처리
new Worker('email', async (job) => {
  console.log(job.data);
}, { connection: redisConnection });
```

## 핵심 기능

| 기능 | 설명 |
|------|------|
| **재시도 (Retry)** | 실패한 작업을 `attempts` 만큼 재시도 |
| **지연 (Delay)** | 지정된 시간 후에 실행 (`delay`) |
| **동시성 (Concurrency)** | 동시에 처리할 작업 수 제한 |
| **Backoff** | 실패 후 대기 시간 증가 (지수 백오프) |
| **Dead Letter** | 끝내 실패한 작업 보관 |

## 예제 실행

예제는 BullMQ 설치 없이 Node 핵심 모듈로 작업 큐를 구현합니다.

```bash
node index.js
```
