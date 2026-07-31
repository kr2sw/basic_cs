# 25: 에러 처리 패턴 — 커스텀 Error, Result 체이닝, context

Rust의 에러 처리는 `Result<T, E>`와 `Option<T>`로 이루어집니다.

## 커스텀 Error 타입

```rust
#[derive(Debug)]
enum AppError {
    NotFound(String),
    ParseError(String),
}

impl fmt::Display for AppError { ... }
impl std::error::Error for AppError {}
```

## Result 체이닝

```rust
fn load() -> Result<Data, AppError> {
    let s = read_file()?;           // ? 연산자
    let n = s.parse().map_err(...)?;
    Ok(n)
}
```

## context 패턴

`anyhow` 크레이트의 `context` 개념을 직접 구현해 봅니다.

## 실행

```bash
cd RUST/25-error-patterns
cargo run
```
