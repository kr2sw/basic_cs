// 22: 고급 패턴 매칭 — 가드, @바인딩, 구조 분해 심화

// === 1. 매치 가드 (Match Guards) ===
fn classify(n: i32) -> &'static str {
    match n {
        x if x < 0 => "음수",
        0 => "0",
        x if x < 10 => "한 자리 수",
        _ => "큰 수",
    }
}

// === 2. @ 바인딩 ===
fn describe(n: u32) -> String {
    match n {
        x @ 1..=5 => format!("{}는 작은 숫자", x),
        x @ 6..=10 => format!("{}는 중간 숫자", x),
        _ => "그 외".into(),
    }
}

// === 3. 튜플 구조 분해 ===
fn swap((a, b): (i32, i32)) -> (i32, i32) {
    (b, a)
}

// === 4. 구조체/Enum 구조 분해 ===
struct Point {
    x: i32,
    y: i32,
}

enum Message {
    Quit,
    Move { x: i32, y: i32 },
    Write(String),
    ChangeColor(u8, u8, u8),
}

fn handle_message(msg: Message) -> String {
    match msg {
        Message::Quit => "종료".into(),
        Message::Move { x, y } => format!("이동: ({}, {})", x, y),
        Message::Write(text) => format!("메시지: {}", text),
        Message::ChangeColor(r, g, b) => format!("색상: #{:02X}{:02X}{:02X}", r, g, b),
    }
}

// === 5. 슬라이스 분해 ===
fn first_two(v: &[i32]) -> String {
    match v {
        [a, b, ..] => format!("첫 두 요소: {}, {}", a, b),
        [a] => format!("요소 하나: {}", a),
        [] => "빈 슬라이스".into(),
    }
}

// === 6. 레스트 패턴과 가변 바인딩 ===
fn split_head(v: &[i32]) -> (&str, i32) {
    match v {
        [head, rest @ ..] => ("헤드", *head),
        [] => ("빈", 0),
    }
}

// === 7. 중첩 구조 분해 ===
struct Config {
    server: Server,
}

struct Server {
    port: u16,
    tls: bool,
}

fn port_info(config: &Config) -> String {
    match config {
        Config { server: Server { port, tls: true } } if *port == 443 => "보안 포트".into(),
        Config { server: Server { port, tls } } => format!("포트 {} (tls: {})", port, tls),
    }
}

// === 8. if let / while let ===
fn main() {
    println!("분류: {}", classify(-5));
    println!("분류: {}", classify(7));
    println!("분류: {}", classify(100));

    println!("@바인딩: {}", describe(3));
    println!("@바인딩: {}", describe(8));

    println!("튜플 분해: {:?}", swap((1, 2)));

    let msgs = vec![
        Message::Quit,
        Message::Move { x: 1, y: 2 },
        Message::Write("안녕".into()),
        Message::ChangeColor(255, 0, 128),
    ];
    for m in msgs {
        println!("{}", handle_message(m));
    }

    println!("슬라이스: {}", first_two(&[10, 20, 30]));
    println!("슬라이스: {}", first_two(&[1]));

    let (_, head) = split_head(&[9, 8, 7]);
    println!("헤드 값: {}", head);

    let cfg = Config { server: Server { port: 443, tls: true } };
    println!("{}", port_info(&cfg));

    // if let
    let opt = Some(5);
    if let Some(v) = opt {
        println!("if let: {}", v);
    }

    // while let
    let mut stack = vec![1, 2, 3];
    while let Some(top) = stack.pop() {
        print!("{} ", top);
    }
    println!();
}
