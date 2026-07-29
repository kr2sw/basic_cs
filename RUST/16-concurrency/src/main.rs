// 16-concurrency
// 동시성: thread, mpsc, Mutex, Arc, rayon

use std::sync::mpsc;
use std::sync::{Arc, Mutex};
use std::thread;
use std::time::Duration;

fn main() {
    // --- 기본 스레드 생성 ---
    let handle = thread::spawn(|| {
        for i in 1..5 {
            println!("스레드: {}", i);
            thread::sleep(Duration::from_millis(10));
        }
    });

    for i in 1..3 {
        println!("메인: {}", i);
        thread::sleep(Duration::from_millis(10));
    }

    handle.join().unwrap(); // 스레드 종료 대기
    println!("스레드 종료됨");

    // --- move 클로저로 데이터 소유권 이전 ---
    let data = vec![1, 2, 3];
    let handle = thread::spawn(move || {
        println!("move된 데이터: {:?}", data);
    });
    handle.join().unwrap();
    // println!("{:?}", data); // 이동됨

    // --- mpsc: 메시지 전달 (Multiple Producer Single Consumer) ---
    let (tx, rx) = mpsc::channel();

    let tx1 = tx.clone();
    thread::spawn(move || {
        let messages = vec!["hello", "from", "thread", "1"];
        for msg in messages {
            tx1.send(msg).unwrap();
            thread::sleep(Duration::from_millis(20));
        }
    });

    thread::spawn(move || {
        let messages = vec!["hi", "from", "thread", "2"];
        for msg in messages {
            tx.send(msg).unwrap();
            thread::sleep(Duration::from_millis(25));
        }
    });

    for received in rx {
        println!("수신: {}", received);
    }

    // --- Mutex + Arc: 공유 상태 ---
    let counter = Arc::new(Mutex::new(0));
    let mut handles = vec![];

    for _ in 0..10 {
        let counter = Arc::clone(&counter);
        let handle = thread::spawn(move || {
            // lock()으로 MutexGuard 획득 (자동 해제)
            let mut num = counter.lock().unwrap();
            *num += 1;
        });
        handles.push(handle);
    }

    for handle in handles {
        handle.join().unwrap();
    }

    println!("공유 카운터: {}", *counter.lock().unwrap());

    // --- Send / Sync 트레이트 (개념) ---
    // - Send: 소유권을 스레드 간에 안전하게 전송 가능
    // - Sync: 여러 스레드에서 안전하게 공유 참조 가능 (&T가 Send)
    // Rc<T>는 Send가 아님, Arc<T>는 Send+Sync
    // Mutex<T>는 Sync (T가 Send일 때)

    // --- rayon: 병렬 반복자 ---
    use rayon::prelude::*;

    let numbers: Vec<u64> = (1..=100_000).collect();

    // 순차 합계
    let sum_sequential: u64 = numbers.iter().sum();
    println!("순차 합계: {}", sum_sequential);

    // 병렬 합계 (rayon)
    let sum_parallel: u64 = numbers.par_iter().sum();
    println!("병렬 합계: {}", sum_parallel);

    // 병렬 map
    let squares: Vec<u64> = numbers
        .par_iter()
        .map(|&x| x * x)
        .collect();
    println!("병렬 squares (처음 5개): {:?}", &squares[..5]);

    // 병렬 filter
    let evens: Vec<&u64> = numbers.par_iter().filter(|&&x| x % 2 == 0).collect();
    println!("병렬 짝수 개수: {}", evens.len());
}
