//! 10-traits: 트레이트 정의, 구현, 기본 메서드, 트레이트 바운드, 트레이트 객체
//!
//! 러스트의 트레이트 시스템을 다룹니다.

use std::fmt;
use std::fmt::Debug;

// =========================================================
// 1. 트레이트 정의 (Trait Definition)
// =========================================================

/// 요약 가능한 동작을 정의하는 트레이트
trait Summary {
    fn summarize(&self) -> String;

    // 기본 구현이 있는 메서드
    fn summarize_author(&self) -> String {
        String::from("(알 수 없는 저자)")  // 기본 구현
    }

    // 기본 구현을 사용하는 다른 메서드
    fn summary_with_author(&self) -> String {
        format!("{} by {}", self.summarize(), self.summarize_author())
    }
}

// =========================================================
// 2. 트레이트 구현 (Implementing Trait)
// =========================================================

#[derive(Debug)]
struct Article {
    headline: String,
    location: String,
    author: String,
    content: String,
}

impl Summary for Article {
    fn summarize(&self) -> String {
        format!("{} - {} (by {})", self.headline, self.location, self.author)
    }

    fn summarize_author(&self) -> String {
        format!("@{}", self.author)
    }
}

#[derive(Debug, Clone)]
struct Tweet {
    username: String,
    content: String,
    reply: bool,
    retweet: bool,
}

impl Summary for Tweet {
    fn summarize(&self) -> String {
        format!("{}: {}", self.username, self.content)
    }

    // summarize_author는 기본 구현 사용 안 함
    fn summarize_author(&self) -> String {
        format!("@{} (트윗)", self.username)
    }
}

// =========================================================
// 3. 기본 구현만 사용하는 트레이트
// =========================================================

trait Greet {
    fn greet(&self) -> String {
        String::from("안녕하세요!")  // 기본 구현
    }

    fn greet_named(&self, name: &str) -> String {
        format!("{}, {}!", self.greet(), name)
    }
}

struct Korean;
struct American;

impl Greet for Korean {
    fn greet(&self) -> String {
        String::from("안녕하세요")
    }
}

impl Greet for American {}  // 기본 구현 사용

// =========================================================
// 4. Display, Debug 트레이트 (직접 구현)
// =========================================================

#[derive(Debug)]  // Debug derive
struct Coordinate {
    x: i32,
    y: i32,
}

// Debug를 derive하지 않고 직접 구현
struct ManualDebug {
    value: i32,
}

impl fmt::Debug for ManualDebug {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "ManualDebug {{ value: {} }}", self.value)
    }
}

// Display 직접 구현
impl fmt::Display for Coordinate {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "({}, {})", self.x, self.y)
    }
}

// =========================================================
// 5. 트레이트 바운드 - impl Trait 문법
// =========================================================

/// impl Trait 문법 (매개변수 위치)
fn notify(item: &impl Summary) {
    println!("속보! {}", item.summarize());
}

/// impl Trait 문법 (반환 위치)
fn create_tweet() -> impl Summary {
    Tweet {
        username: String::from("rust_bot"),
        content: String::from("러스트 최고!"),
        reply: false,
        retweet: false,
    }
}

// =========================================================
// 6. 트레이트 바운드 - where 절
// =========================================================

/// where 절을 사용한 트레이트 바운드
fn some_function<T, U>(t: &T, u: &U)
where
    T: Summary + Clone + Debug,
    U: Summary + Clone + Debug,
{
    println!("T: {:?}", t);
    println!("U: {:?}", u);
    println!("T.summary(): {}", t.summarize());
    println!("U.summary(): {}", u.summarize());
}

// =========================================================
// 7. 트레이트 객체 (Box<dyn Trait>)
// =========================================================

/// 트레이트 객체를 매개변수로 (동적 디스패치)
fn notify_dyn(item: Box<dyn Summary>) {
    println!("동적 디스패치: {}", item.summarize());
}

/// 트레이트 객체 참조
fn notify_dyn_ref(item: &dyn Summary) {
    println!("동적 디스패치(ref): {}", item.summarize());
}

