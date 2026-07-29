//! 09-generics: 제네릭 함수, 구조체, 열거형, 트레이트 바운드, const generics
//!
//! 러스트의 제네릭 프로그래밍을 다룹니다.

use std::cmp::PartialOrd;
use std::fmt::Display;
use std::ops::Add;

// =========================================================
// 1. 제네릭 함수 (Generic Function)
// =========================================================

/// 가장 큰 값을 찾는 제네릭 함수 (PartialOrd 필요)
fn largest<T: PartialOrd>(list: &[T]) -> &T {
    let mut largest = &list[0];
    for item in list.iter() {
        if item > largest {
            largest = item;
        }
    }
    largest
}

/// 두 값 중 큰 값을 반환
fn max_of_two<T: PartialOrd>(a: T, b: T) -> T {
    if a > b { a } else { b }
}

// =========================================================
// 2. 제네릭 구조체 (Generic Struct)
// =========================================================

/// 단일 타입 매개변수를 가지는 Point
#[derive(Debug)]
struct Point<T> {
    x: T,
    y: T,
}

/// 두 개의 다른 타입을 가지는 Point
#[derive(Debug)]
struct Point2<T, U> {
    x: T,
    y: U,
}

// =========================================================
// 3. 제네릭 impl 블록
// =========================================================

impl<T> Point<T> {
    fn x(&self) -> &T {
        &self.x
    }

    fn y(&self) -> &T {
        &self.y
    }
}

// 특정 타입에 대해서만 구현
impl Point<f64> {
    fn distance_from_origin(&self) -> f64 {
        (self.x.powi(2) + self.y.powi(2)).sqrt()
    }
}

// 두 개의 다른 타입 매개변수를 가지는 impl
impl<T, U> Point2<T, U> {
    fn mix_up<V, W>(self, other: Point2<V, W>) -> Point2<T, W> {
        Point2 {
            x: self.x,
            y: other.y,
        }
    }
}

// =========================================================
// 4. 제네릭 enum
// =========================================================

/// 제네릭 Option 모방
#[derive(Debug)]
enum MyOption<T> {
    Some(T),
    None,
}

/// 두 개의 제네릭 타입을 가지는 Result 모방
#[derive(Debug)]
enum MyResult<T, E> {
    Ok(T),
    Err(E),
}

// =========================================================
// 5. 트레이트 바운드 (Trait Bounds)
// =========================================================

/// Display + PartialOrd 바운드
fn compare_and_display<T: Display + PartialOrd>(a: T, b: T) {
    if a > b {
        println!("{} > {}", a, b);
    } else if a < b {
        println!("{} < {}", a, b);
    } else {
        println!("{} == {}", a, b);
    }
}

/// where 절을 사용한 트레이트 바운드
fn print_pairs<T, U>(items: &[(T, U)])
where
    T: Display,
    U: Display,
{
    for (i, (t, u)) in items.iter().enumerate() {
        println!("[{}] {}: {}", i, t, u);
    }
}

/// 여러 트레이트 바운드 (+)
fn print_and_clone<T: Display + Clone>(item: &T) -> T {
    println!("복제할 값: {}", item);
    item.clone()
}

// =========================================================
// 6. 제네릭 구조체 메서드 바운드
// =========================================================

#[derive(Debug)]
struct Pair<T> {
    first: T,
    second: T,
}

impl<T> Pair<T> {
    fn new(first: T, second: T) -> Self {
        Self { first, second }
    }
}

// Display + PartialOrd가 있을 때만 구현
impl<T: Display + PartialOrd> Pair<T> {
    fn cmp_display(&self) {
        if self.first > self.second {
            println!("first({}) > second({})", self.first, self.second);
        } else {
            println!("first({}) <= second({})", self.first, self.second);
        }
    }
}

// =========================================================
// 7. const generics (상수 제네릭)
// =========================================================

/// 고정 크기 배열을 받아 첫 요소를 반환
fn first_element<T, const N: usize>(arr: &[T; N]) -> &T {
    &arr[0]
}

/// 배열의 길이를 반환하는 const generic 함수
fn array_length<T, const N: usize>(_arr: &[T; N]) -> usize {
    N
}

/// const generic으로 배열 복제
fn repeat_element<T: Copy, const N: usize>(element: T) -> [T; N] {
    [element; N]
}

// =========================================================
// 8. 연산자 오버로딩 (제네릭 + 트레이트)
// =========================================================

#[derive(Debug, Clone, Copy)]
struct Vector2D<T> {
    x: T,
    y: T,
}

impl<T: Add<Output = T>> Add for Vector2D<T> {
    type Output = Self;

    fn add(self, other: Self) -> Self {
        Self {
            x: self.x + other.x,
            y: self.y + other.y,
        }
    }
}

// =========================================================
// 9. 트레이트 바운드를 사용한 조건부 메서드
// =========================================================

#[derive(Debug)]
struct Container<T> {
    value: T,
}

impl<T> Container<T> {
    fn new(value: T) -> Self {
        Self { value }
    }

    fn get(&self) -> &T {
        &self.value
    }
}

