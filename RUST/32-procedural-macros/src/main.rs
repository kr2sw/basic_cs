// 32: 프로시저 매크로 — derive 매크로 개념
//
// derive 매크로는 반복적인 트레잇 구현을 자동 생성합니다.
// 여기서는 macro_rules!와 제네릭을 이용해 그 원리를 재현합니다.

use std::fmt;

// === 1. macro_rules! 로 "derive" 흉내 ===
macro_rules! derive_display_like {
    ($name:ident { $($field:ident),* }) => {
        impl fmt::Display for $name {
            fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
                write!(f, "{} {{", stringify!($name))?;
                $(
                    write!(f, "{}: {:?} ", stringify!($field), self.$field)?;
                )*
                write!(f, "}}")
            }
        }
    };
}

struct Book {
    title: String,
    pages: u32,
}

derive_display_like!(Book { title, pages });

struct Point3 {
    x: i32,
    y: i32,
    z: i32,
}

derive_display_like!(Point3 { x, y, z });

// === 2. 제네릭 Blanket 구현으로 derive 유사 효과 ===
trait ToCsv {
    fn to_csv_row(&self) -> String;
}

// 원시 타입용 구현
impl ToCsv for &str {
    fn to_csv_row(&self) -> String {
        self.to_string()
    }
}

impl ToCsv for i32 {
    fn to_csv_row(&self) -> String {
        self.to_string()
    }
}

// Vec<T>를 CSV 한 줄로 (구조체가 자동으로 얻는 구현 흉내)
impl<T: ToCsv> ToCsv for Vec<T> {
    fn to_csv_row(&self) -> String {
        self.iter().map(|v| v.to_csv_row()).collect::<Vec<_>>().join(",")
    }
}

// === 3. derive 매크로 생성 코드가 하는 일 ===
// #[derive(Debug)] 는 대략 다음 코드를 생성합니다.
// impl fmt::Debug for Book {
//     fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
//         f.debug_struct("Book")
//          .field("title", &self.title)
//          .field("pages", &self.pages)
//          .finish()
//     }
// }
// 아래는 그것을 수동으로 보여주는 함수입니다.
fn manual_debug_demo() {
    let book = Book { title: "Rust".into(), pages: 300 };
    let mut s = String::from("Book { ");
    s.push_str(&format!("title: {:?}, ", book.title));
    s.push_str(&format!("pages: {:?} }}", book.pages));
    println!("수동 Debug: {s}");
}

fn main() {
    // 매크로로 생성된 Display 구현
    let book = Book { title: "러스트 입문".into(), pages: 320 };
    println!("derive_display_like: {book}");

    let p = Point3 { x: 1, y: 2, z: 3 };
    println!("Point3: {p}");

    // Blanket 구현
    let row = vec!["kim", "lee", "park"];
    println!("CSV: {}", row.to_csv_row());

    let nums = vec![10, 20, 30];
    println!("CSV: {}", nums.to_csv_row());

    manual_debug_demo();

    println!("\n참고: 실제 proc-macro는 별도 크레이트로 작성합니다.");
    println!("-> proc-macro = true 설정 + proc_macro::TokenStream 처리");
}
