# 22: 고급 패턴 매칭 — 가드, @바인딩, 구조 분해 심화

패턴 매칭은 값을 구조에 따라 분해하고 분기하는 Rust의 강력한 표현입니다.

## 매치 가드 (Match Guards)

```rust
match n {
    x if x > 10 => "크다",
    x => "작다",
}
```

## @ 바인딩

값을 변수에 바인딩하면서 패턴도 검사합니다.

```rust
match n {
    x @ 1..=5 => println!("{}는 1~5", x),
}
```

## 구조 분해 (Destructuring)

튜플, 구조체, enum, 슬라이스를 분해할 수 있습니다.

## 실행

```bash
cd RUST/22-pattern-matching
cargo run
```
