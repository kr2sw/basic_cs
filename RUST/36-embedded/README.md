# 36: 임베디드 Rust — no_std, cortex-m 개념

임베디드 Rust는 `no_std` 환경에서 실행됩니다. 표준 라이브러리(OS 의존) 대신 코어 라이브러리만 사용합니다.

## no_std

```rust
#![no_std]
```

힙, OS 기능이 없는 환경. 대표 타깃으로 ARM Cortex-M이 있습니다.

## cortex-m 개념 (외부 크레이트)

```rust
// cortex-m-rt, cortex-m, embedded-hal
use cortex_m_rt::entry;

#[entry]
fn main() -> ! {
    loop {}
}
```

## 본 챕터 구현

`no_std`의 제약을 데스크톱에서 재현하기 위해, 표준 라이브러리 사용을 제한한 "가상 MCU" 시뮬레이터를 만듭니다.

## 실행

```bash
cd RUST/36-embedded
cargo run
```
