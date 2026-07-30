# 16: Rust → WASM (wasm-pack)

Rust는 WebAssembly의 일급 언어로, wasm-pack과 wasm-bindgen을 통해 쉽게 WASM 모듈을 만들 수 있습니다.

## 필수 도구

```bash
# Rust 설치
curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh

# wasm-pack 설치
cargo install wasm-pack

# wasm32-unknown-unknown 타겟 추가
rustup target add wasm32-unknown-unknown
```

## 프로젝트 생성

```bash
cargo new wasm-game --lib
```

## Cargo.toml 설정

```toml
[package]
name = "wasm-game"
version = "0.1.0"
edition = "2021"

[lib]
crate-type = ["cdylib", "rlib"]

[dependencies]
wasm-bindgen = "0.2"
```

## 빌드

```bash
wasm-pack build --target web
```

## 실행

```bash
npx http-server .
```
