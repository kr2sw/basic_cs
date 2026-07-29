//! 08-error-handling: panic!, Result, Option, ? 연산자, 커스텀 에러
//!
//! 러스트의 에러 처리 시스템을 다룹니다.

use std::fmt;
use std::num::ParseIntError;

// =========================================================
// 커스텀 에러 타입
// =========================================================
#[derive(Debug)]
enum MathError {
    DivisionByZero,
    NegativeSquareRoot,
    Overflow(String),
}

// Display 구현 (사용자에게 보여줄 메시지)
impl fmt::Display for MathError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        match self {
            MathError::DivisionByZero => write!(f, "0으로 나눌 수 없습니다"),
            MathError::NegativeSquareRoot => {
                write!(f, "음수의 제곱근은 실수 범위에서 정의되지 않습니다")
            }
            MathError::Overflow(msg) => write!(f, "오버플로우: {}", msg),
        }
    }
}

// std::error::Error 구현
impl std::error::Error for MathError {}

// From<&str> 구현으로 편의성 제공
impl From<&str> for MathError {
    fn from(msg: &str) -> Self {
        MathError::Overflow(msg.to_string())
    }
}

// =========================================================
// 1. panic! - 복구 불가능한 에러
// =========================================================
fn explain_panic() {
    println!("\n=== panic! ===");
    println!("panic!은 복구 불가능한 에러입니다.");
    println!("프로그램이 중단되고 스택이 unwind되거나 abort됩니다.");

    // 아래 코드의 주석을 해제하면 패닉 발생
    // panic!("이것은 패닉입니다!");
    // let v = vec![1, 2, 3];
    // v[99];  // 인덱스 범위 초과 -> 패닉
}

// =========================================================
// 2. Result<T, E> - 복구 가능한 에러
// =========================================================
fn divide(numerator: f64, denominator: f64) -> Result<f64, MathError> {
    if denominator == 0.0 {
        Err(MathError::DivisionByZero)
    } else {
        Ok(numerator / denominator)
    }
}

fn sqrt(value: f64) -> Result<f64, MathError> {
    if value < 0.0 {
        Err(MathError::NegativeSquareRoot)
    } else {
        Ok(value.sqrt())
    }
}

// =========================================================
// 3. match로 Result 처리
// =========================================================
fn handle_with_match() {
    println!("\n=== match로 Result 처리 ===");

    let result = divide(10.0, 2.0);
    match result {
        Ok(value) => println!("10 / 2 = {}", value),
        Err(e) => println!("에러: {}", e),
    }

    match divide(10.0, 0.0) {
        Ok(value) => println!("10 / 0 = {}", value),
        Err(e) => println!("에러: {}", e),
    }
}

// =========================================================
// 4. unwrap과 expect
// =========================================================
fn handle_with_unwrap() {
    println!("\n=== unwrap / expect ===");

    // unwrap - Ok면 값 반환, Err면 panic!
    let ok_result: Result<i32, &str> = Ok(42);
    let value = ok_result.unwrap();
    println!("unwrap 성공: {}", value);

    // expect - unwrap과 같지만 커스텀 메시지
    let ok_result: Result<i32, &str> = Ok(100);
    let value = ok_result.expect("값이 있어야 합니다");
    println!("expect 성공: {}", value);

    // unwrap_or - 기본값 제공
    let err_result: Result<i32, &str> = Err("에러");
    let value = err_result.unwrap_or(-1);
    println!("unwrap_or: {}", value);

    // unwrap_or_else - 클로저로 기본값 계산
    let err_result: Result<i32, &str> = Err("에러");
    let value = err_result.unwrap_or_else(|e| {
        println!("에러 발생: {}, 기본값 0 사용", e);
        0
    });
    println!("unwrap_or_else: {}", value);
}

// =========================================================
// 5. ? 연산자 (물음표 연산자)
// =========================================================
fn calculate_sqrt_of_ratio(a: f64, b: f64) -> Result<f64, MathError> {
    let ratio = divide(a, b)?;  // Err면 즉시 반환
    let result = sqrt(ratio)?;  // Err면 즉시 반환
    Ok(result)
}

