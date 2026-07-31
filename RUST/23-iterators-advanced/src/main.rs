// 23: 반복자 심화 — 어댑터 체인, Iterator 직접 구현

// === 1. Iterator 직접 구현 ===
struct Fibonacci {
    a: u64,
    b: u64,
}

impl Fibonacci {
    fn new() -> Self {
        Fibonacci { a: 0, b: 1 }
    }
}

impl Iterator for Fibonacci {
    type Item = u64;
    fn next(&mut self) -> Option<Self::Item> {
        let next = self.a + self.b;
        self.a = self.b;
        self.b = next;
        Some(self.a)
    }
}

// === 2. 무한 반복자 (위치 기반) ===
struct PowersOfTwo {
    exp: u32,
}

impl Iterator for PowersOfTwo {
    type Item = u64;
    fn next(&mut self) -> Option<Self::Item> {
        let result = 2u64.checked_pow(self.exp)?;
        self.exp += 1;
        Some(result)
    }
}

// === 3. 상태를 가진 반복자: 윈도우 합계 ===
struct WindowSum<I>
where
    I: Iterator<Item = i32>,
{
    inner: I,
    window: Vec<i32>,
}

impl<I> Iterator for WindowSum<I>
where
    I: Iterator<Item = i32>,
{
    type Item = i32;
    fn next(&mut self) -> Option<Self::Item> {
        let v = self.inner.next()?;
        self.window.push(v);
        if self.window.len() > 3 {
            self.window.remove(0);
        }
        Some(self.window.iter().sum())
    }
}

// === 4. 어댑터 체인 활용 ===
struct Student {
    name: &'static str,
    score: i32,
}

fn main() {
    // 커스텀 반복자
    let fib: Vec<u64> = Fibonacci::new().take(10).collect();
    println!("피보나치: {:?}", fib);

    let pow: Vec<u64> = PowersOfTwo { exp: 0 }.take(6).collect();
    println!("2의 거듭제곱: {:?}", pow);

    // WindowSum 체인
    let sums: Vec<i32> = WindowSum {
        inner: vec![1, 2, 3, 4, 5].into_iter(),
        window: Vec::new(),
    }
    .collect();
    println!("윈도우 합: {:?}", sums);

    // filter_map
    let nums = ["1", "x", "42", "12.5", "7"];
    let parsed: Vec<i32> = nums.iter().filter_map(|s| s.parse().ok()).collect();
    println!("filter_map: {:?}", parsed);

    // flat_map
    let words = ["hello world", "rust is fun"];
    let flattened: Vec<&str> = words.iter().flat_map(|w| w.split_whitespace()).collect();
    println!("flat_map: {:?}", flattened);

    // zip + enumerate
    let names = ["kim", "lee", "park"];
    let scores = [90, 85, 95];
    let ranked: Vec<(usize, (&str, &i32))> = names.iter().copied().zip(scores.iter()).enumerate().collect();
    println!("zip + enumerate: {:?}", ranked);

    // take_while / skip_while
    let v = [1, 2, 3, 4, 1, 2];
    let taken: Vec<i32> = v.iter().take_while(|&&x| x < 4).copied().collect();
    let skipped: Vec<i32> = v.iter().skip_while(|&&x| x < 4).copied().collect();
    println!("take_while: {:?}", taken);
    println!("skip_while: {:?}", skipped);

    // fold
    let factorial = (1..=6).fold(1, |acc, n| acc * n);
    println!("6! = {}", factorial);

    // 최고 점수 학생 찾기
    let students = [
        Student { name: "kim", score: 90 },
        Student { name: "lee", score: 85 },
        Student { name: "park", score: 100 },
    ];
    let best = students.iter().max_by_key(|s| s.score).unwrap();
    println!("최고 점수: {} ({})", best.name, best.score);

    // group_by는 nightly 이므로 sorted + dedup으로 비슷하게
    let mut v = vec![3, 1, 2, 1, 3, 2, 2];
    v.sort();
    v.dedup();
    println!("정렬 후 dedup: {:?}", v);
}
