# 33: Rust + WASM — wasm-bindgen 개념

Rust를 WebAssembly로 컴파일해 브라우저에서 실행할 수 있습니다.

## wasm-bindgen 개념 (외부 크레이트)

```rust
// Cargo.toml
// wasm-bindgen = "0.2"

use wasm_bindgen::prelude::*;

#[wasm_bindgen]
pub fn add(a: i32, b: i32) -> i32 {
    a + b
}
```

`#[wasm_bindgen]`으로 JS와 상호작용하는 바인딩이 자동 생성됩니다.

## 빌드 (외부 도구)

```bash
rustup target add wasm32-unknown-unknown
cargo build --target wasm32-unknown-unknown
wasm-bindgen target/wasm32-unknown-unknown/release/xxx.wasm --out-dir pkg
```

## 본 챕터 구현

브라우저에서 실행할 수 있는 로직을 순수 Rust로 작성하고, 실제 브라우저에서 돌아가는 것은 확인하지 않습니다.

## 실행

```bash
cd RUST/33-rust-wasm
cargo run
```
