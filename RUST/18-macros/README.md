# 18 Macros — 매크로

선언적 매크로 (`macro_rules!`), 반복 패턴, derive 매크로, 내장 매크로.

## 주요 개념
- `macro_rules!` — 패턴 매칭으로 코드 생성
- 메타 변수: `$e:expr` (표현식), `$x:ident` (식별자), `$t:ty` (타입)
- 반복 패턴: `$($x:expr),*` — 여러 인자 처리
- `stringify!` — 코드를 문자열로 변환
- `vec!`, `assert_eq!`, `file!`, `line!`, `column!` 등 내장 매크로
- Builder 패턴 매크로로 반복 코드 생성
- `#[derive(...)]` — derive 매크로로 트레이트 자동 구현

```rust
macro_rules! calculate {
    (eval $e:expr) => {
        {
            let val: usize = $e;
            println!("{} = {}", stringify!($e), val);
            val
        }
    };
    (eval $e:expr, $(eval $rest:expr),*) => {
        calculate!(eval $e);
        $(calculate!(eval $rest);)*
    };
}

let sum = calculate!(eval 1 + 2);
calculate!(eval 10 * 5, eval 100 / 4);
```

## 실행
```bash
cd RUST/18-macros && cargo run
```

## 핵심 요점
- 매크로는 컴파일 타임에 코드 생성 (메타프로그래밍)
- `macro_rules!`은 패턴 매칭으로 다양한 문법 지원
- 반복 패턴 `$()`로 가변 인자 처리 가능
- `derive` 매크로로 상용구 코드(boilerplate) 자동 생성
