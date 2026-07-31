// 30: 직렬화 — serde 개념, 자체 직렬화 구현
//
// serde는 구조체 <-> JSON 매핑을 derive 매크로로 자동화합니다.
// 여기서는 그 원리를 재현하는 미니 직렬화기를 만듭니다.
// (실제 프로덕션에서는 serde + serde_json 사용)

use std::collections::BTreeMap;

// === 1. 값을 표현하는 중간 자료형 (serde의 Value에 해당) ===
#[derive(Clone, Debug, PartialEq)]
enum Value {
    Null,
    Bool(bool),
    Number(f64),
    String(String),
    Array(Vec<Value>),
    Object(BTreeMap<String, Value>),
}

// === 2. Serialize 트레잇 (serde의 Serialize 개념) ===
trait Serialize {
    fn serialize(&self) -> Value;
}

// 구현체들
impl Serialize for i32 {
    fn serialize(&self) -> Value {
        Value::Number(*self as f64)
    }
}

impl Serialize for f64 {
    fn serialize(&self) -> Value {
        Value::Number(*self)
    }
}

impl Serialize for &str {
    fn serialize(&self) -> Value {
        Value::String(self.to_string())
    }
}

impl Serialize for String {
    fn serialize(&self) -> Value {
        Value::String(self.clone())
    }
}

impl Serialize for bool {
    fn serialize(&self) -> Value {
        Value::Bool(*self)
    }
}

impl<T: Serialize> Serialize for Vec<T> {
    fn serialize(&self) -> Value {
        Value::Array(self.iter().map(|v| v.serialize()).collect())
    }
}

// === 3. 파생 매크로를 흉내낸 구조체 + 수동 구현 ===
#[derive(Clone)]
struct Person {
    name: String,
    age: i32,
    active: bool,
    tags: Vec<String>,
}

impl Serialize for Person {
    fn serialize(&self) -> Value {
        let mut map = BTreeMap::new();
        map.insert("name".into(), self.name.serialize());
        map.insert("age".into(), self.age.serialize());
        map.insert("active".into(), self.active.serialize());
        map.insert("tags".into(), self.tags.serialize());
        Value::Object(map)
    }
}

// === 4. Value -> JSON 문자열 (직렬화) ===
fn to_json(v: &Value) -> String {
    match v {
        Value::Null => "null".into(),
        Value::Bool(b) => b.to_string(),
        Value::Number(n) => {
            if n.fract() == 0.0 {
                format!("{}", *n as i64)
            } else {
                format!("{}", n)
            }
        }
        Value::String(s) => format!("\"{}\"", s.replace('"', "\\\"")),
        Value::Array(items) => {
            let inner: Vec<String> = items.iter().map(to_json).collect();
            format!("[{}]", inner.join(","))
        }
        Value::Object(map) => {
            let inner: Vec<String> = map
                .iter()
                .map(|(k, v)| format!("\"{}\":{}", k, to_json(v)))
                .collect();
            format!("{{{}}}", inner.join(","))
        }
    }
}

// === 5. JSON 파싱 (역직렬화, 간단한 재귀 파서) ===
struct Parser<'a> {
    chars: std::iter::Peekable<std::str::Chars<'a>>,
}

impl<'a> Parser<'a> {
    fn new(s: &'a str) -> Self {
        Parser { chars: s.chars().peekable() }
    }

    fn skip_ws(&mut self) {
        while matches!(self.chars.peek(), Some(' ') | Some('\n') | Some('\t')) {
            self.chars.next();
        }
    }

    fn parse(&mut self) -> Option<Value> {
        self.skip_ws();
        match self.chars.peek()? {
            '{' => self.parse_object(),
            '[' => self.parse_array(),
            '"' => Some(Value::String(self.parse_string()?)),
            't' => { self.consume("true")?; Some(Value::Bool(true)) }
            'f' => { self.consume("false")?; Some(Value::Bool(false)) }
            'n' => { self.consume("null")?; Some(Value::Null) }
            c if c.is_ascii_digit() || *c == '-' => self.parse_number(),
            _ => None,
        }
    }

