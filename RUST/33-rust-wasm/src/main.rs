// 33: Rust + WASM — wasm-bindgen 개념
//
// 실제 WASM 배포는 wasm-bindgen 크레이트 + wasm32 타깃 빌드가 필요합니다.
// 여기서는 JS로 내보낼 함수를 순수 Rust로 작성해 두고,
// 데스크톱에서는 시뮬레이션으로 동작을 확인합니다.

// === 1. WASM에서 내보낼 함수들 (wasm-bindgen용) ===
// 사용 시:
//   use wasm_bindgen::prelude::*;
//   #[wasm_bindgen]
pub fn add(a: i32, b: i32) -> i32 {
    a + b
}

//   #[wasm_bindgen]
pub fn fib(n: u32) -> u64 {
    match n {
        0 => 0,
        1 => 1,
        _ => fib(n - 1) + fib(n - 2),
    }
}

//   #[wasm_bindgen]
pub fn fizzbuzz(n: u32) -> String {
    match (n % 3, n % 5) {
        (0, 0) => "FizzBuzz".into(),
        (0, _) => "Fizz".into(),
        (_, 0) => "Buzz".into(),
        _ => n.to_string(),
    }
}

// === 2. 문자열 반환 함수 (JS에서 표시용) ===
//   #[wasm_bindgen]
pub fn greet(name: &str) -> String {
    format!("Hello, {}! Rust에서 만든 인사입니다.", name)
}

// === 3. 구조체도 내보낼 수 있음 ===
pub struct Counter {
    value: i32,
}

//   #[wasm_bindgen]
impl Counter {
    //   #[wasm_bindgen(constructor)]
    pub fn new() -> Self {
        Counter { value: 0 }
    }

    pub fn increment(&mut self) -> i32 {
        self.value += 1;
        self.value
    }

    pub fn get(&self) -> i32 {
        self.value
    }
}

fn main() {
    println!("=== Rust WASM 시뮬레이션 ===");

    // JS에서 add(1, 2) 호출되는 것과 동일
    println!("add(1, 2) = {}", add(1, 2));

    println!("fib(10) = {}", fib(10));

    for n in 1..=15 {
        print!("{} ", fizzbuzz(n));
    }
    println!();

    println!("{}", greet("WASM"));

    let mut c = Counter::new();
    println!("counter: {}, {}", c.increment(), c.increment());

    println!("\n=== 실제 배포 단계 ===");
    println!("1. rustup target add wasm32-unknown-unknown");
    println!("2. cargo build --release --target wasm32-unknown-unknown");
    println!("3. wasm-bindgen ... --out-dir pkg");
    println!("4. JS에서 import 후 호출");
}