fn handle_with_question_mark() {
    println!("\n=== ? 연산자 ===");

    match calculate_sqrt_of_ratio(16.0, 4.0) {
        Ok(val) => println!("sqrt(16/4) = {}", val),
        Err(e) => println!("에러: {}", e),
    }

    match calculate_sqrt_of_ratio(1.0, 0.0) {
        Ok(val) => println!("sqrt(1/0) = {}", val),
        Err(e) => println!("의도적 에러: {}", e),
    }

    match calculate_sqrt_of_ratio(-1.0, 1.0) {
        Ok(val) => println!("sqrt(-1/1) = {}", val),
        Err(e) => println!("의도적 에러: {}", e),
    }
}

// ? 연산자 with Option
fn find_first_even(numbers: &[i32]) -> Option<&i32> {
    let first = numbers.get(0)?;  // None이면 반환
    if first % 2 == 0 {
        Some(first)
    } else {
        None
    }
}

// ? 연산자 체이닝
fn parse_and_double(input: &str) -> Result<i32, ParseIntError> {
    let val: i32 = input.parse()?;
    Ok(val * 2)
}

// =========================================================
// 6. map, and_then, 기타 Result combinators
// =========================================================
fn handle_with_combinators() {
    println!("\n=== Result combinators ===");

    // map: Ok 값을 변환
    let result: Result<i32, &str> = Ok(5);
    let mapped = result.map(|v| v * 3);
    println!("map: {:?}", mapped);

    // map_err: Err 값을 변환
    let result: Result<i32, &str> = Err("에러 발생");
    let mapped_err = result.map_err(|e| format!("매핑된: {}", e));
    println!("map_err: {:?}", mapped_err);

    // and_then: flat map (Result 반환 함수 연결)
    let result: Result<i32, &str> = Ok(10);
    let chained = result
        .and_then(|v| Ok(v * 2))
        .and_then(|v| if v > 0 { Ok(v) } else { Err("음수") });
    println!("and_then 체인: {:?}", chained);

    // or_else: Err를 다른 Result로 대체
    let err: Result<i32, &str> = Err("에러");
    let recovered: Result<i32, &str> = err.or_else(|_| Ok(42));
    println!("or_else (복구): {:?}", recovered);

    // ok_or: Option -> Result 변환
    let some: Option<i32> = Some(42);
    let result: Result<i32, &str> = some.ok_or("값 없음");
    println!("ok_or: {:?}", result);

    let none: Option<i32> = None;
    let result: Result<i32, &str> = none.ok_or("값 없음");
    println!("ok_or (None): {:?}", result);
}

// =========================================================
// 7. Option<T> combinators
// =========================================================
fn handle_option_combinators() {
    println!("\n=== Option combinators ===");

    let some: Option<i32> = Some(42);
    let none: Option<i32> = None;

    // map
    println!("map Some: {:?}", some.map(|v| v * 2));
    println!("map None: {:?}", none.map(|v| v * 2));

    // and_then
    fn try_double(v: i32) -> Option<i32> {
        if v > 0 { Some(v * 2) } else { None }
    }
    println!("and_then Some(10): {:?}", Some(10).and_then(try_double));
    println!("and_then Some(-5): {:?}", Some(-5).and_then(try_double));

    // filter
    println!("filter (>20) Some(42): {:?}", Some(42).filter(|&v| v > 20));
    println!("filter (>50) Some(42): {:?}", Some(42).filter(|&v| v > 50));

    // or, or_else
    println!("or: {:?}", none.or(Some(99)));
    println!("or_else: {:?}", none.or_else(|| Some(100)));

    // unwrap_or, unwrap_or_else
    println!("unwrap_or: {}", none.unwrap_or(-1));
    println!("unwrap_or_else: {}", none.unwrap_or_else(|| -2));

    // get_or_insert
    let mut opt = Some(10);
    *opt.get_or_insert(0) = 5;
    println!("get_or_insert Some: {:?}", opt);

    let mut opt2: Option<i32> = None;
    *opt2.get_or_insert(0) = 5;
    println!("get_or_insert None: {:?}", opt2);

    // zip
    let a = Some(1);
    let b = Some(2);
    println!("zip: {:?}", a.zip(b));

    // flatten
    let nested = Some(Some(42));
    println!("flatten: {:?}", nested.flatten());
}

