//! 01-hello-world: 기본 출력, 입력, 변수, 주석
//!
//! 이 파일은 러스트의 가장 기본적인 개념들을 다룹니다.
//! `cargo run`으로 실행해보세요.

/// 이 함수는 프로그램의 진입점입니다.
/// ///는 문서 주석(doc comment)으로, `cargo doc`으로 문서를 생성할 수 있습니다.
fn main() {
    // 1. println! 매크로 - 콘솔에 출력 (자동으로 줄바꿈 추가)
    println!("Hello, world!");
    println!("안녕하세요, 러스트!");

    // 2. eprintln! 매크로 - 표준 에러(stderr)로 출력
    eprintln!("이 메시지는 stderr로 출력됩니다.");

    // 3. format! 매크로 - 문자열 포맷팅 (출력 없이 String 반환)
    let formatted = format!("{} + {} = {}", 3, 5, 3 + 5);
    println!("format! 결과: {}", formatted);

    // 4. 기본 자료형
    let integer: i32 = -42;           // 부호 있는 32비트 정수
    let float: f64 = 3.1415926535;    // 64비트 부동소수점
    let is_rust_fun: bool = true;     // 불리언
    let letter: char = 'R';           // 문자 (4바이트, 유니코드)

    println!("정수: {}, 실수: {}, 불리언: {}, 문자: {}", integer, float, is_rust_fun, letter);

    // 5. 변수와 출력 형식 지정
    let name = "러스트";
    let version = 2024;
    println!("{name} 언어, 버전 {version}");  // named arguments

    // 6. 디버그 출력
    let numbers = [1, 2, 3, 4, 5];
    println!("배열 디버그 출력: {:?}", numbers);
    println!("배열 예쁜 출력: {:#?}", numbers);

    // 7. 사용자 입력 받기
    let mut input = String::new();
    println!("아무 내용이나 입력하고 Enter를 누르세요:");
    std::io::stdin()
        .read_line(&mut input)
        .expect("입력을 읽는데 실패했습니다.");
    println!("입력한 내용: {}", input.trim());

    // 8. 간단한 연산
    let sum = 10 + 20;
    let product = 6 * 7;
    let divided = 42.0 / 6.5;
    println!("합: {}, 곱: {}, 나눗셈: {:.2}", sum, product, divided);

    // 9. 타입 캐스팅 (as)
    let pi: f64 = 3.14;
    let pi_int: i32 = pi as i32;
    println!("실수를 정수로: {} -> {}", pi, pi_int);  // 3.14 -> 3

    // 10. 블록 표현식
    let result = {
        let x = 10;
        let y = 20;
        x + y  // 마지막 표현식이 반환값 (세미콜론 없음)
    };
    println!("블록 표현식 결과: {}", result);
}
