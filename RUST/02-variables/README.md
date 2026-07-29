# 02 Variables — 변수와 자료형

러스트의 변수 시스템: 불변성, 가변성, 섀도잉, 다양한 자료형과 상수.

## 주요 개념
- `let` 바인딩 (불변)과 `let mut` (가변)
- 섀도잉 (Shadowing) — 같은 이름 재선언, 타입 변경 가능
- 정수 계열 (`i8`~`i128`, `u8`~`u128`, `usize`)
- 부동소수점 (`f32`, `f64`) 및 특수값 (INFINITY, NAN)
- 튜플과 배열 (고정 크기)
- 상수 (`const`)와 정적 변수 (`static`)
- 타입 추론 (type inference)

```rust
let x = 5;           // 불변
let mut y = 10;      // 가변
y = 20;

let shadowed = "문자열";
let shadowed = shadowed.len();  // 타입 변경: &str -> usize

let tuple: (i32, f64, char) = (42, 3.14, 'R');
let array: [i32; 5] = [1, 2, 3, 4, 5];
```

## 실행
```bash
cd RUST/02-variables && cargo run
```

## 핵심 요점
- 러스트 변수는 기본적으로 불변
- 섀도잉은 같은 이름으로 새 변수 선언 (타입 변경 가능)
- `const`는 컴파일 타임 상수, `static`은 고정 메모리 주소
- 튜플은 서로 다른 타입 가능, 배열은 같은 타입만 가능
