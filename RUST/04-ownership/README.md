# 04 Ownership — 소유권

러스트의 가장 핵심 개념인 소유권 시스템: 이동, 복제, 대여, 참조, 슬라이스.

## 주요 개념
- 소유권 규칙: 각 값은 하나의 소유자, 소유자 범위 벗어나면 drop
- 이동(Move): 힙 데이터는 소유권이 이동 (복사 아님)
- `Clone` 트레이트: 깊은 복사 (힙까지 복제)
- `Copy` 트레이트: 스택 전용 타입 자동 복사
- 참조(Reference): 불변 `&T` / 가변 `&mut T` 대여(Borrowing)
- 대여 규칙: 하나의 가변 참조 또는 여러 불변 참조
- 슬라이스(Slice): 컬렉션 일부에 대한 참조 (`&s[0..5]`)

```rust
fn main() {
    let s1 = String::from("hello");
    let s2 = s1;  // 이동: s1은 더 이상 사용 불가

    let len = calculate_length(&s1);  // 불변 참조로 대여

    let hello = &s12[0..5];  // 슬라이스
}
```

## 실행
```bash
cd RUST/04-ownership && cargo run
```

## 핵심 요점
- 러스트는 GC 없이 소유권 시스템으로 메모리 안전성 보장
- 힙 데이터는 이동 semantics, 스택 데이터는 Copy
- 참조는 항상 유효한 데이터만 가리킴 (댕글링 방지)
- 슬라이스는 소유권 없는 뷰(View)
