# 16 Concurrency — 동시성

스레드 생성, 메시지 전달, 공유 상태, 병렬 반복자 등 동시성 프로그래밍.

## 주요 개념
- `thread::spawn` — 새 스레드 생성, `join`으로 종료 대기
- `move` 클로저로 데이터 소유권 스레드로 이전
- `mpsc` 채널: 여러 생산자-단일 소비자 메시지 전달
- `Mutex<T>`: 상호 배제 — `lock()`으로 데이터 접근
- `Arc<T>`: 원자적 참조 카운팅 (스레드 안전 Rc)
- `Arc<Mutex<T>>`: 스레드 간 공유 상태 패턴
- `Send` / `Sync` 트레이트: 스레드 안전성의 언어 차원 지원
- `rayon` 크레이트: 병렬 반복자로 간편한 병렬 처리

```rust
let handle = thread::spawn(|| { /* ... */ });
handle.join().unwrap();

let (tx, rx) = mpsc::channel::<String>();
thread::spawn(move || { tx.send(msg).unwrap(); });

let counter = Arc::new(Mutex::new(0));
let num = counter.lock().unwrap();
*num += 1;

let sum_parallel: u64 = numbers.par_iter().sum();
```

## 실행
```bash
cd RUST/16-concurrency && cargo run
```

## 핵심 요점
- `mpsc`는 채널 기반 메시지 전달 (여러 송신, 하나의 수신)
- `Arc<Mutex<T>>`로 여러 스레드에서 안전하게 데이터 공유
- `Send`: 스레드 간 소유권 이전 가능, `Sync`: 스레드 간 공유 참조 가능
- `rayon`으로 복잡한 스레드 관리 없이 병렬 처리 가능
