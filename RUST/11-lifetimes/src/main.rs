// 11-lifetimes
// lifetime 어노테이션: 참조자의 유효 범위를 명시하여 댕글링 참조 방지

// --- 기본 lifetime 함수 ---

// &str에 lifetime 'a를 명시: 반환값은 x와 y 중 더 짧은 쪽과 같거나 짧아야 함
fn longest<'a>(x: &'a str, y: &'a str) -> &'a str {
    if x.len() > y.len() { x } else { y }
}

// --- struct 필드에 lifetime ---

// 구조체 필드가 참조를 저장할 때 모든 참조에 lifetime을 명시
struct Excerpt<'a> {
    part: &'a str,
}

impl<'a> Excerpt<'a> {
    fn new(part: &'a str) -> Self {
        Self { part }
    }

    fn level(&self) -> i32 {
        3
    }

    fn announce_and_return(&self, announcement: &str) -> &str {
        println!("주의: {}", announcement);
        self.part
    }
}

// --- 여러 lifetime 파라미터 ---

#[allow(dead_code)]
fn longest_with_announcement<'a, 'b>(_x: &'a str, _y: &'b str, ann: &str) -> &'a str
where
    'b: 'a,
{
    println!("공지: {}", ann);
    _x
}

// --- 'static lifetime ---
// 프로그램 전체 수명 동안 살아있는 참조
fn static_demo() -> &'static str {
    "이 문자열은 정적 메모리에 저장됩니다"
}

// --- lifetime 생략 규칙 (elision) ---
// 1. 각 입력 참조는 별도의 lifetime을 가짐
// 2. 하나의 입력 lifetime만 있으면 모든 출력에 적용
// 3. &self 또는 &mut self 메서드면 self의 lifetime이 모든 출력에 적용
fn first_word(s: &str) -> &str {
    // 컴파일러가 lifetime을 자동 추론 (elision)
    let bytes = s.as_bytes();
    for (i, &item) in bytes.iter().enumerate() {
        if item == b' ' {
            return &s[..i];
        }
    }
    &s[..]
}

fn main() {
    // longest 예제
    let string1 = String::from("긴 문자열");
    let result;
    {
        let string2 = "짧음";
        result = longest(string1.as_str(), string2);
        println!("더 긴 쪽: {}", result);
    }

    // struct lifetime
    let novel = String::from("나리는 궂은 비를... 몇 줄기...");
    let excerpt = Excerpt::new(&novel);
    println!("첫 문장: {}", excerpt.part);
    println!("레벨: {}", excerpt.level());
    println!("발표: {}", excerpt.announce_and_return("테스트"));

    // static
    let s: &'static str = static_demo();
    println!("static: {}", s);

    // 생략 규칙
    let words = "hello world rust";
    println!("첫 단어: {}", first_word(words));
}
