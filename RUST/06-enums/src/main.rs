//! 06-enums: 열거형 정의, 패턴 매칭, Option, Result
//!
//! 러스트의 강력한 열거형 시스템을 다룹니다.

// 1. 기본 열거형 정의
#[derive(Debug)]
enum IpAddrKind {
    V4,
    V6,
}

// 2. 데이터를 담는 열거형
#[derive(Debug)]
enum IpAddr {
    V4(String),
    V6(String),
}

// 3. 각 variant가 다른 타입의 데이터를 담을 수 있음
#[derive(Debug)]
enum Message {
    Quit,                       // 데이터 없음
    Move { x: i32, y: i32 },    // 익명 구조체
    Write(String),              // 문자열
    ChangeColor(i32, i32, i32), // 튜플
}

// 4. enum에 메서드 정의
impl Message {
    fn call(&self) {
        match self {
            Message::Quit => println!("Quit 메서드 호출"),
            Message::Move { x, y } => println!("Move to ({}, {})", x, y),
            Message::Write(text) => println!("Write: {}", text),
            Message::ChangeColor(r, g, b) => {
                println!("ChangeColor: RGB({}, {}, {})", r, g, b);
            }
        }
    }

    fn kind(&self) -> &str {
        match self {
            Message::Quit => "Quit",
            Message::Move { .. } => "Move",
            Message::Write(_) => "Write",
            Message::ChangeColor(..) => "ChangeColor",
        }
    }
}

// 5. 열거형을 포함하는 구조체
#[derive(Debug)]
enum Coin {
    Penny,
    Nickel,
    Dime,
    Quarter(UsState),  // Quarter는 추가 데이터 보유
}

#[derive(Debug)]
enum UsState {
    Alabama,
    Alaska,
    California,
    // ...
}

fn main() {
    // 1. 열거형 값 생성
    let four = IpAddrKind::V4;
    let six = IpAddrKind::V6;
    println!("{:?}, {:?}", four, six);

    // 2. 데이터가 있는 열거형
    let home = IpAddr::V4(String::from("127.0.0.1"));
    let loopback = IpAddr::V6(String::from("::1"));
    println!("home: {:?}", home);
    println!("loopback: {:?}", loopback);

    // 3. Message 열거형 사용
    let messages = vec![
        Message::Quit,
        Message::Move { x: 10, y: 20 },
        Message::Write(String::from("안녕하세요")),
        Message::ChangeColor(255, 0, 0),
    ];

    for msg in &messages {
        msg.call();
        println!("kind: {}", msg.kind());
    }

    // 4. match - exhaustive matching
    let coin1 = Coin::Quarter(UsState::California);
    println!("coin1 가치: {} cent(s)", value_in_cents(&coin1));

    // 5. match with Option<T>
    let five = Some(5);
    let six = plus_one(five);
    let none = plus_one(None);
    println!("{:?} + 1 = {:?}, None = {:?}", five, six, none);

    // 6. if let - 한 패턴만 매칭 (syntactic sugar)
    let some_value = Some(42);
    if let Some(42) = some_value {
        println!("정확히 42입니다!");
    }

    // if let with else
    let config_max = Some(3u8);
    if let Some(max) = config_max {
        println!("최대값: {}", max);
    } else {
        println!("설정 없음");
    }

    // match vs if let 비교
    let favorite = Some("Rust".to_string());
    // match 버전
    match &favorite {
        Some(lang) => println!("match: 좋아하는 언어는 {}", lang),
        None => {}
    }
    // if let 버전 (간결)
    if let Some(lang) = &favorite {
        println!("if let: 좋아하는 언어는 {}", lang);
    }

    // 7. Option<T> 활용
    let x: Option<i32> = Some(10);
    let y: i32 = 20;
    // let sum = x + y;  // 컴파일 에러! Option과 i32는 다른 타입
    let sum = x.unwrap_or(0) + y;
    println!("Option 합: {}", sum);

    // Option 메서드들
    let opt: Option<i32> = Some(5);
    println!("is_some: {}", opt.is_some());
    println!("is_none: {}", opt.is_none());
    println!("unwrap_or(0): {}", opt.unwrap_or(0));
    println!("unwrap_or_else: {}", opt.unwrap_or_else(|| 42));

    let mapped = opt.map(|v| v * 2);
    println!("map: {:?}", mapped);

    let filtered = opt.filter(|&v| v > 10);
    println!("filter: {:?}", filtered);

    // 8. Result<T, E> 활용
    let result_success: Result<i32, &str> = Ok(42);
    let result_error: Result<i32, &str> = Err("에러 발생");

    // match로 Result 처리
    match &result_success {
        Ok(value) => println!("성공: {}", value),
        Err(e) => println!("실패: {}", e),
    }

    // Result 메서드들
    println!("unwrap_or(0): {}", result_success.unwrap_or(0));
    println!("unwrap_or(0): {}", result_error.unwrap_or(0));

    let doubled = result_success.map(|v| v * 2);
    println!("map: {:?}", doubled);

    // and_then (flatmap)
    let chained = result_success
        .and_then(|v| if v > 0 { Ok(v * 2) } else { Err("음수") });
    println!("and_then: {:?}", chained);

    // 9. 커스텀 열거형과 종합 예제
    let states = vec![
        Coin::Penny,
        Coin::Nickel,
        Coin::Dime,
        Coin::Quarter(UsState::Alabama),
    ];

    for coin in &states {
        describe_coin(coin);
    }

    // 10. enum으로 상태 머신 표현
    #[derive(Debug)]
    enum TrafficLight {
        Red,
        Yellow,
        Green,
    }

    let light = TrafficLight::Red;
    let action = match light {
        TrafficLight::Red => "정지",
        TrafficLight::Yellow => "주의",
        TrafficLight::Green => "진행",
    };
    println!("신호등: {:?} -> {}", light, action);

    // 11. Option<T> with ? operator (함수 내에서)
    fn try_divide(numerator: f64, denominator: f64) -> Option<f64> {
        if denominator == 0.0 {
            None
        } else {
            Some(numerator / denominator)
        }
    }

    let result = try_divide(10.0, 2.0);
    println!("10/2 = {:?}", result);
    println!("10/0 = {:?}", try_divide(10.0, 0.0));
}

// --- 함수 정의 ---

fn value_in_cents(coin: &Coin) -> u8 {
    match coin {
        Coin::Penny => {
            println!("럭키 페니!");
            1
        }
        Coin::Nickel => 5,
        Coin::Dime => 10,
        Coin::Quarter(state) => {
            println!("{:?} 주의 25센트!", state);
            25
        }
    }
}

fn plus_one(x: Option<i32>) -> Option<i32> {
    match x {
        None => None,
        Some(i) => Some(i + 1),
    }
}

fn describe_coin(coin: &Coin) {
    match coin {
        Coin::Penny => println!("1센트"),
        Coin::Nickel => println!("5센트"),
        Coin::Dime => println!("10센트"),
        Coin::Quarter(state) => println!("25센트 ({:?})", state),
    }
}
