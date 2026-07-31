use wasm_bindgen::prelude::*;

/// 문자열에서 정수들을 파싱해 반환 (JS에서는 Uint32Array)
/// 입력 예: "a1 b22 c333" -> [1, 22, 333]
#[wasm_bindgen]
pub fn parse_ints(input: &str) -> Vec<u32> {
    input
        .split(|c: char| !c.is_ascii_digit())
        .filter(|s| !s.is_empty())
        .filter_map(|s| s.parse::<u32>().ok())
        .collect()
}

/// 단어 개수 세기 (빠른 파서 데모)
#[wasm_bindgen]
pub fn count_words(input: &str) -> usize {
    input.split_whitespace().count()
}

/// "key: value" 라인들을 파싱해 JS 객체로 변환
#[wasm_bindgen]
pub fn parse_key_value(input: &str) -> js_sys::Object {
    let obj = js_sys::Object::new();
    for line in input.lines() {
        let mut parts = line.splitn(2, ':');
        if let (Some(k), Some(v)) = (parts.next(), parts.next()) {
            let _ = js_sys::Reflect::set(
                &obj,
                &JsValue::from_str(k.trim()),
                &JsValue::from_str(v.trim()),
            );
        }
    }
    obj
}

/// 벤치마크용 CPU 작업: limit 이하 소수의 합
/// 순수 JS 구현과 비교해봅시다.
#[wasm_bindgen]
pub fn sum_primes(limit: u32) -> u64 {
    let mut sum: u64 = 0;
    for n in 2..=limit {
        let mut is_prime = true;
        let mut d = 2;
        while d * d <= n {
            if n % d == 0 {
                is_prime = false;
                break;
            }
            d += 1;
        }
        if is_prime {
            sum += n as u64;
        }
    }
    sum
}
