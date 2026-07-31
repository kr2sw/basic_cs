# 35: 성능 최적화 — 벤치마킹, SIMD 개념, 최적화

Rust는 안전성과 성능을 동시에 추구하는 언어입니다.

## 벤치마킹

표준 벤치마크 도구 대신 `Instant::now()`로 측정합니다.

```rust
let start = Instant::now();
// 작업
println!("소요: {:?}", start.elapsed());
```

## 최적화 빌드

```bash
cargo build --release
```

릴리즈 빌드는 `-O` 최적화를 적용합니다.

## SIMD 개념

SSE/AVX 명령어로 여러 데이터를 한 번에 처리합니다. 표준 라이브러리로 개념을 재현합니다.

## 실행

```bash
cd RUST/35-performance
cargo run
```

릴리즈 모드로 비교하려면 `cargo run --release`.