/// 트레이트 객체 벡터
fn print_all_summaries(items: &[Box<dyn Summary>]) {
    for (i, item) in items.iter().enumerate() {
        println!("[{}] {}", i, item.summarize());
    }
}

// =========================================================
// 8. 표준 트레이트 derive
// =========================================================

/// Clone, Copy, PartialEq, Eq, Debug를 derive
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
struct Book {
    title: &'static str,
    pages: u32,
}

// =========================================================
// 9. 트레이트 상속 (Supertrait)
// =========================================================

trait PrintableSummary: Summary + Debug {
    fn print_summary(&self) {
        println!("{:?}", self);
        println!("{}", self.summarize());
    }
}

impl PrintableSummary for Article {}
impl PrintableSummary for Tweet {}

// =========================================================
// 10. From / Into 트레이트 예제
// =========================================================

#[derive(Debug)]
struct Temperature {
    celsius: f64,
}

impl From<f64> for Temperature {
    fn from(celsius: f64) -> Self {
        Temperature { celsius }
    }
}

impl From<i32> for Temperature {
    fn from(celsius: i32) -> Self {
        Temperature {
            celsius: celsius as f64,
        }
    }
}

// =========================================================
// 11. 반환 타입으로 impl Trait
// =========================================================

/// 조건에 따라 다른 구체 타입을 반환할 수 없음 (단일 타입만)
fn returns_summarizable(switch: bool) -> impl Summary {
    if switch {
        Tweet {
            username: String::from("user"),
            content: String::from("트윗 내용"),
            reply: false,
            retweet: false,
        }
    } else {
        Tweet {
            username: String::from("user2"),
            content: String::from("다른 트윗"),
            reply: false,
            retweet: false,
        }
    }
}

// =========================================================
// 12. 트레이트를 구현한 제네릭 타입
// =========================================================

struct Pair<T> {
    x: T,
    y: T,
}

impl<T> Pair<T> {
    fn new(x: T, y: T) -> Self {
        Self { x, y }
    }
}

// 특정 트레이트를 구현한 타입에만 메서드 추가
impl<T: fmt::Display + PartialOrd> Pair<T> {
    fn cmp_display(&self) {
        if self.x > self.y {
            println!("x({}) > y({})", self.x, self.y);
        } else if self.x < self.y {
            println!("x({}) < y({})", self.x, self.y);
        } else {
            println!("x({}) == y({})", self.x, self.y);
        }
    }
}