// Clone이 있을 때만 제공
impl<T: Clone> Container<T> {
    fn clone_inner(&self) -> T {
        self.value.clone()
    }
}

// Display + PartialEq가 있을 때만 제공
impl<T: Display + PartialEq> Container<T> {
    fn describe(&self, other: &T) {
        if self.value == *other {
            println!("값이 같음: {}", self.value);
        } else {
            println!("값이 다름: self={}, other={}", self.value, other);
        }
    }
}

fn main() {
    println!("=== 09-generics: 제네릭 ===");

    // 1. 제네릭 함수
    println!("\n--- 제네릭 함수 ---");

    let numbers = vec![34, 50, 25, 100, 65];
    println!("가장 큰 정수: {}", largest(&numbers));

    let chars = vec!['y', 'm', 'a', 'q'];
    println!("가장 큰 문자: {}", largest(&chars));

    let floats = vec![3.14, 2.71, 1.41, 1.73];
    println!("가장 큰 실수: {}", largest(&floats));

    println!("max(10, 20): {}", max_of_two(10, 20));
    println!("max('a', 'z'): {}", max_of_two('a', 'z'));

    // 2. 제네릭 구조체
    println!("\n--- 제네릭 구조체 ---");

    let integer_point = Point { x: 5, y: 10 };
    let float_point = Point { x: 1.0, y: 4.0 };
    println!("정수 Point: {:?}", integer_point);
    println!("실수 Point: {:?}", float_point);

    let mixed = Point2 { x: 5, y: 4.0 };
    println!("혼합 Point2: {:?}", mixed);

    // 3. 제네릭 메서드
    println!("\n--- 제네릭 메서드 ---");

    println!("Point.x(): {}", integer_point.x());

    let p1 = Point { x: 5.0, y: 10.0 };
    let _p2 = Point { x: 3.0, y: 4.0 };
    // println!("distance: {}", p1.distance_from_origin());  // f64 전용
    println!("f64 Point distance: {}", p1.distance_from_origin());

    let mixed2 = mixed.mix_up(Point2 { x: "hello", y: 'R' });
    println!("mix_up: {:?}", mixed2);

    // 4. 제네릭 enum
    println!("\n--- 제네릭 enum ---");

    let some_value: MyOption<i32> = MyOption::Some(42);
    let no_value: MyOption<i32> = MyOption::None;
    println!("MyOption: {:?}, {:?}", some_value, no_value);

    let ok: MyResult<i32, String> = MyResult::Ok(100);
    let err: MyResult<i32, String> = MyResult::Err(String::from("에러!"));
    println!("MyResult: {:?}, {:?}", ok, err);

    // 5. 트레이트 바운드
    println!("\n--- 트레이트 바운드 ---");

    compare_and_display(10, 20);
    compare_and_display("apple", "banana");

    let pairs = [(1, "one"), (2, "two"), (3, "three")];
    print_pairs(&pairs);

    let cloned = print_and_clone(&"복제됨");
    println!("cloned: {}", cloned);

    // 6. 조건부 메서드
    println!("\n--- Pair 구조체 ---");

    let pair_int = Pair::new(10, 20);
    pair_int.cmp_display();

    let pair_str = Pair::new("abc", "xyz");
    pair_str.cmp_display();

    // 7. const generics
    println!("\n--- const generics ---");

    let arr1: [i32; 5] = [1, 2, 3, 4, 5];
    let arr2: [i32; 3] = [6, 7, 8];

    println!("첫 요소: {}", first_element(&arr1));
    println!("arr1 길이: {}", array_length(&arr1));
    println!("arr2 길이: {}", array_length(&arr2));

    let repeated: [i32; 5] = repeat_element(42);
    println!("const generic 반복: {:?}", repeated);
    let repeated_chars: [char; 3] = repeat_element('R');
    println!("const generic 문자: {:?}", repeated_chars);

    // 8. 연산자 오버로딩
    println!("\n--- 연산자 오버로딩 ---");

    let v1 = Vector2D { x: 1.0, y: 2.0 };
    let v2 = Vector2D { x: 3.0, y: 4.0 };
    let v3 = v1 + v2;
    println!("벡터 합: {:?}", v3);

    // 9. Container 조건부 메서드
    println!("\n--- Container 조건부 ---");

    let c = Container::new(42);
    println!("Container.get(): {}", c.get());

    let c_clone = Container::new("문자열");
    let cloned_inner = c_clone.clone_inner();
    println!("Container.clone_inner(): {}", cloned_inner);

    let c_desc = Container::new(100);
    c_desc.describe(&100);
    c_desc.describe(&200);

    // 10. 컴파일러가 추론하는 제네릭
    println!("\n--- 타입 추론 ---");

    let inferred = Point { x: 1, y: 2 };  // Point<i32>
    // let inferred2 = Point { x: 1, y: 2.0 };  // 에러! 같은 타입이어야 함
    let inferred3 = Point2 { x: 1, y: 2.0 };  // Point2<i32, f64>
    println!("추론됨: {:?}, {:?}", inferred, inferred3);
}