    fn consume(&mut self, lit: &str) -> Option<()> {
        for c in lit.chars() {
            if self.chars.next()? != c {
                return None;
            }
        }
        Some(())
    }

    fn parse_string(&mut self) -> Option<String> {
        self.chars.next()?; // "
        let mut out = String::new();
        loop {
            match self.chars.next()? {
                '"' => break,
                '\\' => {
                    let esc = self.chars.next()?;
                    out.push(match esc {
                        'n' => '\n',
                        't' => '\t',
                        '"' => '"',
                        '\\' => '\\',
                        c => c,
                    });
                }
                c => out.push(c),
            }
        }
        Some(out)
    }

    fn parse_number(&mut self) -> Option<Value> {
        let mut s = String::new();
        while let Some(&c) = self.chars.peek() {
            if c.is_ascii_digit() || c == '-' || c == '.' || c == 'e' || c == 'E' || c == '+' {
                s.push(c);
                self.chars.next();
            } else {
                break;
            }
        }
        s.parse::<f64>().ok().map(Value::Number)
    }

    fn parse_array(&mut self) -> Option<Value> {
        self.chars.next()?; // [
        let mut items = Vec::new();
        self.skip_ws();
        if self.chars.peek() == Some(&']') {
            self.chars.next();
            return Some(Value::Array(items));
        }
        loop {
            items.push(self.parse()?);
            self.skip_ws();
            match self.chars.next()? {
                ',' => continue,
                ']' => break,
                _ => return None,
            }
        }
        Some(Value::Array(items))
    }

    fn parse_object(&mut self) -> Option<Value> {
        self.chars.next()?; // {
        let mut map = BTreeMap::new();
        self.skip_ws();
        if self.chars.peek() == Some(&'}') {
            self.chars.next();
            return Some(Value::Object(map));
        }
        loop {
            let key = self.parse_string()?;
            self.skip_ws();
            self.chars.next()?; // :
            let val = self.parse()?;
            map.insert(key, val);
            self.skip_ws();
            match self.chars.next()? {
                ',' => continue,
                '}' => break,
                _ => return None,
            }
        }
        Some(Value::Object(map))
    }
}

// === 6. Deserialize 흉내: Person 재구성 ===
impl Person {
    fn from_json(json: &str) -> Option<Person> {
        let mut parser = Parser::new(json);
        let value = parser.parse()?;
        match value {
            Value::Object(map) => {
                let name = match map.get("name")? { Value::String(s) => s.clone(), _ => return None };
                let age = match map.get("age")? { Value::Number(n) => *n as i32, _ => return None };
                let active = match map.get("active")? { Value::Bool(b) => *b, _ => return None };
                let tags = match map.get("tags")? {
                    Value::Array(items) => items.iter().filter_map(|v| match v {
                        Value::String(s) => Some(s.clone()),
                        _ => None,
                    }).collect(),
                    _ => return None,
                };
                Some(Person { name, age, active, tags })
            }
            _ => None,
        }
    }
}

fn main() {
    let person = Person {
        name: "Alice".into(),
        age: 30,
        active: true,
        tags: vec!["rust".into(), "serde".into()],
    };

    // 직렬화
    let value = person.serialize();
    let json = to_json(&value);
    println!("직렬화 결과: {}", json);

    // 역직렬화
    if let Some(back) = Person::from_json(&json) {
        println!("역직렬화: name={} age={} active={} tags={:?}", back.name, back.age, back.active, back.tags);
    }

    // 배열 직렬화
    let nums = vec![1, 2, 3];
    println!("배열: {}", to_json(&nums.serialize()));

    println!("\n실제 프로덕션에서는 serde + serde_json을 사용하세요.");
}
