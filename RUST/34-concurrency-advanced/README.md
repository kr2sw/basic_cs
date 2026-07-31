# 34: 고급 동시성 — Arc, mpsc channel, AtomicU64

Rust는 스레드 간 안전한 공유를 타입 시스템으로 보장합니다.

## Arc (원자적 참조 카운팅)

```rust
let data = Arc::new(vec![1, 2, 3]);
let t = { let d = Arc::clone(&data); thread::spawn(move || d[0]) };
```

## mpsc 채널

```rust
let (tx, rx) = mpsc::channel();
tx.send(42)?;
let v = rx.recv()?;
```

## Atomic 타입

```rust
static COUNTER: AtomicU64 = AtomicU64::new(0);
COUNTER.fetch_add(1, Ordering::SeqCst);
```

## 실행

```bash
cd RUST/34-concurrency-advanced
cargo run
```
