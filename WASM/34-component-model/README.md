# 34: 컴포넌트 모델 — wit, 인터페이스, 합성

컴포넌트 모델(Component Model)은 WASM 모듈이 서로를 **언어에 무관하게** 호출할 수 있게 하는 표준입니다. 인터페이스를 `wit` 파일로 정의하고, WASI Preview 2로 시스템 접근을 추상화합니다.

## wit 인터페이스 정의

```wit
package example:add;

world calculator {
  export add: func(a: u32, b: u32) -> u32;
  export sub: func(a: u32, b: u32) -> u32;
  export greet: func(name: string) -> string;
}
```

- `world`: 컴포넌트의 경계(import/export) 묶음
- `func`: 인터페이스 함수 (문자열/리스트 등 고급 타입 지원)

## Rust 게스트 구현

```rust
wit_bindgen::generate!({
    path: "add.wit",
    world: "calculator",
});

struct Calculator;

impl Guest for Calculator {
    fn add(a: u32, b: u32) -> u32 { a + b }
    fn sub(a: u32, b: u32) -> u32 { a - b }
    fn greet(name: String) -> String { format!("Hello, {name}!") }
}

export!(Calculator);
```

## 컴포넌트 빌드

```bash
# 1. WASI 대상을 위한 코어 모듈 빌드
cargo build --target wasm32-wasip1 --release

# 2. wit 메타데이터 임베드
wasm-tools component embed add.wit \
  target/wasm32-wasip1/release/calculator_component.wasm \
  -o embedded.wasm

# 3. 컴포넌트로 승격 (component new)
wasm-tools component new embedded.wasm -o calculator.wasm
```

## 인터페이스 확인

```bash
# 컴포넌트가 선언한 wit 확인
wasm-tools component wit calculator.wasm
```

## 실행

```bash
# Wasmtime (Preview 2 지원)
wasmtime run --invoke greet calculator.wasm "World"

# JS 생태계: jco로 JS로 변환
npx jco transpile calculator.wasm
```

## 합성 (Composition)

여러 컴포넌트를 연결해 하나로 조립할 수 있습니다.

```bash
wasm-tools compose -o composed.wasm \
  --definitions plugin.wasm \
  host.wasm
```

## 실행

```bash
cargo build --target wasm32-wasip1 --release
wasm-tools component embed add.wit target/wasm32-wasip1/release/calculator_component.wasm -o embedded.wasm
wasm-tools component new embedded.wasm -o calculator.wasm
wasmtime run --invoke add calculator.wasm 3 4
```
