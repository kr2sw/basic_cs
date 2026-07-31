# 24: 제네릭 심화 — const generics

컨스트 제네릭(const generics)은 타입 매개변수에 값을 전달할 수 있게 하는 기능입니다.

## 기본 문법

```rust
struct Array<T, const N: usize> {
    data: [T; N],
}
```

`N`은 컴파일 타임 상수여야 합니다.

## 활용 예

- 고정 길이 배열 타입 안전성
- 수학 라이브러리의 차원 검증
- 템플릿 메타프로그래밍

## 실행

```bash
cd RUST/24-const-generics
cargo run
```
