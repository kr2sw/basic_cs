// 15-smart-pointers
// 스마트 포인터: Box, Rc, RefCell, Arc, Mutex, Deref, Drop

use std::cell::RefCell;
use std::ops::Deref;
use std::rc::Rc;
use std::sync::{Arc, Mutex};
use std::thread;

// --- Box<T>: 힙 할당 ---
// 크기가 컴파일 타임에 알려지지 않은 타입을 힙에 저장

// 재귀 타입 (크기를 알 수 없음) - Box로 해결
#[derive(Debug)]
#[allow(dead_code)]
enum List {
    Cons(i32, Box<List>),
    Nil,
}

// --- MyBox: Deref 트레이트 구현 ---
struct MyBox<T>(T);

impl<T> MyBox<T> {
    fn new(x: T) -> Self {
        MyBox(x)
    }
}

impl<T> Deref for MyBox<T> {
    type Target = T;

    fn deref(&self) -> &Self::Target {
        &self.0
    }
}

// --- MySmartPointer: Drop 트레이트 구현 ---
struct MySmartPointer {
    data: String,
}

impl Drop for MySmartPointer {
    fn drop(&mut self) {
        println!("MySmartPointer drop: {}", self.data);
    }
}

// --- Rc<T>: 참조 카운팅 (단일 스레드) ---
#[derive(Debug)]
#[allow(dead_code)]
enum RcList {
    Cons(i32, Rc<RcList>),
    Nil,
}

// --- RefCell<T>: 내부 가변성 (interior mutability) ---
// 컴파일 타임에는 불변이지만 런타임에 가변성 허용

trait Messenger {
    fn send(&self, msg: &str);
}

struct LimitTracker<'a, T: Messenger> {
    messenger: &'a T,
    value: usize,
    max: usize,
}

impl<'a, T: Messenger> LimitTracker<'a, T> {
    fn new(messenger: &'a T, max: usize) -> Self {
        Self {
            messenger,
            value: 0,
            max,
        }
    }

    fn set_value(&mut self, value: usize) {
        self.value = value;
        let ratio = self.value as f64 / self.max as f64;
        if ratio >= 1.0 {
            self.messenger.send("초과했습니다!");
        } else if ratio >= 0.9 {
            self.messenger.send("90% 이상 사용");
        } else if ratio >= 0.75 {
            self.messenger.send("75% 이상 사용");
        }
    }
}

struct MockMessenger {
    // RefCell로 불변 참조(&self)에서도 내부 변경 가능
    sent_messages: RefCell<Vec<String>>,
}

impl MockMessenger {
    fn new() -> Self {
        Self {
            sent_messages: RefCell::new(vec![]),
        }
    }
}

impl Messenger for MockMessenger {
    fn send(&self, msg: &str) {
        // borrow_mut: 가변 대여 (런타임에 검사)
        self.sent_messages.borrow_mut().push(String::from(msg));
    }
}

// --- Rc<RefCell<T>> 조합 ---
// 여러 소유권 + 내부 가변성

#[derive(Debug)]
#[allow(dead_code)]
enum RcRefCellList {
    Cons(Rc<RefCell<i32>>, Rc<RcRefCellList>),
    Nil,
}

// --- Arc<Mutex<T>>: 스레드 안전 참조 카운팅 + 상호 배제 ---

fn main() {
    // --- Box ---
    let b = Box::new(5);
    println!("Box 값: {}", b);

    let list = List::Cons(1, Box::new(List::Cons(2, Box::new(List::Nil))));
    println!("Box 리스트: {:?}", list);

    // --- Deref ---
    let my_box = MyBox::new(42);
    // *(my_box.deref())로 자동 변환 (역참조 강제)
    assert_eq!(42, *my_box);
    println!("MyBox 역참조: {}", *my_box);

    // --- Drop ---
    let sp = MySmartPointer {
        data: String::from("테스트"),
    };
    println!("MySmartPointer 생성됨");
    // sp가 범위를 벗어나면 drop 호출
    drop(sp); // 명시적 소멸 (std::mem::drop)
    println!("명시적 drop 후");

    // --- Rc ---
    use crate::RcList::{Cons, Nil};
    let a = Rc::new(Cons(5, Rc::new(Cons(10, Rc::new(Nil)))));
    println!("Rc 참조 카운트: {}", Rc::strong_count(&a));

    let _b = RcList::Cons(3, Rc::clone(&a));
    println!("Rc 참조 카운트: {}", Rc::strong_count(&a));
    {
        let c = RcList::Cons(4, Rc::clone(&a));
        println!("Rc 참조 카운트: {}", Rc::strong_count(&a));
        println!("c: {:?}", c);
    }
    println!("Rc 참조 카운트: {}", Rc::strong_count(&a));

    // --- RefCell ---
    let mock = MockMessenger::new();
    let mut tracker = LimitTracker::new(&mock, 100);
    tracker.set_value(80);
    tracker.set_value(95);
    println!("메시지: {:?}", mock.sent_messages.borrow());

    // RefCell Runtime Borrow Check
    let ref_cell = RefCell::new(vec![1, 2, 3]);
    let mut borrow1 = ref_cell.borrow_mut();
    borrow1.push(4);
    // let borrow2 = ref_cell.borrow_mut(); // 런타임 panic!
    drop(borrow1);
    let borrow2 = ref_cell.borrow();
    println!("RefCell: {:?}", borrow2);

    // --- Rc<RefCell<T>> ---
    let value = Rc::new(RefCell::new(5));
    let list_a = Rc::new(RcRefCellList::Cons(
        Rc::clone(&value),
        Rc::new(RcRefCellList::Nil),
    ));
    let list_b = RcRefCellList::Cons(
        Rc::new(RefCell::new(3)),
        Rc::clone(&list_a),
    );
    let list_c = RcRefCellList::Cons(
        Rc::new(RefCell::new(4)),
        Rc::clone(&list_a),
    );

    // value 변경 - list_a, list_b, list_c 모두 영향 받음
    *value.borrow_mut() += 10;
    println!("list_a: {:?}", list_a);
    println!("list_b: {:?}", list_b);
    println!("list_c: {:?}", list_c);

    // --- Arc<Mutex<T>> ---
    let counter = Arc::new(Mutex::new(0));
    let mut handles = vec![];

    for _ in 0..10 {
        let counter = Arc::clone(&counter);
        let handle = thread::spawn(move || {
            let mut num = counter.lock().unwrap();
            *num += 1;
        });
        handles.push(handle);
    }

    for handle in handles {
        handle.join().unwrap();
    }

    println!("Arc<Mutex> 결과: {}", *counter.lock().unwrap());
}
