//! 07-collections: Vec, HashMap, HashSet, String, VecDeque 등 컬렉션
//!
//! 러스트 표준 라이브러리의 주요 컬렉션 타입을 다룹니다.

use std::collections::{HashMap, HashSet, VecDeque};

fn main() {
    // =========================================================
    // 1. Vec<T> (벡터) - 가변 배열
    // =========================================================
    println!("=== Vec<T> ===");

    // 생성
    let mut v: Vec<i32> = Vec::new();
    let mut v2 = vec![1, 2, 3, 4, 5];  // vec! 매크로

    // 추가
    v.push(10);
    v.push(20);
    v.push(30);

    println!("v: {:?}", v);
    println!("v2: {:?}", v2);

    // 접근
    let third: &i32 = &v2[2];  // 인덱싱 (범위 초과시 panic)
    println!("v2[2]: {}", third);

    let third_option: Option<&i32> = v2.get(2);  // 안전한 접근
    match third_option {
        Some(val) => println!("get(2): {}", val),
        None => println!("인덱스 범위 초과"),
    }

    // get으로 안전하게 접근
    let out_of_range = v2.get(100);
    println!("get(100): {:?}", out_of_range);  // None

    // 제거
    let last = v.pop();
    println!("pop: {:?}, v: {:?}", last, v);

    // 반복
    print!("for 반복: ");
    for i in &v2 {
        print!("{} ", i);
    }
    println!();

    // 가변 반복
    for i in &mut v2 {
        *i *= 2;
    }
    println!("가변 반복 후 v2: {:?}", v2);

    // 다양한 메서드
    let mut nums = vec![3, 1, 4, 1, 5, 9, 2, 6, 5];
    nums.sort();
    println!("정렬: {:?}", nums);
    nums.reverse();
    println!("역순: {:?}", nums);
    nums.dedup();
    println!("중복 제거: {:?}", nums);
    println!("길이: {}, 비었나? {}", nums.len(), nums.is_empty());

    // =========================================================
    // 2. HashMap<K, V> (해시맵) - 키-값 쌍
    // =========================================================
    println!("\n=== HashMap<K, V> ===");

    let mut scores = HashMap::new();
    scores.insert(String::from("Blue"), 100);
    scores.insert(String::from("Red"), 200);
    scores.insert(String::from("Blue"), 250);  // 덮어씀

    println!("scores: {:?}", scores);

    // get
    let team = String::from("Blue");
    let blue_score = scores.get(&team);
    println!("Blue 점수: {:?}", blue_score);

    // iterate
    for (key, value) in &scores {
        println!("{}: {}", key, value);
    }

    // entry API - 키가 없을 때만 삽입
    let mut map = HashMap::new();
    map.insert("a", 1);
    map.entry("b").or_insert(2);
    map.entry("a").or_insert(99);  // a가 이미 있으므로 무시
    println!("entry API: {:?}", map);

    // or_insert_with
    map.entry("c").or_insert_with(|| 3 * 3);
    println!("or_insert_with: {:?}", map);

    // 키가 존재하는지 확인 후 업데이트
    let text = "hello world hello rust world";
    let mut word_count = HashMap::new();
    for word in text.split_whitespace() {
        let count = word_count.entry(word).or_insert(0);
        *count += 1;
    }
    println!("단어 빈도: {:?}", word_count);

    // =========================================================
    // 3. HashSet<T> (해시셋) - 중복 없는 집합
    // =========================================================
    println!("\n=== HashSet<T> ===");

    let mut set_a = HashSet::new();
    set_a.insert(1);
    set_a.insert(2);
    set_a.insert(3);
    set_a.insert(3);  // 중복, 무시됨
    println!("set_a ({:?}): {:?}", set_a.len(), set_a);
    println!("contains 2? {}", set_a.contains(&2));
    println!("contains 5? {}", set_a.contains(&5));

    // 집합 연산
    let set_b: HashSet<_> = vec![2, 3, 4, 5].into_iter().collect();

    // 합집합 (union)
    let union: HashSet<_> = set_a.union(&set_b).copied().collect();
    println!("합집합: {:?}", union);

    // 교집합 (intersection)
    let intersection: HashSet<_> = set_a.intersection(&set_b).collect();
    println!("교집합: {:?}", intersection);

    // 차집합 (difference)
    let diff: HashSet<_> = set_a.difference(&set_b).collect();
    println!("차집합 (A-B): {:?}", diff);

    // 대칭 차집합 (symmetric_difference)
    let sym_diff = set_a.symmetric_difference(&set_b);
    println!("대칭 차집합: {:?}", sym_diff);

    // =========================================================
    // 4. String - 러스트의 문자열
    // =========================================================
    println!("\n=== String ===");

    let mut s = String::new();
    s.push_str("hello");
    s.push(' ');
    s.push_str("world");
    println!("s: {}", s);

    // 문자열 연결
    let s1 = String::from("Hello, ");
    let s2 = String::from("Rust!");
    // let s3 = s1 + &s2;  // s1 이동됨
    let s3 = format!("{}{}", s1, s2);  // format!은 소유권 유지
    println!("format!: {}", s3);
    println!("s1 여전히 유효: {}", s1);

    // 문자열 슬라이싱 (바이트 단위, 주의)
    let hello = "안녕하세요";
    let slice = &hello[0..6];  // '안녕' (UTF-8: 6바이트)
    println!("슬라이스: {}", slice);

    // 문자열 반복
    println!("char 반복:");
    for c in "Rust".chars() {
        print!("{} ", c);
    }
    println!();

    println!("바이트 반복:");
    for b in "Rust".bytes() {
        print!("{} ", b);
    }
    println!();

    // 문자열 메서드
    let text = "  Hello, Rust World!  ";
    println!("trim: '{}'", text.trim());
    println!("to_lowercase: '{}'", text.to_lowercase());
    println!("to_uppercase: '{}'", text.to_uppercase());
    println!("contains 'Rust': {}", text.contains("Rust"));
    println!("replace: '{}'", text.replace("Rust", "러스트"));

    let words: Vec<&str> = text.trim().split_whitespace().collect();
    println!("split: {:?}", words);

    // &str -> String
    let str_slice: &str = "&str";
    let string_from_str: String = str_slice.to_string();
    println!("&str -> String: {}", string_from_str);

    // =========================================================
    // 5. VecDeque<T> (덱) - 양방향 큐
    // =========================================================
    println!("\n=== VecDeque<T> ===");

    let mut deque: VecDeque<i32> = VecDeque::new();
    deque.push_back(1);    // 뒤에 추가
    deque.push_back(2);
    deque.push_front(0);   // 앞에 추가
    deque.push_front(-1);

    println!("deque: {:?}", deque);
    println!("front: {:?}", deque.front());   // 앞 요소 참조
    println!("back: {:?}", deque.back());     // 뒤 요소 참조

    let popped_front = deque.pop_front();  // 앞 제거
    let popped_back = deque.pop_back();    // 뒤 제거
    println!("pop_front: {:?}, pop_back: {:?}", popped_front, popped_back);
    println!("deque after pops: {:?}", deque);

    // 회전 (rotation)
    let mut deck = VecDeque::from([1, 2, 3, 4, 5]);
    deck.rotate_left(2);
    println!("rotate_left(2): {:?}", deck);
    deck.rotate_right(1);
    println!("rotate_right(1): {:?}", deck);

    // 반복
    print!("deque iterate: ");
    for item in &deck {
        print!("{} ", item);
    }
    println!();

    // =========================================================
    // 6. 벡터와 다양한 타입 (enum으로 해결)
    // =========================================================
    println!("\n=== 이종 컬렉션 (enum) ===");

    #[derive(Debug)]
    enum SpreadsheetCell {
        Int(i32),
        Float(f64),
        Text(String),
    }

    let row = vec![
        SpreadsheetCell::Int(42),
        SpreadsheetCell::Float(3.14),
        SpreadsheetCell::Text(String::from("러스트")),
    ];

    for cell in &row {
        match cell {
            SpreadsheetCell::Int(val) => println!("정수: {}", val),
            SpreadsheetCell::Float(val) => println!("실수: {}", val),
            SpreadsheetCell::Text(val) => println!("문자열: {}", val),
        }
    }

    // =========================================================
    // 7. HashMap with complex types
    // =========================================================
    println!("\n=== 복합 HashMap ===");

    let mut users: HashMap<String, Vec<i32>> = HashMap::new();
    users.entry(String::from("alice")).or_default().push(100);
    users.entry(String::from("alice")).or_default().push(90);
    users.entry(String::from("bob")).or_default().push(80);
    println!("users: {:?}", users);

    // from arrays/tuples
    let pairs = vec![("x", 1), ("y", 2), ("z", 3)];
    let map_from_pairs: HashMap<&str, i32> = pairs.into_iter().collect();
    println!("튜플에서 HashMap: {:?}", map_from_pairs);
}
