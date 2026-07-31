# 21: 고급 트레잇 — 연관 타입, 제네릭 트레잇, 상속 트레잇

트레잇(Trait)은 Rust의 다형성을 담당하는 핵심 기능입니다.

## 연관 타입 (Associated Types)

```rust
trait Iterator {
    type Item;
    fn next(&mut self) -> Option<Self::Item>;
}
```

`type Item`은 구현마다 다르게 지정됩니다.

## 제네릭 트레잇

```rust
trait Convert<T> {
    fn convert(&self) -> T;
}
```

## 상속 트레잇 (Supertrait)

```rust
trait Display {}
trait Debug: Display {}  // Debug는 Display를 요구
```

## 실행

```bash
cd RUST/21-traits-advanced
cargo run
```
