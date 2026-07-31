// 35: Node + TS 고급 — 워커, 스트림 타입
// (외부 모듈 없이 Node 내장 타입 구조를 모델링합니다)

type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. 스트림 타입 모델링 ===
interface TransformCallback {
  (error: Error | null, data?: unknown): void;
}

interface StreamOptions<T> {
  highWaterMark: number;
  transform?: (chunk: T, encoding: string, callback: TransformCallback) => void;
}

class TransformStream<T, R> {
  private options: StreamOptions<T>;

  constructor(options: StreamOptions<T>) {
    this.options = options;
  }

  push(chunk: T): void {
    if (this.options.transform) {
      this.options.transform(chunk, "utf8", (err, data) => {
        if (err) console.error("스트림 오류:", err.message);
        else if (data !== undefined) console.log("변환 출력:", data as R);
      });
    }
  }

  pipe(destination: WritableStream<R>): WritableStream<R> {
    return destination;
  }
}

class WritableStream<T> {
  write(data: T): void {
    console.log("쓰기:", data);
  }
}

// === 2. 대문자 변환 스트림 (node:stream Transform 유사) ===
const upperStream = new TransformStream<string, string>({
  highWaterMark: 16,
  transform: (chunk, _enc, callback) => callback(null, chunk.toUpperCase()),
});

const collector = new WritableStream<string>();
upperStream.pipe(collector);
upperStream.push("hello");
upperStream.push("typescript");

// === 3. 배압(backpressure) 모델링 ===
class BackpressureStream<T> {
  private buffer: T[] = [];
  private readonly limit: number;

  constructor(limit = 3) {
    this.limit = limit;
  }

  write(data: T): boolean {
    if (this.buffer.length >= this.limit) {
      console.log(`배압! 버퍼 가득 참 (${this.limit}) — 소비 필요`);
      return false;
    }
    this.buffer.push(data);
    return true;
  }

  read(): T | undefined {
    return this.buffer.shift();
  }
}

const bp = new BackpressureStream<number>(2);
bp.write(1);  // true
bp.write(2);  // true
console.log("3번째 쓰기:", bp.write(3) ? "성공" : "배압 발생");
bp.read();
console.log("소비 후 다시 쓰기:", bp.write(3) ? "성공" : "배압 발생");

// === 4. 워커 스레드 메시지 타입 ===
type WorkerMessage =
  | { kind: "task"; payload: { n: number } }
  | { kind: "result"; payload: { value: number; ms: number } }
  | { kind: "error"; payload: { message: string } };

interface WorkerLike {
  postMessage(msg: WorkerMessage): void;
  onMessage(cb: (msg: WorkerMessage) => void): void;
}

class FakeWorker implements WorkerLike {
  postMessage(msg: WorkerMessage): void {
    if (msg.kind === "task") {
      const n = msg.payload.n;
      const result: WorkerMessage = { kind: "result", payload: { value: n * 2, ms: 1 } };
      this.onMessage(result);
    }
  }

  onMessage(_cb: (msg: WorkerMessage) => void): void {
    // 실제 구현에서는 이벤트로 전달
  }
}

function fibonacci(n: number): number {
  return n <= 1 ? n : fibonacci(n - 1) + fibonacci(n - 2);
}

console.log("\nfib(30) =", fibonacci(30));

// === 5. 채널/큐 제네릭 ===
class Channel<T> {
  private queue: T[] = [];

  send(item: T): void {
    this.queue.push(item);
  }

  receive(): T | undefined {
    return this.queue.shift();
  }

  get size(): number {
    return this.queue.length;
  }
}

const jobs = new Channel<{ id: number; script: string }>();
jobs.send({ id: 1, script: "worker.js" });
jobs.send({ id: 2, script: "worker.js" });
console.log("채널 잡 수:", jobs.size);

// === 타입 검증 ===
type T1 = Expect<Equal<ReturnType<typeof fibonacci>, number>>;

console.log("\nNode + TS 고급 데모 완료!");
