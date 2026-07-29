# 08 Error Handling — 에러 처리

러스트의 에러 처리 시스템: panic!, Result, Option, ? 연산자, 커스텀 에러.

## 주요 개념
- `panic!` — 복구 불가능한 에러 (프로그램 중단)
- `Result<T, E>` — 복구 가능한 에러 (Ok / Err)
- `match`로 Result 처리, `unwrap` / `expect` / `unwrap_or`
- `?` 연산자 — 에러를 호출자로 전파 (간결한 에러 처리)
- `map`, `and_then`, `or_else` 등 Result/Option 콤비네이터
- 커스텀 에러 타입: `Display` + `Error` 트레이트 구현
- `Box<dyn Error>` — 여러 에러 타입 통합 처리
- `Option <-> Result` 변환 (`ok_or`, `transpose`)

```rust
fn calculate_sqrt_of_ratio(a: f64, b: f64) -> Result<f64, MathError> {
    let ratio = divide(a, b)?;
    let result = sqrt(ratio)?;
    Ok(result)
}

fn find_first_even(numbers: &[i32]) -> Option<&i32> {
    let first = numbers.get(0)?;
    if first % 2 == 0 { Some(first) } else { None }
}
```

## 실행
```bash
cd RUST/08-error-handling && cargo run
```

## 핵심 요점
- `?` 연산자로 에러 전파를 간결하게 처리
- `unwrap()`은 간단하지만 panic 위험 — 가급적 `match`나 `?` 사용
- 커스텀 에러 타입으로 구체적인 에러 정보 전달 가능
- `Box<dyn Error>`로 여러 에러 타입을 하나로 통합
