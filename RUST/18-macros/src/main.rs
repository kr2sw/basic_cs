// 18-macros
// 매크로: declarative macro (macro_rules!), derive, built-in

// --- 사용자 정의 calculate! 매크로 ---
macro_rules! calculate {
    // (표현식) 패턴 매칭
    (eval $e:expr) => {
        {
            let val: usize = $e;
            println!("{} = {}", stringify!($e), val);
            val
        }
    };

    // 여러 표현식
    (eval $e:expr, $(eval $rest:expr),*) => {
        {
            calculate!(eval $e);
            $(calculate!(eval $rest);)*
        }
    };
}

// --- vec! 매크로 시뮬레이션 (std::vec! 재구현) ---
macro_rules! my_vec {
    // 빈 Vec
    () => {
        Vec::new()
    };

    // 단일 요소 반복: my_vec![5; 3]
    ($elem:expr; $n:expr) => {
        std::vec::from_elem($elem, $n)
    };

    // 여러 요소: my_vec![1, 2, 3]
    ($($x:expr),+ $(,)?) => {
        {
            let mut v = Vec::new();
            $(v.push($x);)+
            v
        }
    };
}

// --- Builder 패턴 매크로 ---
macro_rules! builder {
    ($struct_name:ident, $($field:ident: $field_type:ty),+ $(,)?) => {
        struct $struct_name {
            $($field: Option<$field_type>),+
        }

        impl $struct_name {
            fn new() -> Self {
                Self {
                    $($field: None),+
                }
            }

            $(fn $field(mut self, value: $field_type) -> Self {
                self.$field = Some(value);
                self
            })+

            fn build(self) -> Result<$struct_name, String> {
                // build 로직
                Ok(self)
            }
        }
    };
}

// Builder 매크로 사용
builder!(Config, name: String, port: u16, debug: bool);

// --- derive 매크로 (사용자 정의) ---
// #[derive(Debug, Clone, Copy, PartialEq, Eq)]
#[derive(Debug, Clone, PartialEq)]
struct Point {
    x: i32,
    y: i32,
}

fn main() {
    // --- calculate! 매크로 ---
    let sum = calculate!(eval 1 + 2);
    println!("sum = {}", sum);

    calculate!(eval 10 * 5, eval 100 / 4, eval 50 - 22);

    // --- my_vec! 매크로 ---
    let v1 = my_vec![1, 2, 3];
    println!("my_vec![1,2,3] = {:?}", v1);

    let v2 = my_vec![5; 3];
    println!("my_vec![5;3] = {:?}", v2);

    let v3: Vec<i32> = my_vec![];
    println!("my_vec![] = {:?}", v3);

    // --- builder! 매크로 ---
    let config = Config::new()
        .name("server".to_string())
        .port(8080)
        .debug(true)
        .build()
        .unwrap();
    println!("Config: name={:?}, port={:?}, debug={:?}",
        config.name, config.port, config.debug);

    // --- derive ---
    let p1 = Point { x: 10, y: 20 };
    let p2 = p1.clone();
    println!("Point: {:?}", p2);
    println!("p1 == p2: {}", p1 == p2);

    // --- 내장 매크로 ---
    println!("file!(): {}", file!());        // 현재 파일명
    println!("line!(): {}", line!());        // 현재 라인 번호
    println!("column!(): {}", column!());    // 현재 컬럼
    println!("module_path!(): {}", module_path!()); // 모듈 경로

    let v = vec![1, 2, 3];
    assert_eq!(v.len(), 3);
    assert!(v.contains(&2));
    assert_ne!(v[0], 0);

    // stringify!
    println!("stringify!(x+1): {}", stringify!(x + 1));

    // concat!
    let s = concat!("a", "b", "c");
    println!("concat!: {}", s);
}