fn main() {
    println!("=== 10-traits: 트레이트 ===");

    // 1. 트레이트 구현 사용
    println!("\n--- 트레이트 구현 ---");

    let article = Article {
        headline: String::from("러스트 2024 에디션 발표!"),
        location: String::from("Berlin"),
        author: String::from("Rust Team"),
        content: String::from("러스트 2024 에디션이 발표되었습니다..."),
    };

    let tweet = Tweet {
        username: String::from("rust_user"),
        content: String::from("러스트 공부 중! #rust"),
        reply: false,
        retweet: false,
    };

    println!("Article: {}", article.summarize());
    println!("Tweet: {}", tweet.summarize());
    println!("Article with author: {}", article.summary_with_author());
    println!("Tweet with author: {}", tweet.summary_with_author());

    // 2. 기본 메서드
    println!("\n--- 기본 메서드 ---");

    let korean = Korean;
    let american = American;

    println!("Korean: {}", korean.greet());
    println!("American: {}", american.greet());
    println!("Korean named: {}", korean.greet_named("철수"));
    println!("American named: {}", american.greet_named("John"));

    // 3. Display / Debug
    println!("\n--- Display / Debug ---");

    let coord = Coordinate { x: 10, y: 20 };
    println!("Display: {}", coord);
    println!("Debug: {:?}", coord);

    let manual = ManualDebug { value: 42 };
    println!("Manual Debug: {:?}", manual);

    // 4. impl Trait 매개변수
    println!("\n--- impl Trait 매개변수 ---");

    notify(&article);
    notify(&tweet);

    // 5. where 절
    println!("\n--- where 절 ---");

    // some_function(&article, &tweet);  // Debug 필요

    #[derive(Clone, Debug)]
    struct SimpleSummary(String);

    impl Summary for SimpleSummary {
        fn summarize(&self) -> String {
            self.0.clone()
        }
    }

    let s1 = SimpleSummary("첫 번째".to_string());
    let s2 = SimpleSummary("두 번째".to_string());
    // some_function(&s1, &s2);

    // 6. 트레이트 객체
    println!("\n--- 트레이트 객체 ---");

    let article_box: Box<dyn Summary> = Box::new(Article {
        headline: String::from("트레이트 객체 예제"),
        location: String::from("Seoul"),
        author: String::from("Author"),
        content: String::from("내용..."),
    });

    let tweet_box: Box<dyn Summary> = Box::new(Tweet {
        username: String::from("tweeter"),
        content: String::from("트레이트 객체 재미있네요!"),
        reply: false,
        retweet: false,
    });

    notify_dyn(article_box);
    // article_box는 이동됨

    notify_dyn_ref(&*tweet_box);
    println!("tweet_box는 아직 유효: {}", tweet_box.summarize());

    let items: Vec<Box<dyn Summary>> = vec![
        Box::new(Tweet {
            username: "user1".to_string(),
            content: "첫 번째".to_string(),
            reply: false,
            retweet: false,
        }),
        Box::new(Tweet {
            username: "user2".to_string(),
            content: "두 번째".to_string(),
            reply: false,
            retweet: false,
        }),
    ];
    print_all_summaries(&items);

    // 7. Derive 매크로
    println!("\n--- Derive 매크로 ---");

    let book1 = Book {
        title: "러스트 프로그래밍",
        pages: 500,
    };
    let book2 = book1;  // Copy (derive Copy 덕분)
    let book3 = book1.clone();  // Clone

    println!("book1: {:?}", book1);
    println!("book1 == book2: {}", book1 == book2);
    println!("book1 == book3: {}", book1 == book3);

    // 8. From / Into
    println!("\n--- From / Into ---");

    let temp1 = Temperature::from(36.5);
    let temp2: Temperature = 37.into();
    println!("temp1: {:?}", temp1);
    println!("temp2: {:?}", temp2);

    // 9. impl Trait 반환
    println!("\n--- impl Trait 반환 ---");

    let summary1 = returns_summarizable(true);
    println!("반환된 요약: {}", summary1.summarize());

    let summary2 = create_tweet();
    println!("create_tweet: {}", summary2.summarize());

    // 10. 트레이트 바운드 메서드
    println!("\n--- 트레이트 바운드 메서드 ---");

    let pair = Pair::new(10, 20);
    pair.cmp_display();

    let pair2 = Pair::new("hello", "world");
    pair2.cmp_display();

    let pair3 = Pair::new(3.14, 3.14);
    pair3.cmp_display();

    // 11. 트레이트를 매개변수로
    println!("\n--- 트레이트 매개변수 (제네릭) ---");

    fn print_summary<T: Summary>(item: &T) {
        println!("제네릭: {}", item.summarize());
    }

    print_summary(&article);
    print_summary(&tweet);

    fn print_summaries<T: Summary>(items: &[T]) {
        for item in items {
            println!(" - {}", item.summarize());
        }
    }

    // 12. 여러 트레이트 바운드
    println!("\n--- 여러 트레이트 바운드 ---");

    fn display_and_summarize_string(summary: &str) {
        println!("Summary: {}", summary);
    }

    println!("Debug tweet: {:?}", tweet);

    display_and_summarize_string(&tweet.summarize());

    // 13. Clone + Summary 바운드
    fn clone_and_summarize<T: Summary + Clone>(item: &T) -> (T, String) {
        (item.clone(), item.summarize())
    }

    let tweet_clone = Tweet {
        username: "clone_test".to_string(),
        content: "클론 테스트".to_string(),
        reply: false,
        retweet: false,
    };

    let (cloned, summary) = clone_and_summarize(&tweet_clone);
    println!("클론됨: {}, 요약: {}", cloned.summarize(), summary);
}
