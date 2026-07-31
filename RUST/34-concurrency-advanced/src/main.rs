// 34: 고급 동시성 — Arc, mpsc channel, AtomicU64

use std::sync::atomic::{AtomicU64, Ordering};
use std::sync::mpsc;
use std::sync::Arc;
use std::thread;
use std::time::Duration;

// 전역 원자 카운터
static GLOBAL_COUNTER: AtomicU64 = AtomicU64::new(0);

// === 1. Arc로 공유 데이터 ===
fn arc_demo() {
    let data = Arc::new(vec![10, 20, 30, 40, 50]);

    let mut handles = Vec::new();
    for i in 0..4 {
        let d = Arc::clone(&data);
        handles.push(thread::spawn(move || {
            let sum: i32 = d.iter().sum::<i32>() + i as i32;
            sum
        }));
    }

    let total: i32 = handles.into_iter().map(|h| h.join().unwrap()).sum();
    println!("Arc 스레드 합계: {total}");
}

// === 2. mpsc 채널 (생산자-소비자) ===
fn channel_demo() {
    let (tx, rx) = mpsc::channel();

    // 생산자 스레드
    let producer = thread::spawn(move || {
        for i in 1..=5 {
            let _ = tx.send(format!("작업-{i}"));
            thread::sleep(Duration::from_millis(50));
        }
    });

    // 메인(소비자)에서 수신
    println!("채널 수신:");
    for msg in rx {
        println!("  받음: {msg}");
    }
    producer.join().unwrap();
}

// === 3. AtomicU64 경쟁 증가 ===
fn atomic_demo() {
    let mut handles = Vec::new();
    for _ in 0..8 {
        handles.push(thread::spawn(|| {
            for _ in 0..1000 {
                GLOBAL_COUNTER.fetch_add(1, Ordering::SeqCst);
            }
        }));
    }
    for h in handles {
        h.join().unwrap();
    }
    println!("Atomic 카운터: {} (기대값 8000)", GLOBAL_COUNTER.load(Ordering::SeqCst));
}

// === 4. 데이터 병렬 처리 (work stealing 느낌) ===
fn parallel_map_demo() {
    let nums: Vec<u32> = (1..=20).collect();
    let chunk_size = 5;

    let (tx, rx) = mpsc::channel();
    let mut handles = Vec::new();

    for chunk in nums.chunks(chunk_size) {
        let tx = tx.clone();
        let data = chunk.to_vec();
        handles.push(thread::spawn(move || {
            let sum: u32 = data.iter().sum();
            tx.send(sum).unwrap();
        }));
    }
    drop(tx);

    let total: u32 = rx.iter().sum();
    for h in handles {
        h.join().unwrap();
    }
    println!("청크 병렬 합: {total}");
}

fn main() {
    println!("=== Arc ===");
    arc_demo();

    println!("\n=== mpsc 채널 ===");
    channel_demo();

    println!("\n=== Atomic ===");
    atomic_demo();

    println!("\n=== 병렬 분할 합 ===");
    parallel_map_demo();
}
