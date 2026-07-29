# 11 Lifetimes — 라이프타임

참조자의 유효 범위를 명시하는 라이프타임 시스템: 댕글링 참조 방지.

## 주요 개념
- 라이프타임 어노테이션: `&'a T`, `&'a mut T`
- 여러 인자 간의 참조 수명 관계 명시
- 구조체 필드에 라이프타임 적용
- `'static` 라이프타임: 프로그램 전체 수명
- 라이프타임 생략 규칙 (Elision): 컴파일러가 자동 추론
  - 1) 각 입력 참조는 별도 라이프타임
  - 2) 하나의 입력 라이프타임이면 출력에 적용
  - 3) 메서드의 `&self` 라이프타임이 출력에 적용

```rust
fn longest<'a>(x: &'a str, y: &'a str) -> &'a str {
    if x.len() > y.len() { x } else { y }
}

struct Excerpt<'a> {
    part: &'a str,
}

fn first_word(s: &str) -> &str {
    // 라이프타임 생략 규칙으로 자동 추론
    let bytes = s.as_bytes();
    for (i, &item) in bytes.iter().enumerate() {
        if item == b' ' { return &s[..i]; }
    }
    &s[..]
}
```

## 실행
```bash
cd RUST/11-lifetimes && cargo run
```

## 핵심 요점
- 라이프타임은 댕글링 참조를 컴파일 타임에 방지
- `'a`는 이름표로, 실제 수명이 아닌 관계를 표현
- 생략 규칙 덕분에 대부분의 경우 명시 불필요
- `'static`은 문자열 리터럴 등 프로그램 전체 수명
