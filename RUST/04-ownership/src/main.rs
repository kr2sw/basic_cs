//! 04-ownership: 소유권, 이동, 복제, 참조, 슬라이스
//!
//! 러스트의 가장 핵심 개념인 소유권 시스템을 다룹니다.

fn main() {
    // =========================================================
    // 1. 소유권 규칙 (Ownership Rules)
    //    - 각 값은 소유자(owner)가 있다.
    //    - 한 번에 하나의 소유자만 존재한다.
    //    - 소유자가 범위를 벗어나면 값은 drop된다.
    // =========================================================
    {
        let s = String::from("안녕");  // s가 소유
        println!("s: {}", s);
    }  // s가 범위를 벗어나 메모리 해제 (drop)
    // println!("{}", s);  // 컴파일 에러!

    // =========================================================
    // 2. 이동 (Move Semantics)
    //    - Heap 데이터는 소유권이 이동함
    //    - Stack 데이터는 Copy 가능
    // =========================================================
    let s1 = String::from("hello");
    let s2 = s1;  // s1의 소유권이 s2로 이동
    // println!("{}", s1);  // 컴파일 에러! s1은 유효하지 않음
    println!("s2: {}", s2);

    // 함수로 소유권 이동
    let s3 = String::from("world");
    takes_ownership(s3);
    // println!("{}", s3);  // 에러! 소유권이 이동됨

    // 함수에서 소유권 반환
    let s4 = gives_ownership();
    println!("s4: {}", s4);

    let s5 = String::from("hello");
    let s6 = takes_and_gives_back(s5);
    // println!("{}", s5);  // 에러!
    println!("s6: {}", s6);

    // =========================================================
    // 3. Clone 트레이트 (깊은 복사)
    //    - Heap 데이터까지 실제로 복사
    // =========================================================
    let s7 = String::from("clone me");
    let s8 = s7.clone();  // 깊은 복사 (Heap 데이터도 복사)
    println!("s7: {}, s8: {}", s7, s8);  // 둘 다 사용 가능!

    // =========================================================
    // 4. Copy 트레이트 (스택 복사)
    //    - 정수, 실수, bool, char 등 스택 전용 타입
    //    - Copy 타입은 이동이 아닌 복사가 일어남
    // =========================================================
    let x = 42;
    let y = x;  // x는 i32 (Copy), 복사됨
    println!("x: {}, y: {}", x, y);  // 둘 다 사용 가능!

    let a = true;
    let b = a;
    println!("a: {}, b: {}", a, b);

    // Copy 트레이트를 구현한 사용자 정의 타입
    #[derive(Debug, Clone, Copy)]
    struct Point {
        x: i32,
        y: i32,
    }

    let p1 = Point { x: 10, y: 20 };
    let p2 = p1;  // Copy! (Clone + Copy 덕분에)
    println!("p1: {:?}, p2: {:?}, p1.x: {}, p1.y: {}", p1, p2, p1.x, p1.y);

    // =========================================================
    // 5. 참조 (Reference)와 대여 (Borrowing)
    //    - &T: 불변 참조 (읽기 전용 대여)
    //    - &mut T: 가변 참조 (읽기/쓰기 대여)
    //    - 규칙:
    //      1) 하나의 가변 참조 또는 여러 불변 참조 (둘 다 불가)
    //      2) 참조는 항상 유효해야 함
    // =========================================================
    let s9 = String::from("borrow me");
    let len = calculate_length(&s9);  // &s9: 불변 참조로 대여
    println!("'{}'의 길이: {}", s9, len);  // s9 계속 사용 가능!

    // 가변 참조
    let mut s10 = String::from("change me");
    change_string(&mut s10);  // &mut s10: 가변 참조
    println!("변경 후: {}", s10);

    // 가변 참조 규칙: 동시에 하나만!
    let value = String::from("test");
    let r1 = &value;     // 불변 참조
    let r2 = &value;     // 불변 참조 (여러 개 가능)
    println!("r1: {}, r2: {}", r1, r2);
    // let r3 = &mut value;  // 에러! 불변 참조가 있을 때 가변 참조 불가
    // println!("r1: {}", r1);

    // =========================================================
    // 6. 댕글링 참조 (Dangling Reference) 방지
    //    - 참조자는 항상 유효한 데이터를 가리켜야 함
    // =========================================================
    // fn dangle() -> &String {  // 컴파일 에러!
    //     let s = String::from("hello");
    //     &s  // s가 drop된 후 참조를 반환
    // }  // <- s가 drop됨

    fn no_dangle() -> String {
        let s = String::from("hello");
        s  // 소유권을 이동시켜 반환
    }
    let s11 = no_dangle();
    println!("no_dangle: {}", s11);

    // =========================================================
    // 7. 슬라이스 (Slice) 타입
    //    - 컬렉션의 일부분에 대한 참조
    //    - 소유권이 없음 (참조)
    // =========================================================
    // 문자열 슬라이스 (&str)
    let s12 = String::from("Hello, Rust!");
    let hello = &s12[0..5];   // "Hello"
    let rust = &s12[7..11];   // "Rust"
    let whole = &s12[..];     // 전체 슬라이스
    println!("슬라이스: '{}' '{}' '{}'", hello, rust, whole);

    // 문자열 리터럴은 &str
    let _s13: &str = "나는 &str입니다";

    // 배열 슬라이스
    let array = [1, 2, 3, 4, 5, 6, 7, 8];
    let slice = &array[2..5];   // [3, 4, 5]
    println!("배열 슬라이스: {:?}", slice);
    println!("슬라이스 길이: {}", slice.len());

    // 가변 슬라이스
    let mut arr2 = [10, 20, 30, 40, 50];
    let slice_mut = &mut arr2[1..4];
    slice_mut[0] = 999;  // arr2[1]을 999로 변경
    println!("가변 슬라이스로 변경 후: {:?}", arr2);

    // =========================================================
    // 8. 함수 매개변수로의 참조
    // =========================================================
    let text = String::from("러스트 소유권");
    let word = first_word(&text);
    println!("첫 단어: {}", word);
}

// --- 함수 정의 ---

// 소유권을 가져감 (이동)
fn takes_ownership(s: String) {
    println!("소유권 이동: {}", s);
}  // s가 drop됨

// 소유권을 반환
fn gives_ownership() -> String {
    String::from("새로 생성된 문자열")
}

// 받고 반환
fn takes_and_gives_back(s: String) -> String {
    s  // 소유권 반환
}

// 불변 참조로 대여 (빌림)
fn calculate_length(s: &String) -> usize {
    s.len()
}  // s가 drop되지 않음 (소유권 없음)

// 가변 참조로 대여
fn change_string(s: &mut String) {
    s.push_str(" (수정됨)");
}

// 문자열 슬라이스 반환
fn first_word(s: &str) -> &str {
    let bytes = s.as_bytes();
    for (i, &byte) in bytes.iter().enumerate() {
        if byte == b' ' {
            return &s[0..i];
        }
    }
    &s[..]
}
