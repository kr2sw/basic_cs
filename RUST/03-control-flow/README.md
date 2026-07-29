# 03 Control Flow — 제어 흐름

조건문, 반복문, 패턴 매칭 등 러스트의 제어 흐름을 다룹니다.

## 주요 개념
- `if` / `else if` / `else` 표현식 (값 반환 가능)
- `loop` — 무한 반복, `break`로 값 반환
- `while` 조건 반복, `while let` 패턴 반복
- `for` 범위 반복 (`0..5`, `0..=5`), `enumerate`
- `match` 표현식 — 패턴 매칭, 가드(`if`), 범위, `|`
- `if let` — 한 패턴만 간결하게 매칭
- 루프 레이블 (`'outer`)로 중첩 루프 제어
- `continue`로 건너뛰기

```rust
let grade = if score >= 90 { 'A' } else if score >= 80 { 'B' } else { 'F' };

let result = loop { counter += 1; if counter == 10 { break counter * 2; } };

for (index, value) in arr.iter().enumerate() { /* ... */ }

match day {
    1 => "월요일",
    6 | 7 => "주말",
    _ => "알 수 없음",
};
```

## 실행
```bash
cd RUST/03-control-flow && cargo run
```

## 핵심 요점
- `if`와 `match`는 표현식 → 값을 반환 가능
- `match`는 모든 경우를 빠짐없이 처리해야 함 (exhaustive)
- 루프 레이블로 중첩 루프 한 번에 탈출 가능
- `if let`은 한 가지 패턴만 검사할 때 간결함
