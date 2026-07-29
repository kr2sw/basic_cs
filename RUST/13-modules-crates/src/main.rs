// 13-modules-crates
// 모듈 시스템: mod, pub, use, self, super

// 모듈 선언 (math/mod.rs 파일)
mod math;

// 하위 모듈의 특정 항목 가져오기
use math::advanced::factorial;
use math::add;

// 별칭 사용 (as)
use math::advanced::power as pow;

fn main() {
    // 기본 모듈 함수
    let sum = add(10, 20);
    println!("add(10, 20) = {}", sum);

    let diff = math::subtract(30, 15);
    println!("subtract(30, 15) = {}", diff);

    // pub(crate) 함수 - 같은 크레이트라서 접근 가능
    let prod = math::multiply(6, 7);
    println!("multiply(6, 7) = {}", prod);

    // use로 가져온 함수
    let fact = factorial(5);
    println!("factorial(5) = {}", fact);

    // as 별칭
    let p = pow(2, 10);
    println!("power(2, 10) = {}", p);

    // 재익스포트 사용 (pub use)
    let fact2 = math::my_factorial(6);
    println!("my_factorial(6) = {}", fact2);

    // self/super 사용 예제 (pub(super)는 부모 모듈에서 접근 가능)
    // internal_helper는 pub(super)이므로 math 모듈 내부에서 사용됨
    let _ = math::advanced::factorial(3); // advanced 모듈 사용

    // --- 외부 크레이트: serde_json ---
    use serde_json::{json, Value};

    // JSON 생성
    let data = json!({
        "name": "Rust",
        "year": 2015,
        "versions": ["2015", "2018", "2021", "2024"],
        "features": {
            "safety": true,
            "speed": true
        }
    });
    println!("JSON: {}", data);

    // JSON 파싱
    let json_str = r#"{"lesson": "modules", "completed": true}"#;
    let parsed: Value = serde_json::from_str(json_str).unwrap();
    println!("파싱된 lesson: {}", parsed["lesson"]);
    println!("completed: {}", parsed["completed"]);
}
