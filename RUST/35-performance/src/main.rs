// 35: 성능 최적화 — 벤치마킹, SIMD 개념, 최적화

use std::time::Instant;

// === 1. 벤치마킹 헬퍼 ===
fn bench<F>(name: &str, iters: usize, mut f: F)
where
    F: FnMut(),
{
    let start = Instant::now();
    for _ in 0..iters {
        f();
    }
    let elapsed = start.elapsed();
    println!(
        "{name:<25} {iters}회: {:?} ({:.2} ns/회)",
        elapsed,
        elapsed.as_nanos() as f64 / iters as f64
    );
}

// === 2. 비교: sum vs fold vs iterator ===
const DATA: [i32; 1000] = [7; 1000];

fn sum_loop() -> i64 {
    let mut s: i64 = 0;
    for &v in DATA.iter() {
        s += v as i64;
    }
    s
}

fn sum_fold() -> i64 {
    DATA.iter().fold(0i64, |acc, v| acc + *v as i64)
}

// === 3. SIMD 개념 재현 (청크로 병렬 누적) ===
fn sum_simd_like() -> i64 {
    // 4개씩 묶어 누적하는 "SIMD 느낌" (실제 SIMD는 아님)
    let mut acc = 0i64;
    let mut i = 0;
    while i + 4 <= DATA.len() {
        acc += DATA[i] as i64 + DATA[i + 1] as i64 + DATA[i + 2] as i64 + DATA[i + 3] as i64;
        i += 4;
    }
    while i < DATA.len() {
        acc += DATA[i] as i64;
        i += 1;
    }
    acc
}

// === 4. 문자열 최적화: String vs &str 누적 ===
fn concat_string() -> String {
    let mut s = String::new();
    for i in 0..1000 {
        s.push_str(&i.to_string());
        s.push(',');
    }
    s
}

fn concat_capacity() -> String {
    let mut s = String::with_capacity(1000 * 4);
    for i in 0..1000 {
        s.push_str(&i.to_string());
        s.push(',');
    }
    s
}

// === 5. 검색 최적화 ===
fn linear_search(data: &[i32], target: i32) -> Option<usize> {
    data.iter().position(|&v| v == target)
}

// === 6. Caching 최적화 개념 ===
struct FibCache {
    cache: Vec<u64>,
}

impl FibCache {
    fn new() -> Self {
        FibCache { cache: vec![0, 1] }
    }

    fn fib(&mut self, n: usize) -> u64 {
        if let Some(&v) = self.cache.get(n) {
            return v;
        }
        let v = self.fib(n - 1) + self.fib(n - 2);
        self.cache.push(v);
        v
    }
}

fn main() {
    println!("=== 루프 vs 반복자 (1000개 합) ===");
    bench("loop", 100_000, || { std::hint::black_box(sum_loop()); });
    bench("fold", 100_000, || { std::hint::black_box(sum_fold()); });
    bench("simd-like", 100_000, || { std::hint::black_box(sum_simd_like()); });

    println!("\n=== 문자열 (용량 예약 전후) ===");
    bench("String push", 100, || { std::hint::black_box(concat_string()); });
    bench("with_capacity", 100, || { std::hint::black_box(concat_capacity()); });

    println!("\n=== 이진 탐색 (정렬된 데이터) ===");
    let mut sorted: Vec<i32> = (0..100_000).collect();
    sorted.sort();
    bench("linear_search", 100_000, || {
        std::hint::black_box(linear_search(&sorted, 99_999));
    });

    println!("\n=== 캐시된 피보나치 ===");
    let mut fc = FibCache::new();
    let start = Instant::now();
    println!("fib(40) = {}", fc.fib(40));
    println!("재귀+캐시: {:?}", start.elapsed());

    println!("\n=== 최적화 팁 ===");
    println!("1. cargo build --release");
    println!("2. 프로파일러(perf)로 병목 지점 찾기");
    println!("3. 캐시/반복자/메모리 레이아웃 최적화");
}
