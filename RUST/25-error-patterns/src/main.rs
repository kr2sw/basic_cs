// 25: 에러 처리 패턴 — 커스텀 Error, Result 체이닝, context

use std::fmt;

// === 1. 커스텀 Error 타입 ===
#[derive(Debug)]
enum AppError {
    NotFound(String),
    InvalidInput { field: String, value: String },
    ParseInt(String),
    Io(String),
}

impl fmt::Display for AppError {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        match self {
            AppError::NotFound(what) => write!(f, "찾을 수 없음: {what}"),
            AppError::InvalidInput { field, value } => write!(f, "잘못된 입력 {field}: {value}"),
            AppError::ParseInt(s) => write!(f, "정수 변환 실패: {s}"),
            AppError::Io(s) => write!(f, "IO 오류: {s}"),
        }
    }
}

impl std::error::Error for AppError {}

// === 2. From 변환으로 ? 연산자 지원 ===
impl From<std::io::Error> for AppError {
    fn from(e: std::io::Error) -> Self {
        AppError::Io(e.to_string())
    }
}

impl From<std::num::ParseIntError> for AppError {
    fn from(e: std::num::ParseIntError) -> Self {
        AppError::ParseInt(e.to_string())
    }
}

// === 3. Result 체이닝 ===
fn read_number_file(path: &str) -> Result<i32, AppError> {
    // 존재하지 않는 파일은 NotFound로
    if !std::path::Path::new(path).exists() {
        return Err(AppError::NotFound(path.to_string()));
    }
    // 실제 파일 읽기 (없으면 Io)
    let content = std::fs::read_to_string(path)?;
    let n: i32 = content.trim().parse()?;
    Ok(n)
}

// === 4. map_err로 컨텍스트 추가 ===
fn validate_user(input: &str) -> Result<(&str, u32), AppError> {
    let mut parts = input.splitn(2, ':');
    let name = parts.next().unwrap_or("");
    let age_raw = parts.next().ok_or_else(|| AppError::InvalidInput {
        field: "age".into(),
        value: "없음".into(),
    })?;
    let age: u32 = age_raw.trim().parse().map_err(|_| AppError::InvalidInput {
        field: "age".into(),
        value: age_raw.trim().into(),
    })?;
    if name.is_empty() {
        return Err(AppError::InvalidInput {
            field: "name".into(),
            value: "빈 이름".into(),
        });
    }
    Ok((name, age))
}

// === 5. Option과 조합 ===
fn find_user(id: u32) -> Option<&'static str> {
    match id {
        1 => Some("kim"),
        2 => Some("lee"),
        _ => None,
    }
}

fn user_label(id: u32) -> Result<String, AppError> {
    let name = find_user(id).ok_or_else(|| AppError::NotFound(format!("사용자 id {id}")))?;
    Ok(format!("[{}] {}", id, name))
}

// === 6. 단일 함수로 통합 (간단한 파일 기반 저장소) ===
struct Db {
    base_dir: String,
}

impl Db {
    fn new(base_dir: &str) -> Self {
        Db { base_dir: base_dir.into() }
    }

    fn set(&self, key: &str, value: &str) -> Result<(), AppError> {
        std::fs::create_dir_all(&self.base_dir)?;
        let path = format!("{}/{}.txt", self.base_dir, key);
        std::fs::write(&path, value)?;
        Ok(())
    }

    fn get(&self, key: &str) -> Result<String, AppError> {
        let path = format!("{}/{}.txt", self.base_dir, key);
        if !std::path::Path::new(&path).exists() {
            return Err(AppError::NotFound(format!("키 {key}")));
        }
        Ok(std::fs::read_to_string(&path)?)
    }
}

fn main() {
    // 존재하지 않는 파일
    match read_number_file("missing.txt") {
        Ok(v) => println!("값: {v}"),
        Err(e) => println!("오류: {e}"),
    }

    // DB 저장/조회
    let db = Db::new("data");
    db.set("score", "100").expect("저장 실패");
    match db.get("score") {
        Ok(v) => println!("score = {}", v),
        Err(e) => println!("오류: {e}"),
    }
    match db.get("nope") {
        Ok(v) => println!("nope = {}", v),
        Err(e) => println!("오류: {e}"),
    }

    // 검증
    match validate_user("alice:30") {
        Ok((name, age)) => println!("사용자 {name}, {age}세"),
        Err(e) => println!("오류: {e}"),
    }
    match validate_user(":abc") {
        Ok((name, age)) => println!("사용자 {name}, {age}세"),
        Err(e) => println!("오류: {e}"),
    }

    // Option 조합
    println!("{}", user_label(1).unwrap());
    match user_label(99) {
        Ok(l) => println!("{l}"),
        Err(e) => println!("오류: {e}"),
    }

    // 에러 타입은 dyn Error로 업캐스트 가능
    let err: Box<dyn std::error::Error> = Box::new(AppError::NotFound("item".into()));
    println!("다형 에러: {err}");

    // 정리
    let _ = std::fs::remove_dir_all("data");
}
