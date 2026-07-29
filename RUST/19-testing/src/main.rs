// 19-testing - main.rs
// 바이너리 크레이트: lib.rs의 함수 사용

use ch19_testing::{add, divide, fibonacci, is_palindrome, multiply};

fn main() {
    let a = 10;
    let b = 5;

    println!("add({}, {}) = {}", a, b, add(a, b));
    println!("multiply({}, {}) = {}", a, b, multiply(a, b));

    let n = 15;
    println!("fibonacci({}) = {}", n, fibonacci(n));

    println!("divide({}, {}) = {}", a, b, divide(a, b));

    let s = "A man, a plan, a canal: Panama";
    println!("is_palindrome('{}') = {}", s, is_palindrome(s));

    println!("모든 함수가 정상 동작합니다!");
}
