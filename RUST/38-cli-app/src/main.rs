// 38: CLI 애플리케이션 — clap 개념, 인자 파싱 직접 구현
//
// 실행 예:
//   cargo run -- add "공부하기"
//   cargo run -- list
//   cargo run -- done 1
//   cargo run -- --help

use std::env;
use std::io::Write;

// === 1. 파싱된 인자 구조 ===
struct ParsedArgs {
    verbose: bool,
    help: bool,
    positionals: Vec<String>,
}

// === 2. 미니 인자 파서 (clap 원리) ===
fn parse_args(raw: Vec<String>) -> ParsedArgs {
    let mut out = ParsedArgs { verbose: false, help: false, positionals: Vec::new() };
    let mut iter = raw.into_iter();
    while let Some(arg) = iter.next() {
        match arg.as_str() {
            "-v" | "--verbose" => out.verbose = true,
            "-h" | "--help" => out.help = true,
            s if s.starts_with("--") || s.starts_with('-') => {
                // --key value 형태
                if let Some(value) = iter.next() {
                    out.positionals.push(value);
                }
            }
            s => out.positionals.push(s.to_string()),
        }
    }
    out
}

// === 3. Todo 저장 (파일 기반) ===
const DB_FILE: &str = "todos.txt";

fn load_todos() -> Vec<(String, bool)> {
    let content = std::fs::read_to_string(DB_FILE).unwrap_or_default();
    content
        .lines()
        .map(|line| {
            let done = line.starts_with("[x]");
            let text = line.trim_start_matches("[x] ").trim_start_matches("[ ] ");
            (text.to_string(), done)
        })
        .collect()
}

fn save_todos(todos: &[(String, bool)]) -> std::io::Result<()> {
    let mut f = std::fs::File::create(DB_FILE)?;
    for (text, done) in todos {
        writeln!(f, "{} {}", if *done { "[x]" } else { "[ ]" }, text)?;
    }
    Ok(())
}

fn cmd_add(text: &str) {
    let mut todos = load_todos();
    todos.push((text.to_string(), false));
    save_todos(&todos).expect("저장 실패");
    println!("추가됨: {text}");
}

fn cmd_list() {
    let todos = load_todos();
    if todos.is_empty() {
        println!("할 일이 없습니다.");
        return;
    }
    for (i, (text, done)) in todos.iter().enumerate() {
        println!("{} {} {}", i + 1, if *done { "[x]" } else { "[ ]" }, text);
    }
}

fn cmd_done(index: usize) {
    let mut todos = load_todos();
    if index == 0 || index > todos.len() {
        println!("잘못된 번호: {index}");
        return;
    }
    todos[index - 1].1 = true;
    save_todos(&todos).expect("저장 실패");
    println!("완료 처리: {}", todos[index - 1].0);
}

fn print_help() {
    println!("미니 Todo CLI");
    println!();
    println!("사용법:");
    println!("  todo add <할 일>      할 일 추가");
    println!("  todo list             목록 보기");
    println!("  todo done <번호>      완료 처리");
    println!("  todo -v ...           상세 출력");
    println!("  todo --help           도움말");
}

fn main() {
    let args: Vec<String> = env::args().skip(1).collect();
    let parsed = parse_args(args);

    if parsed.help {
        print_help();
        return;
    }

    if parsed.verbose {
        println!("[verbose] 인자: {:?}", parsed.positionals);
    }

    let cmd = parsed.positionals.first().map(String::as_str).unwrap_or("list");
    match cmd {
        "add" => {
            let text = parsed.positionals.get(1).cloned().unwrap_or_default();
            if text.is_empty() {
                println!("내용을 입력하세요: todo add <할 일>");
            } else {
                cmd_add(&text);
            }
        }
        "list" => cmd_list(),
        "done" => {
            let idx = parsed.positionals.get(1).and_then(|s| s.parse().ok()).unwrap_or(0);
            cmd_done(idx);
        }
        "--help" | "-h" => print_help(),
        other => {
            println!("알 수 없는 명령: {other}");
            print_help();
        }
    }
}
