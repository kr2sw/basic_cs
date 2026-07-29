# 13 Modules & Crates — 모듈과 크레이트

러스트 모듈 시스템: `mod`, `pub`, `use`, 경로, 외부 크레이트, Cargo.toml.

## 주요 개념
- `mod` 키워드: 모듈 선언 (파일 또는 디렉토리)
- 가시성 제어: `pub`, `pub(crate)`, `pub(super)`
- `use` 키워드: 경로 단축, `as` 별칭
- `self` / `super`: 상대 경로
- `pub use` (Re-export): 내부 항목을 외부로 노출
- 외부 크레이트: `Cargo.toml`에 추가 후 `use`
- `mod.rs` 또는 파일명으로 모듈 구성

```rust
mod math;
use math::advanced::factorial;
use math::add;
use math::advanced::power as pow;

fn main() {
    let sum = add(10, 20);
    let fact = factorial(5);
    println!("add: {}, factorial: {}", sum, fact);
}
```

## 실행
```bash
cd RUST/13-modules-crates && cargo run
```

## 핵심 요점
- 모듈은 코드를 논리적으로 그룹화하고 캡슐화
- 기본적으로 모든 항목은 비공개 (`pub`으로 공개)
- `use`로 경로 단축, `as`로 별칭 가능
- `Cargo.toml`에 의존성 추가 후 외부 크레이트 사용
