# 09 Generics — 제네릭

러스트의 제네릭 프로그래밍: 제네릭 함수, 구조체, enum, 트레이트 바운드, const generics.

## 주요 개념
- 제네릭 함수: `<T: PartialOrd>` — 타입에 관계없는 함수
- 제네릭 구조체: `Point<T>`, `Point2<T, U>` — 여러 타입 저장
- 제네릭 enum: `Option<T>`, `Result<T, E>` 모방
- 트레이트 바운드: `T: Display + Clone`, `where` 절
- 특정 타입에만 구현: `impl Point<f64> { ... }`
- const generics: `<T, const N: usize>` — 컴파일 타임 상수
- 연산자 오버로딩: `impl Add for Vector2D<T>`
- 조건부 메서드: 특정 트레이트가 있을 때만 메서드 제공

```rust
fn largest<T: PartialOrd>(list: &[T]) -> &T { /* ... */ }

struct Point<T> { x: T, y: T }

impl<T: Display + PartialOrd> Pair<T> {
    fn cmp_display(&self) { /* ... */ }
}

fn first_element<T, const N: usize>(arr: &[T; N]) -> &T { &arr[0] }
```

## 실행
```bash
cd RUST/09-generics && cargo run
```

## 핵심 요점
- 제네릭은 런타임 오버헤드 없이 타입 안전성 제공 (단형화)
- 트레이트 바운드는 제네릭 타입이 가져야 할 기능 명시
- const generics는 배열 크기 등 상수 값을 타입 매개변수로 사용
- `where` 절로 복잡한 바운드를 가독성 좋게 표현
