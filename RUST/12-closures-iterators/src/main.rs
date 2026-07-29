// 12-closures-iterators
// 클로저: 변수에 저장하거나 함수에 전달할 수 있는 익명 함수

fn main() {
    // --- 기본 클로저 문법 ---
    let add = |a, b| a + b;
    println!("add(3, 5) = {}", add(3, 5));

    let square = |x: i32| -> i32 { x * x };
    println!("square(4) = {}", square(4));

    // --- 환경 캡처 ---
    let x = 10;

    // 참조로 캡처 (&T): Fn
    let print_x = || println!("x = {}", x);
    print_x();
    print_x(); // 여러 번 호출 가능

    // 가변 참조로 캡처 (&mut T): FnMut
    let mut count = 0i32;
    let mut increment = || {
        count += 1;
        println!("count = {}", count);
    };
    increment();
    increment();

    // 소유권으로 캡처 (T): FnOnce
    let name = String::from("Rust");
    let consume = || {
        let _ = name;
        println!("name 소비됨");
    };
    consume();
    // consume(); // 컴파일 에러: name이 이미 이동됨

    // --- move 클로저 ---
    let data = vec![1, 2, 3];
    let moved = move || {
        // data의 소유권을 클로저로 이동
        println!("data: {:?}", data);
    };
    moved();
    // println!("{:?}", data); // 컴파일 에러: data가 이동됨

    // --- 클로저를 인자로 받는 함수 ---
    #[allow(dead_code)]
    fn call_fn<F: Fn()>(f: F) {
        f();
    }
    #[allow(dead_code)]
    fn call_fn_mut<F: FnMut()>(mut f: F) {
        f();
    }
    #[allow(dead_code)]
    fn call_fn_once<F: FnOnce()>(f: F) {
        f();
    }

    let msg = String::from("hello");
    let c1 = || println!("{}", msg);
    call_fn(c1);
    // call_fn은 Fn이면 FnMut, FnOnce에도 전달 가능

    // --- Iterator trait ---

    // into_iter - 소유권 이동
    let v1 = vec![1, 2, 3];
    let iter1: Vec<i32> = v1.into_iter().collect();
    println!("into_iter: {:?}", iter1);
    // println!("{:?}", v1); // v1은 이동됨

    // iter - 불변 참조
    let v2 = vec![4, 5, 6];
    let iter2: Vec<&i32> = v2.iter().collect();
    println!("iter: {:?}", iter2);
    println!("원본 유지: {:?}", v2);

    // iter_mut - 가변 참조
    let mut v3 = vec![7, 8, 9];
    v3.iter_mut().for_each(|x| *x *= 2);
    println!("iter_mut 후: {:?}", v3);

    // --- Iterator 어댑터 ---

    // map
    let numbers = vec![1, 2, 3, 4, 5];
    let doubled: Vec<i32> = numbers.iter().map(|x| x * 2).collect();
    println!("map: {:?}", doubled);

    // filter
    let evens: Vec<&i32> = numbers.iter().filter(|&&x| x % 2 == 0).collect();
    println!("filter: {:?}", evens);

    // fold (reduce)
    let sum = numbers.iter().fold(0, |acc, x| acc + x);
    println!("fold sum: {}", sum);

    // enumerate
    for (i, val) in numbers.iter().enumerate() {
        println!("enumerate[{}] = {}", i, val);
    }

    // zip
    let names = vec!["a", "b", "c"];
    let vals = vec![1, 2, 3];
    let zipped: Vec<(&str, i32)> = names.into_iter().zip(vals.into_iter()).collect();
    println!("zip: {:?}", zipped);

    // --- 체이닝 ---
    let result: i32 = (1..=10)
        .filter(|x| x % 2 == 0)
        .map(|x| x * x)
        .sum();
    println!("체이닝 결과: {}", result);
}
