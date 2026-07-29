# 06 Enums — 열거형

러스트의 강력한 열거형 시스템: 데이터를 담는 enum, 패턴 매칭, Option, Result.

## 주요 개념
- 열거형 정의와 값 생성 (`IpAddrKind::V4`)
- 각 variant가 서로 다른 타입의 데이터를 보유 가능
- `impl`로 enum에 메서드 정의
- `match`로 exhaustive 패턴 매칭
- `Option<T>` — null 안전성 (Some/None)
- `Result<T, E>` — 에러 처리를 위한 enum
- `if let` — 한 패턴만 간결하게 매칭
- enum 콤비네이터 (`map`, `and_then`, `filter`, `unwrap_or`)

```rust
enum Message {
    Quit,
    Move { x: i32, y: i32 },
    Write(String),
    ChangeColor(i32, i32, i32),
}

fn plus_one(x: Option<i32>) -> Option<i32> {
    match x {
        None => None,
        Some(i) => Some(i + 1),
    }
}
```

## 실행
```bash
cd RUST/06-enums && cargo run
```

## 핵심 요점
- enum은 여러 variant를 하나의 타입으로 묶음
- `Option<T>`는 null 개념을 안전하게 대체
- `match`는 모든 경우를 처리해야 함 (컴파일러 강제)
- `if let`은 한 패턴만 다룰 때 간결 구문
