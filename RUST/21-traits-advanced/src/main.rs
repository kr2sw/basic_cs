// 21: 고급 트레잇 — 연관 타입, 제네릭 트레잇, 상속 트레잇

// === 1. 연관 타입 (Associated Types) ===
trait Summarize {
    type Output;                       // 연관 타입
    fn summarize(&self) -> Self::Output;
}

struct Article {
    title: String,
    body: String,
}

struct Tweet {
    author: String,
    text: String,
}

impl Summarize for Article {
    type Output = String;
    fn summarize(&self) -> Self::Output {
        format!("[기사] {}: {}", self.title, &self.body[..self.body.len().min(20)])
    }
}

impl Summarize for Tweet {
    type Output = (String, String);    // Tweet은 튜플로 반환
    fn summarize(&self) -> Self::Output {
        (self.author.clone(), self.text.clone())
    }
}

// === 2. 제네릭 트레잇 (파라미터로 타입을 받는 트레잇) ===
trait Convert<T> {
    fn convert(&self) -> T;
}

struct Celsius(f64);

impl Convert<f64> for Celsius {
    fn convert(&self) -> f64 {
        self.0 * 9.0 / 5.0 + 32.0      // 섭씨 → 화씨
    }
}

impl Convert<String> for Celsius {
    fn convert(&self) -> String {
        format!("{:.1}°C", self.0)
    }
}

// === 3. 상속 트레잇 (Supertrait) ===
trait Greet {
    fn name(&self) -> &str;
}

trait Welcome: Greet {                  // Welcome을 구현하려면 Greet도 구현
    fn welcome(&self) -> String {
        format!("환영합니다, {}님!", self.name())
    }
}

struct User {
    name: String,
}

impl Greet for User {
    fn name(&self) -> &str {
        &self.name
    }
}

impl Welcome for User {}                // Greet 구현만으로 Welcome도 동작

// === 4. 트레잇 객체 (dyn Trait) ===
trait Draw {
    fn draw(&self) -> String;
}

struct Circle;
struct Square;

impl Draw for Circle {
    fn draw(&self) -> String { "원 그리기".into() }
}
impl Draw for Square {
    fn draw(&self) -> String { "사각형 그리기".into() }
}

fn render(shapes: &[Box<dyn Draw>]) -> Vec<String> {
    shapes.iter().map(|s| s.draw()).collect()
}

// === 5. 블랭킷 구현 (Blanket Implementation) ===
trait Double {
    fn double(&self) -> Self;
}

impl<T: std::ops::Add<Output = T> + Copy> Double for T {
    fn double(&self) -> Self {
        *self + *self
    }
}

// === main ===
fn main() {
    // 연관 타입
    let article = Article { title: "Rust 중급".into(), body: "트레잇을 깊이 이해한다".into() };
    let tweet = Tweet { author: "kim".into(), text: "안녕하세요!".into() };
    println!("기사 요약: {}", article.summarize());
    let (author, text) = tweet.summarize();
    println!("트윗: {} - {}", author, text);

    // 제네릭 트레잇
    let temp = Celsius(25.0);
    println!("화씨: {:.1}", <Celsius as Convert<f64>>::convert(&temp));
    println!("표시: {}", <Celsius as Convert<String>>::convert(&temp));

    // 상속 트레잇
    let user = User { name: "Alice".into() };
    println!("{}", user.welcome());

    // 트레잇 객체
    let shapes: Vec<Box<dyn Draw>> = vec![Box::new(Circle), Box::new(Square)];
    println!("렌더링: {:?}", render(&shapes));

    // 블랭킷 구현
    println!("double: {}", 21.0.double());
    println!("double: {}", 42.double());
}
