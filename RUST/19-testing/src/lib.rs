// 19-testing - lib.rs
// 테스트 가능한 공개 함수들

/// 주어진 두 수를 더합니다.
///
/// # 예제
///
/// ```
/// use ch19_testing::add;
///
/// let result = add(2, 3);
/// assert_eq!(result, 5);
/// ```
pub fn add(a: i32, b: i32) -> i32 {
    a + b
}

/// 주어진 두 수를 곱합니다.
///
/// # 예제
///
/// ```
/// use ch19_testing::multiply;
///
/// assert_eq!(multiply(3, 4), 12);
/// ```
pub fn multiply(a: i32, b: i32) -> i32 {
    a * b
}

/// n번째 피보나치 수를 반환합니다.
///
/// # 예제
///
/// ```
/// use ch19_testing::fibonacci;
///
/// assert_eq!(fibonacci(0), 0);
/// assert_eq!(fibonacci(1), 1);
/// assert_eq!(fibonacci(10), 55);
/// ```
pub fn fibonacci(n: u64) -> u64 {
    match n {
        0 | 1 => n,
        _ => fibonacci(n - 1) + fibonacci(n - 2),
    }
}

/// 나눗셈을 수행합니다. (0으로 나누면 panic)
///
/// # 예제
///
/// ```
/// use ch19_testing::divide;
///
/// assert_eq!(divide(10, 2), 5);
/// ```
///
/// ```should_panic
/// use ch19_testing::divide;
///
/// divide(1, 0); // panic 발생
/// ```
pub fn divide(a: i32, b: i32) -> i32 {
    if b == 0 {
        panic!("0으로 나눌 수 없습니다");
    }
    a / b
}

/// 문자열이 회문(palindrome)인지 확인합니다.
pub fn is_palindrome(s: &str) -> bool {
    let cleaned: String = s.chars().filter(|c| c.is_alphanumeric()).collect();
    let lower = cleaned.to_lowercase();
    lower == lower.chars().rev().collect::<String>()
}

/// 비공개 헬퍼 함수 (내부 테스트 가능)
#[allow(dead_code)]
fn internal_helper() -> bool {
    true
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn add_positive_numbers() {
        assert_eq!(add(2, 3), 5);
    }

    #[test]
    fn add_negative_numbers() {
        assert_eq!(add(-1, -2), -3);
    }

    #[test]
    fn add_zero() {
        assert_eq!(add(0, 5), 5);
        assert_eq!(add(5, 0), 5);
    }

    #[test]
    fn multiply_basic() {
        assert_eq!(multiply(3, 4), 12);
        assert_eq!(multiply(0, 100), 0);
        assert_eq!(multiply(-2, 3), -6);
    }

    #[test]
    fn fibonacci_base_cases() {
        assert_eq!(fibonacci(0), 0);
        assert_eq!(fibonacci(1), 1);
    }

    #[test]
    fn fibonacci_larger() {
        assert_eq!(fibonacci(10), 55);
        assert_eq!(fibonacci(20), 6765);
    }

    #[test]
    #[should_panic(expected = "0으로 나눌 수 없습니다")]
    fn divide_by_zero() {
        divide(1, 0);
    }

    #[test]
    fn divide_normal() {
        assert_eq!(divide(10, 2), 5);
        assert_eq!(divide(7, 3), 2); // 정수 나눗셈
    }

    #[test]
    fn palindrome_true() {
        assert!(is_palindrome("racecar"));
        assert!(is_palindrome("A man, a plan, a canal: Panama"));
        assert!(is_palindrome(""));
        assert!(is_palindrome("a"));
    }

    #[test]
    fn palindrome_false() {
        assert!(!is_palindrome("hello"));
        assert!(!is_palindrome("rust"));
    }

    #[test]
    fn internal_helper_works() {
        assert!(internal_helper());
    }

    #[test]
    #[ignore = "성능 테스트 - 느림"]
    fn fibonacci_large_slow() {
        assert_eq!(fibonacci(40), 102334155);
    }

    #[test]
    fn assert_ne_demo() {
        assert_ne!(add(1, 1), 3);
        assert_ne!(multiply(2, 2), 5);
    }
}
