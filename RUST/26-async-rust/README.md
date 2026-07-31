# 26: 비동기 — async/await, Future 개념, 블로킹 폴링 재현

Rust의 비동기는 `async`/`await`와 `Future` 트레잇 기반입니다.

## Future 트레잇

```rust
trait Future {
    type Output;
    fn poll(self: Pin<&mut Self>, cx: &mut Context<'_>) -> Poll<Self::Output>;
}
```

`Poll::Ready` 또는 `Poll::Pending`을 반환합니다. 실제 `tokio` 크레이트를 사용하는 예시를 README에 기록하고, 본 코드는 표준 라이브러리만으로 이벤트 루프를 재현합니다.

## tokio 사용 예 (외부 크레이트)

```rust
// Cargo.toml
// [dependencies]
// tokio = { version = "1", features = ["full"] }

#[tokio::main]
async fn main() {
    let handle = tokio::spawn(async { 1 + 1 });
    println!("{}", handle.await.unwrap());
}
```

## 실행

```bash
cd RUST/26-async-rust
cargo run
```
