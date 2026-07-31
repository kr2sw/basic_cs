// 39: 디자인 패턴 — 전략, 옵저버, 빌더 구현

// === 1. 전략 패턴 (Strategy) ===
trait PaymentStrategy {
    fn pay(&self, amount: u32) -> String;
}

struct CreditCard {
    number: String,
}

impl PaymentStrategy for CreditCard {
    fn pay(&self, amount: u32) -> String {
        format!("카드 {}로 {}원 결제", &self.number[self.number.len() - 4..], amount)
    }
}

struct KakaoPay;

impl PaymentStrategy for KakaoPay {
    fn pay(&self, amount: u32) -> String {
        format!("카카오페이로 {}원 결제", amount)
    }
}

struct PaymentProcessor<S: PaymentStrategy> {
    strategy: S,
}

impl<S: PaymentStrategy> PaymentProcessor<S> {
    fn new(strategy: S) -> Self {
        PaymentProcessor { strategy }
    }

    fn process(&self, amount: u32) -> String {
        self.strategy.pay(amount)
    }
}

// === 2. 옵저버 패턴 (Observer) ===
trait Observer {
    fn update(&self, event: &str);
}

struct EmailNotifier {
    email: String,
}

impl Observer for EmailNotifier {
    fn update(&self, event: &str) {
        println!("[이메일: {}] 알림: {}", self.email, event);
    }
}

struct SmsNotifier {
    phone: String,
}

impl Observer for SmsNotifier {
    fn update(&self, event: &str) {
        println!("[SMS: {}] 알림: {}", self.phone, event);
    }
}

struct EventManager {
    observers: Vec<Box<dyn Observer>>,
}

impl EventManager {
    fn new() -> Self {
        EventManager { observers: Vec::new() }
    }

    fn subscribe(&mut self, obs: Box<dyn Observer>) {
        self.observers.push(obs);
    }

    fn notify(&self, event: &str) {
        for obs in &self.observers {
            obs.update(event);
        }
    }
}

// === 3. 빌더 패턴 (Builder) ===
struct Pizza {
    size: u8,
    cheese: bool,
    pepperoni: bool,
    mushrooms: bool,
}

struct PizzaBuilder {
    size: u8,
    cheese: bool,
    pepperoni: bool,
    mushrooms: bool,
}

impl PizzaBuilder {
    fn new(size: u8) -> Self {
        PizzaBuilder { size, cheese: false, pepperoni: false, mushrooms: false }
    }

    fn add_cheese(mut self) -> Self {
        self.cheese = true;
        self
    }

    fn add_pepperoni(mut self) -> Self {
        self.pepperoni = true;
        self
    }

    fn add_mushrooms(mut self) -> Self {
        self.mushrooms = true;
        self
    }

    fn build(self) -> Pizza {
        Pizza {
            size: self.size,
            cheese: self.cheese,
            pepperoni: self.pepperoni,
            mushrooms: self.mushrooms,
        }
    }
}

fn main() {
    println!("=== 전략 패턴 ===");
    let card = PaymentProcessor::new(CreditCard { number: "1234-5678-9012-3456".into() });
    println!("{}", card.process(15000));
    let kakao = PaymentProcessor::new(KakaoPay);
    println!("{}", kakao.process(3000));

    println!("\n=== 옵저버 패턴 ===");
    let mut manager = EventManager::new();
    manager.subscribe(Box::new(EmailNotifier { email: "a@example.com".into() }));
    manager.subscribe(Box::new(SmsNotifier { phone: "010-1234-5678".into() }));
    manager.notify("주문이 접수되었습니다");
    manager.notify("배송이 시작되었습니다");

    println!("\n=== 빌더 패턴 ===");
    let pizza = PizzaBuilder::new(12).add_cheese().add_pepperoni().build();
    println!(
        "피자: {}인치, 치즈={}, 페퍼로니={}, 버섯={}",
        pizza.size, pizza.cheese, pizza.pepperoni, pizza.mushrooms
    );

    // 함수형 스타일 전략 (클로저)
    println!("\n=== 클로저 전략 ===");
    let discount = |amount: u32| amount.saturating_sub(1000);
    println!("할인 적용: {}원", discount(20000));
}
