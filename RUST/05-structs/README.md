# 05 Structs — 구조체

구조체 정의, 메서드, 연관 함수, 튜플 구조체, 유닛 구조체, `derive` 매크로.

## 주요 개념
- 구조체 정의 및 생성 (필드명 축약, 업데이트 문법 `..`)
- 튜플 구조체 (Tuple Struct) — 필드명 없는 명명된 튜플
- 유닛 구조체 (Unit Struct) — 필드 없음
- `impl` 블록: 메서드(`&self`)와 연관 함수(`Self::`)
- `#[derive(Debug, Clone, Copy, PartialEq)]`
- 여러 `impl` 블록 가능
- 구조체 패턴 매칭과 `let` 분해

```rust
struct Rectangle { width: u32, height: u32 }

impl Rectangle {
    fn area(&self) -> u32 { self.width * self.height }
    fn square(size: u32) -> Self { Self { width: size, height: size } }
}

let rect = Rectangle { width: 30, height: 50 };
println!("면적: {}", rect.area());
let square = Rectangle::square(25);
```

## 실행
```bash
cd RUST/05-structs && cargo run
```

## 핵심 요점
- 구조체는 관련 데이터를 묶는 사용자 정의 타입
- 메서드는 첫 인자가 `&self`, 연관 함수는 `Self::`로 호출
- `derive`로 공통 트레이트 자동 구현 가능
- 업데이트 문법 `..other`로 필드 복사
