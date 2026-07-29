# 12 Closures & Iterators — 클로저와 반복자

클로저(익명 함수)와 반복자 패턴: 환경 캡처, Iterator 트레이트, 어댑터 메서드.

## 주요 개념
- 클로저 문법: `|params| body`, 타입 추론
- 환경 캡처 방식: `Fn` (참조) / `FnMut` (가변 참조) / `FnOnce` (소유권)
- `move` 키워드: 클로저로 소유권 이동
- 반복자: `iter()` (불변) / `iter_mut()` (가변) / `into_iter()` (소유권)
- 반복자 어댑터: `map`, `filter`, `fold`, `enumerate`, `zip`
- 지연 평가(Lazy Evaluation): `collect()`로 소비할 때까지 실행 안 함
- 메서드 체이닝: 여러 어댑터 연속 호출

```rust
let add = |a, b| a + b;
println!("{}", add(3, 5));

let doubled: Vec<i32> = numbers.iter().map(|x| x * 2).collect();

let result: i32 = (1..=10)
    .filter(|x| x % 2 == 0)
    .map(|x| x * x)
    .sum();

let sum = numbers.iter().fold(0, |acc, x| acc + x);
```

## 실행
```bash
cd RUST/12-closures-iterators && cargo run
```

## 핵심 요점
- 클로저는 환경을 캡처하는 익명 함수
- `Fn`, `FnMut`, `FnOnce`는 캡처 방식에 따라 자동 결정
- 반복자 어댑터는 지연 평가되어 효율적
- 체이닝으로 복잡한 데이터 변환을 선언적으로 표현
