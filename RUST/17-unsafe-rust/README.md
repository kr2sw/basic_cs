# 17 Unsafe Rust — 안전하지 않은 러스트

Raw 포인터, unsafe 함수/블록, extern FFI, 가변 정적 변수 등 고급 기능.

## 주요 개념
- Raw 포인터: `*const T`, `*mut T` (생성은 안전, 역참조는 unsafe)
- `unsafe` 블록: 외부 라이브러리 함수 호출, raw 포인터 역참조
- `unsafe` 함수: 호출 시 unsafe 블록 필요
- `unsafe trait` 구현
- `extern "C"`: C ABI 함수 선언 및 호출 (FFI)
- 가변 정적 변수 (`static mut`) 접근
- 인라인 어셈블리 (`asm!`)
- 안전한 추상화로 unsafe 감싸기

```rust
let mut num = 42;
let r1 = &num as *const i32;
let r2 = &mut num as *mut i32;

unsafe {
    println!("r1: {}", *r1);
    *r2 = 100;
}

unsafe extern "C" {
    fn abs(input: i32) -> i32;
    fn strlen(s: *const u8) -> usize;
}
```

## 실행
```bash
cd RUST/17-unsafe-rust && cargo run
```

## 핵심 요점
- `unsafe`는 borrow checker 해제 — 메모리 안전성은 프로그래머 책임
- raw 포인터 생성은 안전, 역참조만 unsafe
- `unsafe` 코드는 최소화하고 안전한 추상화로 감싸는 것이 모범 사례
- FFI로 C 라이브러리 연동 가능, 반대로 Rust 함수를 C에 노출 가능
