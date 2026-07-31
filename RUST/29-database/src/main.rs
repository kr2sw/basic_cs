// 29: 데이터베이스 — sqlx/Diesel 개념, 파일 기반 저장
//
// 실제 Rust DB 크레이트(sqlx/Diesel) 대신 파일 기반의
// "데이터베이스 흉내" 저장소를 구현해 저장/조회/인덱스 개념을 이해합니다.

use std::collections::HashMap;
use std::io::{Read, Write};

// === 1. 레코드 ===
#[derive(Clone, Debug)]
struct User {
    id: u64,
    name: String,
    age: u32,
}

impl User {
    fn to_line(&self) -> String {
        format!("{}|{}|{}", self.id, self.name, self.age)
    }
    fn from_line(line: &str) -> Option<Self> {
        let mut parts = line.splitn(3, '|');
        let id = parts.next()?.parse().ok()?;
        let name = parts.next()?.to_string();
        let age = parts.next()?.trim().parse().ok()?;
        Some(User { id, name, age })
    }
}

// === 2. 간단한 파일 DB ===
struct FileDb {
    path: String,
    // 인덱스: 이름 -> id 목록 (성능 향상 시뮬레이션)
    index: HashMap<String, Vec<u64>>,
}

impl FileDb {
    fn open(path: &str) -> Self {
        let mut db = FileDb { path: path.into(), index: HashMap::new() };
        db.rebuild_index();
        db
    }

    fn rebuild_index(&mut self) {
        self.index.clear();
        for user in self.scan() {
            self.index.entry(user.name.clone()).or_default().push(user.id);
        }
    }

    fn scan(&self) -> Vec<User> {
        let content = std::fs::read_to_string(&self.path).unwrap_or_default();
        content
            .lines()
            .filter_map(User::from_line)
            .collect()
    }

    fn save(&self, users: &[User]) -> std::io::Result<()> {
        let mut out = String::new();
        for u in users {
            out.push_str(&u.to_line());
            out.push('\n');
        }
        std::fs::write(&self.path, out)
    }

    // INSERT (단순 추가)
    fn insert(&mut self, user: User) -> std::io::Result<()> {
        let mut users = self.scan();
        users.push(user.clone());
        self.save(&users)?;
        self.index.entry(user.name.clone()).or_default().push(user.id);
        Ok(())
    }

    // SELECT by id
    fn find(&self, id: u64) -> Option<User> {
        self.scan().into_iter().find(|u| u.id == id)
    }

    // SELECT by name (인덱스 사용)
    fn find_by_name(&self, name: &str) -> Vec<User> {
        let ids = self.index.get(name);
        let all = self.scan();
        match ids {
            Some(ids) => all.into_iter().filter(|u| ids.contains(&u.id)).collect(),
            None => vec![],
        }
    }

    // DELETE
    fn delete(&mut self, id: u64) -> std::io::Result<bool> {
        let mut users = self.scan();
        let before = users.len();
        users.retain(|u| u.id != id);
        if users.len() == before {
            return Ok(false);
        }
        self.save(&users)?;
        self.rebuild_index();
        Ok(true)
    }
}

// === 3. 간단한 "SQL" 흉내 ===
enum Query {
    SelectAll,
    SelectWhere { col: String, value: String },
}

fn parse_sql(sql: &str) -> Option<Query> {
    let sql = sql.trim();
    let upper = sql.to_uppercase();
    if upper == "SELECT * FROM users" {
        return Some(Query::SelectAll);
    }
    if upper.starts_with("SELECT * FROM users WHERE") {
        // 예: SELECT * FROM users WHERE name = 'kim'
        let rest = sql["SELECT * FROM users WHERE".len()..].trim();
        if let Some((col, value)) = rest.split_once('=') {
            let col = col.trim().to_string();
            let value = value.trim().trim_matches('\'').to_string();
            return Some(Query::SelectWhere { col, value });
        }
    }
    None
}

fn main() {
    let db_path = "users.db";
    let mut db = FileDb::open(db_path);

    // 데이터 삽입
    db.insert(User { id: 1, name: "kim".into(), age: 30 }).expect("insert");
    db.insert(User { id: 2, name: "lee".into(), age: 25 }).expect("insert");
    db.insert(User { id: 3, name: "kim".into(), age: 35 }).expect("insert");
    println!("3명 저장 완료");

    // id 조회
    if let Some(u) = db.find(2) {
        println!("id=2: {:?}", u);
    }

    // 이름 인덱스 조회
    println!("name=kim 인덱스 조회: {:?}", db.find_by_name("kim").iter().map(|u| u.id).collect::<Vec<_>>());

    // 간단한 SQL 파싱 실행
    let sql = "SELECT * FROM users WHERE name = 'lee'";
    if let Some(query) = parse_sql(sql) {
        match query {
            Query::SelectAll => println!("전체: {:?}", db.scan()),
            Query::SelectWhere { col, value } => {
                if col == "name" {
                    println!("WHERE 결과: {:?}", db.find_by_name(&value).iter().map(|u| u.name.clone()).collect::<Vec<_>>());
                }
            }
        }
    } else {
        println!("지원하지 않는 SQL: {sql}");
    }

    // 삭제
    db.delete(1).expect("delete");
    println!("id=1 삭제 후 scan: {:?}", db.scan().iter().map(|u| u.name.clone()).collect::<Vec<_>>());

    // 정리
    let _ = std::fs::remove_file(db_path);
    println!("\n정리 완료 (테스트 DB 삭제)");
    println!("실제 프로덕션에서는 sqlx / Diesel 크레이트를 사용하세요.");
}
