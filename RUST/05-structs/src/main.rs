//! 05-structs: 구조체 정의, 메서드, 연관 함수, derive 매크로

// 디버그 출력, 복제, 부분 동등 비교를 위한 derive 매크로
#[derive(Debug, Clone, PartialEq)]
struct User {
    username: String,
    email: String,
    sign_in_count: u64,
    active: bool,
}

// Tuple Struct (튜플 구조체) - 필드에 이름 없음
#[derive(Debug, Clone, Copy, PartialEq)]
struct Color(i32, i32, i32);

#[derive(Debug, Clone, Copy, PartialEq)]
struct Point(i32, i32, i32);

// Unit Struct (유닛 구조체) - 필드 없음
#[derive(Debug)]
struct AlwaysEqual;

// Rectangle - 메서드 예제
#[derive(Debug, Clone, Copy, PartialEq)]
struct Rectangle {
    width: u32,
    height: u32,
}

// Rectangle의 impl 블록 - 연관 함수와 메서드
impl Rectangle {
    // 메서드 (method) - &self를 받음
    fn area(&self) -> u32 {
        self.width * self.height
    }

    fn width(&self) -> bool {
        self.width > 0
    }

    fn can_hold(&self, other: &Rectangle) -> bool {
        self.width > other.width && self.height > other.height
    }

    // 연관 함수 (associated function) - self 없음, 생성자 역할
    fn square(size: u32) -> Self {
        Self {
            width: size,
            height: size,
        }
    }

    fn new(width: u32, height: u32) -> Self {
        Self { width, height }
    }
}

// 여러 impl 블록 가능
impl Rectangle {
    fn is_square(&self) -> bool {
        self.width == self.height
    }
}

fn main() {
    // 1. 구조체 생성
    let user1 = User {
        username: String::from("rustacean"),
        email: String::from("rust@example.com"),
        sign_in_count: 1,
        active: true,
    };
    println!("User 구조체: {:?}", user1);
    println!("이름: {}, 이메일: {}", user1.username, user1.email);

    // 2. 가변 구조체
    let mut user2 = User {
        username: String::from("coder"),
        email: String::from("coder@example.com"),
        sign_in_count: 0,
        active: false,
    };
    user2.email = String::from("new_email@example.com");
    user2.active = true;
    println!("수정된 user2: {:?}", user2);

    // 3. 함수로 구조체 생성
    let user3 = build_user(
        String::from("alice"),
        String::from("alice@example.com"),
    );
    println!("기본 sign_in_count로 생성: {:?}", user3);

    // 4. Struct Update Syntax (..)
    let user4 = User {
        username: String::from("bob"),
        email: String::from("bob@example.com"),
        ..user2  // 나머지 필드는 user2에서 복사
    };
    println!("업데이트 문법으로 생성: {:?}", user4);

    // 5. Tuple Struct 사용
    let black = Color(0, 0, 0);
    let origin = Point(0, 0, 0);
    println!("Color: {:?}", black);
    println!("Point: {:?}", origin);
    println!("Color.0: {}, .1: {}, .2: {}", black.0, black.1, black.2);

    // Tuple Struct 분해
    let Color(r, g, b) = black;
    println!("분해: R={}, G={}, B={}", r, g, b);

    // 6. Unit Struct
    let equal = AlwaysEqual;
    println!("Unit struct: {:?}", equal);

    // 7. 메서드 호출
    let rect = Rectangle {
        width: 30,
        height: 50,
    };
    println!("rect: {:?}", rect);
    println!("면적: {}", rect.area());
    println!("width > 0: {}", rect.width());

    // 8. 메서드 with 참조
    let rect1 = Rectangle {
        width: 40,
        height: 60,
    };
    let rect2 = Rectangle {
        width: 20,
        height: 30,
    };
    println!("rect1이 rect2를 포함? {}", rect1.can_hold(&rect2));

    // 9. 연관 함수 호출 (::)
    let square = Rectangle::square(25);
    println!("정사각형: {:?}, 면적: {}", square, square.area());

    let rect3 = Rectangle::new(15, 35);
    println!("new로 생성: {:?}, 정사각형? {}", rect3, rect3.is_square());

    // 10. Debug derive - pretty print
    println!("예쁜 출력:\n{:#?}", rect);

    // 11. Clone, PartialEq 테스트
    let rect4 = Rectangle::new(15, 35);
    println!("rect3 == rect4? {}", rect3 == rect4);  // PartialEq

    let rect5 = rect4.clone();  // Clone
    println!("clone: {:?}", rect5);

    // 12. 구조체 패턴 매칭
    let user5 = User {
        username: String::from("pattern"),
        email: String::from("pattern@example.com"),
        sign_in_count: 42,
        active: true,
    };

    // let 분해
    let User { username, email, .. } = user5;
    println!("패턴 분해: {}, {}", username, email);
    // user5.username은 이동됨! (username이 String)
    // 나머지 필드는 drop (email도 String)
    // println!("{:?}", user5);  // 부분 이동되어 사용 불가

    // 참조로 분해 (이동 방지)
    let user6 = User {
        username: String::from("ref_pattern"),
        email: String::from("ref@example.com"),
        sign_in_count: 7,
        active: false,
    };
    let User {
        ref username,
        ref email,
        ..
    } = user6;
    println!("참조 분해: {}, {}", username, email);
    println!("user6 여전히 유효: {:?}", user6);
}

// Helper 함수
fn build_user(username: String, email: String) -> User {
    User {
        username,           // 필드명과 변수명이 같으면 축약
        email,              // email: email -> email
        sign_in_count: 0,   // 기본값
        active: true,
    }
}
