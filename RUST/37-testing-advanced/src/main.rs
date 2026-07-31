// 37: 고급 테스팅 — 속성 테스트(proptest 개념), 벤치마크
//
// 실행: cargo test
// 데모: cargo run

/// 두 수를 더하는 함수 (문서 테스트 포함)
///
/// ```
/// assert_eq!(ch37_testing_advanced::add(2, 3), 5);
/// ```
pub fn add(a: i32, b: i32) -> i32 {
    a + b
}

// === 1. 검증 대상 함수들 ===
pub fn factorial(n: u64) -> u64 {
    match n {
        0 | 1 => 1,
        _ => (2..=n).product(),
    }
}

pub fn is_palindrome(s: &str) -> bool {
    let chars: Vec<char> = s.chars().filter(|c| !c.is_whitespace()).collect();
    let n = chars.len();
    for i in 0..n / 2 {
        if chars[i] != chars[n - 1 - i] {
            return false;
        }
    }
    true
}

pub fn max_pair_product(nums: &[i32]) -> i32 {
    if nums.len() < 2 {
        return 0;
    }
    let mut a = i32::MIN;
    let mut b = i32::MIN;
    for &n in nums {
        if n > a {
            b = a;
            a = n;
        } else if n > b {
            b = n;
        }
    }
    a * b
}

fn main() {
    println!("테스트 실행: cargo test");
    println!("demo: add={}, factorial(5)={}, palindrome={}",
        add(1, 2),
        factorial(5),
        is_palindrome("소주 만병만 주소")
    );
}

// === 2. 단위 테스트 모듈 ===
#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_add() {
        assert_eq!(add(2, 3), 5);
        assert_eq!(add(-1, 1), 0);
    }

    #[test]
    fn test_factorial() {
        assert_eq!(factorial(0), 1);
        assert_eq!(factorial(1), 1);
        assert_eq!(factorial(5), 120);
    }

    #[test]
    fn test_palindrome() {
        assert!(is_palindrome("level"));
        assert!(is_palindrome("소주 만병만 주소"));
        assert!(!is_palindrome("hello"));
    }

    #[test]
    fn test_max_pair_product() {
        assert_eq!(max_pair_product(&[1, 5, 3, 4]), 20);
        assert_eq!(max_pair_product(&[1, 2]), 2);
        assert_eq!(max_pair_product(&[]), 0);
    }

    // === 3. 속성 테스트 (proptest 느낌): 랜덤 입력 검증 ===
    // proptest 크레이트처럼 무작위 입력을 많이 넣고 불변식을 확인합니다.
    fn test_is_palindrome_property_impl() {
        let mut rng = 42u64;
        for _ in 0..200 {
            // 단순 LCG 랜덤으로 문자열 생성
            rng = rng.wrapping_mul(6364136223846793005).wrapping_add(1442695040888963407);
            let len = (rng % 10) as usize;
            let mut s = String::new();
            for _ in 0..len {
                rng = rng.wrapping_mul(6364136223846793005).wrapping_add(1442695040888963407);
                let c = b'a' + (rng % 26) as u8;
                s.push(c as char);
            }
            // 불변식: 문자열 + reverse(s) 는 항상 palindrome
            let rev: String = s.chars().rev().collect();
            assert!(is_palindrome(&format!("{s}{rev}")));
        }
    }

    #[test]
    fn test_is_palindrome_property() {
        test_is_palindrome_property_impl();
    }

    // === 4. 경계 조건 테스트 ===
    #[test]
    #[should_panic(expected = "index out of bounds")]
    fn test_panic_bounds() {
        let v = [1, 2, 3];
        let idx: usize = std::hint::black_box(5); // 런타임 값이라 컴파일러가 감지 못함
        let _ = v[idx];
    }

    // === 5. 벤치마크 느낌 테스트 (시간 측정 출력) ===
    #[test]
    fn test_factorial_large() {
        let start = std::time::Instant::now();
        assert_eq!(factorial(20), 2432902008176640000);
        println!("factorial(20) 소요: {:?}", start.elapsed());
    }
}
