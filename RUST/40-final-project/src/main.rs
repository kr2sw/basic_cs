// 40: 종합 프로젝트 — CLI 할일 관리 앱 (파일 저장)
//
// 실행 예:
//   cargo run -- add "Rust 복습" --priority high --tag study
//   cargo run -- list
//   cargo run -- list --done
//   cargo run -- done 1
//   cargo run -- remove 2
//   cargo run -- stats

use std::collections::HashMap;
use std::io::Write;

const DB_FILE: &str = "tasks.txt";

// === 1. 커스텀 에러 타입 (ch25) ===
#[derive(Debug)]
enum AppError {
    Io(String),
    Format(String),
}

impl std::fmt::Display for AppError {
    fn fmt(&self, f: &mut std::fmt::Formatter) -> std::fmt::Result {
        match self {
            AppError::Io(s) => write!(f, "IO 오류: {s}"),
            AppError::Format(s) => write!(f, "형식 오류: {s}"),
        }
    }
}

impl From<std::io::Error> for AppError {
    fn from(e: std::io::Error) -> Self {
        AppError::Io(e.to_string())
    }
}

type Result<T> = std::result::Result<T, AppError>;

// === 2. 도메인 모델 ===
#[derive(Clone, Debug)]
struct Task {
    id: u64,
    title: String,
    priority: u8,
    tag: String,
    done: bool,
}

impl Task {
    fn to_line(&self) -> String {
        format!("{}|{}|{}|{}|{}", self.id, self.title, self.priority, self.tag, self.done)
    }

    fn from_line(line: &str) -> Result<Self> {
        let parts: Vec<&str> = line.splitn(5, '|').collect();
        if parts.len() != 5 {
            return Err(AppError::Format(format!("잘못된 줄: {line}")));
        }
        Ok(Task {
            id: parts[0].parse().map_err(|_| AppError::Format("id 오류".into()))?,
            title: parts[1].to_string(),
            priority: parts[2].parse().map_err(|_| AppError::Format("priority 오류".into()))?,
            tag: parts[3].to_string(),
            done: parts[4].trim() == "true",
        })
    }
}

// === 3. 저장소 ===
fn load_tasks() -> Result<Vec<Task>> {
    let content = std::fs::read_to_string(DB_FILE).unwrap_or_default();
    content.lines().filter(|l| !l.trim().is_empty()).map(Task::from_line).collect()
}

fn save_tasks(tasks: &[Task]) -> Result<()> {
    let mut f = std::fs::File::create(DB_FILE)?;
    for t in tasks {
        writeln!(f, "{}", t.to_line())?;
    }
    Ok(())
}

// === 4. 커맨드 파싱 ===
enum Command {
    Add { title: String, priority: u8, tag: String },
    List { done: bool },
    Done { id: u64 },
    Remove { id: u64 },
    Stats,
    Help,
}

fn parse_command(args: &[String]) -> Command {
    let cmd = args.first().map(String::as_str).unwrap_or("help");
    match cmd {
        "add" => {
            let title = args.get(1).cloned().unwrap_or_default();
            let mut priority = 3u8;
            let mut tag = "일반".to_string();
            let mut i = 2;
            while i < args.len() {
                match args[i].as_str() {
                    "--priority" => {
                        if let Some(v) = args.get(i + 1) {
                            priority = v.parse().unwrap_or(3);
                            i += 1;
                        }
                    }
                    "--tag" => {
                        if let Some(v) = args.get(i + 1) {
                            tag = v.clone();
                            i += 1;
                        }
                    }
                    _ => {}
                }
                i += 1;
            }
            Command::Add { title, priority, tag }
        }
        "list" => {
            let done = args.iter().any(|a| a == "--done");
            Command::List { done }
        }
        "done" => {
            let id = args.get(1).and_then(|s| s.parse().ok()).unwrap_or(0);
            Command::Done { id }
        }
        "remove" => {
            let id = args.get(1).and_then(|s| s.parse().ok()).unwrap_or(0);
            Command::Remove { id }
        }
        "stats" => Command::Stats,
        _ => Command::Help,
    }
}

// === 5. 명령 실행 ===
fn run(command: Command) -> Result<()> {
    match command {
        Command::Add { title, priority, tag } => {
            if title.is_empty() {
                println!("제목이 비어 있습니다.");
                return Ok(());
            }
            let mut tasks = load_tasks()?;
            // 반복자로 최대 id 계산 (ch23)
            let new_id = tasks.iter().map(|t| t.id).max().unwrap_or(0) + 1;
            tasks.push(Task { id: new_id, title, priority, tag, done: false });
            save_tasks(&tasks)?;
            println!("추가 완료 (#{new_id})");
        }
        Command::List { done } => {
            let tasks = load_tasks()?;
            let filtered: Vec<&Task> = tasks.iter().filter(|t| t.done == done).collect();
            if filtered.is_empty() {
                println!("조건에 맞는 할 일이 없습니다.");
                return Ok(());
            }
            for t in filtered {
                println!(
                    "#{:<3} [{}] (우선순위 {}) <{}> {}",
                    t.id,
                    if t.done { "x" } else { " " },
                    t.priority,
                    t.tag,
                    t.title
                );
            }
        }
        Command::Done { id } => {
            let mut tasks = load_tasks()?;
            if let Some(t) = tasks.iter_mut().find(|t| t.id == id) {
                t.done = true;
                let title = t.title.clone();
                save_tasks(&tasks)?;
                println!("완료 처리: {title}");
            } else {
                println!("id {id}를 찾을 수 없습니다.");
            }
        }
        Command::Remove { id } => {
            let mut tasks = load_tasks()?;
            let before = tasks.len();
            tasks.retain(|t| t.id != id);
            if tasks.len() == before {
                println!("id {id}를 찾을 수 없습니다.");
            } else {
                save_tasks(&tasks)?;
                println!("삭제 완료: id {id}");
            }
        }
        Command::Stats => {
            let tasks = load_tasks()?;
            let total = tasks.len();
            let done_count = tasks.iter().filter(|t| t.done).count();
            let avg_priority: f64 =
                if total == 0 { 0.0 } else { tasks.iter().map(|t| t.priority as f64).sum::<f64>() / total as f64 };
            let mut by_tag: HashMap<&str, usize> = HashMap::new();
            for t in &tasks {
                *by_tag.entry(&t.tag).or_insert(0) += 1;
            }
            println!("총: {total}, 완료: {done_count}, 미완료: {}", total - done_count);
            println!("평균 우선순위: {avg_priority:.1}");
            println!("태그별: {:?}", by_tag);
        }
        Command::Help => {
            println!("사용법:");
            println!("  add <제목> [--priority 1-5] [--tag <태그>]");
            println!("  list [--done]");
            println!("  done <id>");
            println!("  remove <id>");
            println!("  stats");
        }
    }
    Ok(())
}

fn main() {
    let args: Vec<String> = std::env::args().skip(1).collect();
    if let Err(e) = run(parse_command(&args)) {
        eprintln!("오류: {e}");
    }
}
