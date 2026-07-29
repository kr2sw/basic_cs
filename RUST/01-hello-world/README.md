# 01 Hello, World! — 기본 출력과 입력

러스트의 가장 기초적인 문법 요소를 다룹니다: 출력 매크로, 변수, 자료형, 사용자 입력, 표현식.

## 주요 개념
- `println!`, `eprintln!`, `format!` 매크로
- 기본 자료형 (`i32`, `f64`, `bool`, `char`)
- 변수 선언과 타입 캐스팅 (`as`)
- 블록 표현식 (값을 반환하는 `{ }`)
- 사용자 입력 처리 (`std::io::stdin`)

```rust
fn main() {
    let name = "러스트";
    println!("{name} 언어, 버전 {version}", version = 2024);

    let result = {
        let x = 10;
        let y = 20;
        x + y  // 마지막 표현식이 반환값
    };
    println!("블록 표현식 결과: {}", result);
}
```

## 실행
```bash
cd RUST/01-hello-world && cargo run
```

## 핵심 요점
- `println!`은 매크로(함수 아님)이며 다양한 포맷팅 지원
- `format!`은 문자열을 반환, `eprintln!`은 stderr 출력
- 블록 표현식으로 값을 반환할 수 있음
- `as` 키워드로 타입 변환 가능
