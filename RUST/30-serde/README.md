# 30: 직렬화 — serde 개념, 자체 직렬화 구현

`serde`는 Rust에서 가장 널리 쓰이는 직렬화 프레임워크입니다. `derive` 매크로로 구조체를 자동 직렬화합니다.

## serde 개념 (외부 크레이트)

```rust
// Cargo.toml
// serde = { version = "1", features = ["derive"] }
// serde_json = "1"

#[derive(Serialize, Deserialize)]
struct Person { name: String, age: u8 }

let json = serde_json::to_string(&person)?;
let back: Person = serde_json::from_str(&json)?;
```

## 본 챕터 구현

`serde`의 동작 원리를 이해하기 위해 구조체를 JSON 형식의 문자열로 직렬화하는 함수와, 이를 다시 파싱하는 역직렬화를 직접 구현합니다.

## 실행

```bash
cd RUST/30-serde
cargo run
```