// =========================================================
// 8. 여러 에러 타입 처리 (Box<dyn Error>)
// =========================================================
use std::error::Error;

fn parse_and_calculate(input: &str) -> Result<f64, Box<dyn Error>> {
    let num: f64 = input.parse()?;            // ParseFloatError
    if num < 0.0 {
        return Err("음수는 허용되지 않습니다".into());
    }
    let result = sqrt(num)?;                   // MathError
    Ok(result)
}

fn handle_multiple_errors() {
    println!("\n=== 여러 에러 타입 처리 ===");

    match parse_and_calculate("16") {
        Ok(v) => println!("결과: {}", v),
        Err(e) => println!("에러: {}", e),
    }

    match parse_and_calculate("-5") {
        Ok(v) => println!("결과: {}", v),
        Err(e) => println!("에러: {}", e),
    }

    match parse_and_calculate("not_a_number") {
        Ok(v) => println!("결과: {}", v),
        Err(e) => println!("에러: {}", e),
    }
}

// =========================================================
// 9. Option과 Result 변환
// =========================================================
fn convert_between_option_result() {
    println!("\n=== Option <-> Result 변환 ===");

    // Result -> Option
    let ok: Result<i32, &str> = Ok(42);
    println!("Result Ok -> Option: {:?}", ok.ok());

    let err: Result<i32, &str> = Err("에러");
    println!("Result Err -> Option: {:?}", err.ok());

    // Option -> Result
    let some: Option<i32> = Some(42);
    println!("Option Some -> Result: {:?}", some.ok_or("값 없음"));

    let none: Option<i32> = None;
    println!("Option None -> Result: {:?}", none.ok_or("값 없음"));

    // transpose
    let result_option: Result<Option<i32>, &str> = Ok(Some(42));
    println!("transpose: {:?}", result_option.transpose());
}

// =========================================================
// 10. 에러 발생 함수 예제
// =========================================================
fn safe_divide_example() {
    println!("\n=== 안전한 나눗셈 ===");

    let pairs = [(10.0, 2.0), (10.0, 0.0), (-4.0, 2.0)];

    for (a, b) in pairs {
        let result = divide(a, b);
        match result {
            Ok(v) => println!("{} / {} = {}", a, b, v),
            Err(e) => println!("{} / {} -> 에러: {}", a, b, e),
        }
    }
}

fn main() {
    println!("=== 08-error-handling: 에러 처리 ===");

    explain_panic();
    handle_with_match();
    handle_with_unwrap();
    handle_with_question_mark();
    handle_with_combinators();
    handle_option_combinators();
    handle_multiple_errors();
    convert_between_option_result();
    safe_divide_example();

    // ? 연산자 추가 예제
    let result = parse_and_double("42");
    println!("\nparse_and_double(\"42\"): {:?}", result);

    let result = parse_and_double("abc");
    println!("parse_and_double(\"abc\"): {:?}", result);

    // find_first_even
    let numbers = [1, 3, 5, 7];
    println!("첫 짝수: {:?}", find_first_even(&numbers));
    let numbers = [1, 2, 3, 4];
    println!("첫 짝수: {:?}", find_first_even(&numbers));

    // into()를 사용한 에러 변환
    fn validate_age(age: i32) -> Result<i32, MathError> {
        if age < 0 {
            Err("나이는 음수일 수 없습니다".into())  // From<&str> 사용
        } else if age > 150 {
            Err(MathError::Overflow(format!("비현실적 나이: {}", age)))
        } else {
            Ok(age)
        }
    }

    match validate_age(-5) {
        Ok(v) => println!("나이: {}", v),
        Err(e) => println!("검증 실패: {}", e),
    }
}
