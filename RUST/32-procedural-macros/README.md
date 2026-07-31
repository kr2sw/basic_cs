# 32: 프로시저 매크로 — derive 매크로 개념

프로시저 매크로(procedural macro)는 컴파일 타임에 코드를 생성하는 Rust의 메타프로그래밍 도구입니다.

## 종류

- **derive 매크로**: `#[derive(Debug)]`처럼 구조체에 구현을 자동 추가
- **속성 매크로**: `#[route("/")]`
- **함수형 매크로**: `vec![1, 2]`

## derive 매크로 개념

```rust
#[derive(Serialize)]
struct Person { ... }
```

위 코드는 컴파일러가 `Serialize` 트레잇의 구현 코드를 자동 생성합니다.

## 본 챕터 구현

derive 매크로가 하는 일(반복 코드 자동 생성)을 `macro_rules!`와 제네릭 구현으로 재현합니다.

## 실행

```bash
cd RUST/32-procedural-macros
cargo run
```
