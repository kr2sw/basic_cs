# 37: 고급 테스팅 — 속성 테스트(proptest 개념), 벤치마크

Rust의 테스트는 `#[test]` 속성으로 작성하고 `cargo test`로 실행합니다.

## 기본 테스트

```rust
#[test]
fn test_add() {
    assert_eq!(add(2, 3), 5);
}
```

## 속성 테스트 (proptest 개념)

`proptest` 크레이트가 입력을 자동 생성하지만, 여기서는 수동으로 많은 입력을 생성해 검증합니다.

## 본 챕터 구현

- `#[test]` 단위 테스트
- 테스트 모듈 패턴
- 문서 테스트
- 수동 속성 테스트(랜덤 입력)

## 실행

```bash
cd RUST/37-testing-advanced
cargo test
```
