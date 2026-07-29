// 19-testing - integration test
// 통합 테스트: lib.rs의 공개 API만 테스트 가능

use ch19_testing;

#[test]
fn integration_add() {
    assert_eq!(ch19_testing::add(100, 200), 300);
}

#[test]
fn integration_multiply() {
    assert_eq!(ch19_testing::multiply(6, 7), 42);
}

#[test]
fn integration_fibonacci() {
    assert_eq!(ch19_testing::fibonacci(15), 610);
}

#[test]
fn integration_divide() {
    assert_eq!(ch19_testing::divide(20, 4), 5);
}

#[test]
#[should_panic]
fn integration_divide_by_zero() {
    ch19_testing::divide(1, 0);
}

#[test]
fn integration_palindrome() {
    assert!(ch19_testing::is_palindrome("level"));
    assert!(!ch19_testing::is_palindrome("world"));
}
