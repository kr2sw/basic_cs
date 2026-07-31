# 23: 반복자 심화 — 어댑터 체인, Iterator 직접 구현

반복자(Iterator)는 Rust에서 가장 중요한 추상화 중 하나입니다.

## 어댑터 체인

`map`, `filter`, `fold`, `collect` 등을 연결합니다.

```rust
let sum: i32 = (1..=10).filter(|n| n % 2 == 0).map(|n| n * n).sum();
```

## Iterator 직접 구현

```rust
struct Fibonacci {
    a: u64,
    b: u64,
}

impl Iterator for Fibonacci {
    type Item = u64;
    fn next(&mut self) -> Option<Self::Item> {
        let next = self.a + self.b;
        self.a = self.b;
        self.b = next;
        Some(next)
    }
}
```

## 주요 어댑터

`filter_map`, `flat_map`, `enumerate`, `zip`, `take_while`, `skip_while` 등

## 실행

```bash
cd RUST/23-iterators-advanced
cargo run
```
