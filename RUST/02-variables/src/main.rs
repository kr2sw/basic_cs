//! 02-variables: 변수 선언, 가변성, 섀도잉, 자료형
//!
//! 러스트의 변수 시스템을 상세히 다룹니다.

// 상수 (const) - 컴파일 타임에 결정, 반드시 타입 명시
const MAX_POINTS: u32 = 100_000;
const GRAVITY: f64 = 9.80665;

// 정적 변수 (static) - 고정된 메모리 주소, 'static 수명
static APP_NAME: &str = "러스트 학습 앱";

fn main() {
    // 1. let 바인딩 (불변, immutable)
    let x = 5;
    // x = 6;  // 컴파일 에러! 불변 변수는 재할당 불가
    println!("불변 변수 x: {}", x);

    // 2. let mut (가변, mutable)
    let mut y = 10;
    println!("가변 변수 y (초기값): {}", y);
    y = 20;
    println!("가변 변수 y (변경 후): {}", y);

    // 3. 섀도잉 (Shadowing) - 같은 이름으로 새 변수 선언
    let z = 100;
    let z = z + 50;  // 이전 z를 가림
    {
        let z = 999;  // 블록 내에서만 유효
        println!("블록 내 섀도잉 z: {}", z);
    }
    println!("블록 밖 z (다시 원래 값): {}", z);

    // 섀도잉으로 타입 변경 가능
    let shadowed = "문자열";
    let shadowed = shadowed.len();  // usize로 타입 변경
    println!("섀도잉으로 타입 변경: {}", shadowed);

    // 4. 정수 타입 (i8, i16, i32, i64, i128, u8, u16, u32, u64, u128)
    let a: i8 = 127;
    let b: i16 = 32_767;
    let c: i32 = 2_147_483_647;
    let d: i64 = 9_223_372_036_854_775_807;
    let e: i128 = 170_141_183_460_469_231_731_687_303_715_884_105_727;
    let f: u8 = 255;       // 부호 없음
    let g: usize = 64;     // 플랫폼 의존적 (64비트 시스템에서 64비트)

    println!("i8: {}, i16: {}, i32: {}, i64: {}, i128: {}", a, b, c, d, e);
    println!("u8: {}, usize: {}", f, g);

    // 5. 부동소수점 타입 (f32, f64)
    let h: f32 = 3.14;     // 32비트 단정도
    let i: f64 = 2.718281828459045;  // 64비트 배정도 (기본)
    // IEEE 754 특수값
    let inf = f64::INFINITY;
    let neg_inf = f64::NEG_INFINITY;
    let nan = f64::NAN;

    println!("f32: {}, f64: {}", h, i);
    println!("무한대: {}, 음의 무한대: {}, NaN: {}", inf, neg_inf, nan);

    // 6. bool
    let is_true: bool = true;
    let is_false: bool = false;
    let from_expr = 10 > 5;
    println!("bool: {}, {}, {}", is_true, is_false, from_expr);

    // 7. char (유니코드, 4바이트)
    let heart_eyed_cat: char = '😻';
    let korean: char = '한';
    let letter: char = 'a';
    println!("char: {}, {}, {}", heart_eyed_cat, korean, letter);

    // 8. 튜플 (Tuple) - 서로 다른 타입을 묶음
    let tuple: (i32, f64, char) = (42, 3.14, 'R');
    let (t_x, t_y, t_z) = tuple;  // 구조 분해 (destructuring)
    println!("튜플 전체: {:?}", tuple);
    println!("튜플 인덱스: {}, {}, {}", tuple.0, tuple.1, tuple.2);
    println!("튜플 분해: {}, {}, {}", t_x, t_y, t_z);

    // 9. 배열 (Array) - 같은 타입, 고정 길이
    let array: [i32; 5] = [1, 2, 3, 4, 5];
    let zeros = [0; 10];  // [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    println!("배열: {:?}", array);
    println!("배열 인덱스: {}", array[2]);  // 3
    println!("0으로 채운 배열: {:?}", zeros);

    // 배열 길이 확인
    println!("배열 길이: {}", array.len());

    // 10. const와 static 사용
    println!("상수 MAX_POINTS: {}", MAX_POINTS);
    println!("상수 GRAVITY: {}", GRAVITY);
    println!("정적 변수 APP_NAME: {}", APP_NAME);

    // 가변 static은 Rust 2024에서 raw pointer로만 접근 가능
    // (static_mut_refs가 기본 deny됨)
    static mut COUNTER: u32 = 0;
    unsafe {
        let ptr = &raw mut COUNTER;
        *ptr += 1;
        println!("가변 static COUNTER: {}", *ptr);
    }

    // 11. 타입 추론 (type inference)
    let inferred = 42;              // i32로 추론
    let inferred_float = 3.14;     // f64로 추론
    // let mut vec = Vec::new();  // 타입을 알 수 없어 에러 (주석 해제시)
    let mut vec: Vec<i32> = Vec::new();  // 명시하면 OK
    vec.push(1);
    println!("타입 추론: {}, {}, {:?}", inferred, inferred_float, vec);

    // 12. 바인딩 초기화
    let uninitialized: i32;  // 선언만
    // println!("{}", uninitialized);  // 컴파일 에러! 초기화 전 사용
    uninitialized = 99;  // 초기화
    println!("초기화 후 사용: {}", uninitialized);
}
