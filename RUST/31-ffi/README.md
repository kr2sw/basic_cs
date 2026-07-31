# 31: FFI — extern "C", unsafe, C 바인딩 개념

FFI(외부 함수 인터페이스)로 Rust에서 C 라이브러리를 호출하거나, C에서 Rust를 호출할 수 있습니다.

## extern "C"

```rust
extern "C" {
    fn strlen(s: *const c_char) -> usize;
}
```

`unsafe` 블록 안에서 호출합니다.

## libc 대체 (표준 라이브러리만 사용)

본 예제는 std의 `std::process::Command`로 C 라이브러리(`ntdll.dll`)의 존재를 확인하는 개념을 보여줍니다.

## 안전한 래퍼 패턴

raw 포인터를 안전한 Rust 타입으로 감싸는 패턴을 학습합니다.

## 실행

```bash
cd RUST/31-ffi
cargo run
```
