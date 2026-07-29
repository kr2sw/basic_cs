# 19 Testing — 테스팅

러스트의 테스트 시스템: `#[test]`, `assert!`, `#[should_panic]`, 모듈 테스트.

## 주요 개념
- `#[test]` 속성 — 테스트 함수 선언
- `assert_eq!`, `assert_ne!`, `assert!` 매크로
- `#[should_panic]` — 패닉 발생 테스트
- `#[ignore]` — 느린 테스트 제외
- 테스트 모듈: `#[cfg(test)]` 조건부 컴파일
- `cargo test` 실행
- `pub` 함수 단위 테스트와 비공개 함수 테스트
- lib.rs와 main.rs 분리 (바이너리에서 라이브러리 함수 사용)

```rust
#[test]
fn add_positive_numbers() {
    assert_eq!(add(2, 3), 5);
}

#[test]
#[should_panic(expected = "0으로 나눌 수 없습니다")]
fn divide_by_zero() {
    divide(1, 0);
}

#[test]
#[ignore = "성능 테스트"]
fn fibonacci_large_slow() {
    assert_eq!(fibonacci(40), 102334155);
}
```

## 실행
```bash
cd RUST/19-testing && cargo test
cargo run
```

## 핵심 요점
- `cargo test`로 모든 테스트 실행, `-- --ignored`로 무시된 테스트 포함
- `#[cfg(test)]`로 테스트 코드가 빌드에 포함되지 않도록 함
- `#[should_panic]`으로 에러 상황 테스트 가능
- 비공개 함수도 같은 모듈 내에서 테스트 가능
