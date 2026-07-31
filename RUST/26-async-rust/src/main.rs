// 26: 비동기 — async/await, Future 개념, 블로킹 폴링 재현
//
// Rust 표준 라이브러리에는 async 런타임이 없어 tokio 크레이트가 필요합니다.
// 여기서는 Future의 핵심 개념인 "폴링(polling) + 이벤트 루프"를 직접 구현해
// 비동기 메커니즘을 이해합니다. (실제 프로덕션에서는 tokio를 사용하세요)

use std::cell::Cell;
use std::collections::VecDeque;
use std::thread;
use std::time::{Duration, Instant};

// === 1. 작업 상태 표현 ===
#[derive(Debug, PartialEq, Clone, Copy)]
enum TaskState {
    Pending,
    Ready,
    Done,
}

// === 2. 작업(task)의 추상화: 상태를 가지는 클로저 ===
struct Task {
    id: u32,
    name: &'static str,
    done_after_ms: u64,
    state: Cell<TaskState>,
    started: Cell<Option<Instant>>,
}

impl Task {
    fn new(id: u32, name: &'static str, done_after_ms: u64) -> Self {
        Task {
            id,
            name,
            done_after_ms,
            state: Cell::new(TaskState::Pending),
            started: Cell::new(None),
        }
    }

    // 폴링: 아직 안 끝났으면 Pending, 시간이 되면 Ready
    fn poll(&self, now: Instant) -> TaskState {
        match self.state.get() {
            TaskState::Done => TaskState::Done,
            _ => {
                let start = self.started.get().unwrap_or_else(|| {
                    self.started.set(Some(now));
                    now
                });
                if now.duration_since(start) >= Duration::from_millis(self.done_after_ms) {
                    self.state.set(TaskState::Ready);
                    TaskState::Ready
                } else {
                    self.state.set(TaskState::Pending);
                    TaskState::Pending
                }
            }
        }
    }

    fn complete(&self) {
        self.state.set(TaskState::Done);
    }
}

// === 3. 단일 스레드 이벤트 루프 ===
fn event_loop(tasks: Vec<Task>) {
    let mut queue: VecDeque<Task> = tasks.into();
    let started_at = Instant::now();

    println!("=== 이벤트 루프 시작 ===");
    while !queue.is_empty() {
        let n = queue.len();
        for _ in 0..n {
            let task = queue.pop_front().unwrap();
            match task.poll(Instant::now()) {
                TaskState::Ready => {
                    // 실행 준비 완료 → "실행" 후 완료 처리
                    println!(
                        "[+{:.1}ms] task#{} '{}' 완료 (Ready)",
                        started_at.elapsed().as_millis(),
                        task.id,
                        task.name
                    );
                    task.complete();
                }
                TaskState::Pending => {
                    // 아직 준비 안 됨 → 큐 맨 뒤로
                    queue.push_back(task);
                }
                TaskState::Done => { /* 이미 완료 */ }
            }
        }
        // 짧은 휴식 후 다음 폴링 라운드
        thread::sleep(Duration::from_millis(10));
    }
    println!("=== 이벤트 루프 종료 (+{:.1}ms) ===", started_at.elapsed().as_millis());
}

// === 4. 런타임 없이 "블로킹" 폴링 재현 ===
// 실제 tokio의 block_on은 내부적으로 이와 같은 폴링 루프를 돌립니다.
fn block_on_simulate(task: &Task) {
    let started = Instant::now();
    loop {
        match task.poll(Instant::now()) {
            TaskState::Ready => {
                println!(
                    "block_on: '{}' 완료 ({}ms 소요)",
                    task.name,
                    started.elapsed().as_millis()
                );
                task.complete();
                break;
            }
            _ => thread::sleep(Duration::from_millis(5)),
        }
    }
}

fn main() {
    // 서로 다른 지연 시간의 작업들을 동시에 제출
    let tasks = vec![
        Task::new(1, "HTTP 요청 300ms", 300),
        Task::new(2, "DB 쿼리 150ms", 150),
        Task::new(3, "파일 읽기 50ms", 50),
        Task::new(4, "계산 100ms", 100),
    ];

    // 이벤트 루프가 여러 작업을 동시에 진행(concurrency) 처리
    event_loop(tasks);

    // 하나의 작업을 "블로킹"하며 기다리기
    let one = Task::new(10, "단일 블로킹 작업", 120);
    block_on_simulate(&one);

    // 설명: 실제 async/await 는 Future 트레잇의 poll을 통해
    // 이와 동일한 "준비되면 실행" 원리를 사용합니다.
    println!();
    println!("-> Future.pending = Pending(아직 준비 안 됨), Ready(완료)");
    println!("-> 런타임(tokio)이 poll을 반복 호출하는 것은 위 event_loop와 같습니다.");
}
