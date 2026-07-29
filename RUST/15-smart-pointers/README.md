# 15 Smart Pointers — 스마트 포인터

Box, Rc, RefCell, Arc, Mutex, Deref, Drop 등 스마트 포인터 심층 탐구.

## 주요 개념
- `Box<T>`: 힙 할당, 재귀 타입에 필수
- `Deref` 트레이트: 역참조 연산자 `*` 동작 정의, 역참조 강제
- `Drop` 트레이트: 범위 벗어날 때 정리 코드 실행
- `Rc<T>`: 참조 카운팅 (단일 스레드), 여러 소유자 가능
- `RefCell<T>`: 내부 가변성 — 런타임 대여 검사
- `Rc<RefCell<T>>`: 여러 소유권 + 내부 가변성 조합
- `Arc<Mutex<T>>`: 스레드 안전 참조 카운팅 + 상호 배제

```rust
let list = List::Cons(1, Box::new(List::Cons(2, Box::new(List::Nil))));

let a = Rc::new(Cons(5, Rc::new(Cons(10, Rc::new(Nil)))));
let _b = RcList::Cons(3, Rc::clone(&a));

let mock = MockMessenger::new();
mock.sent_messages.borrow_mut().push(String::from(msg));

let counter = Arc::new(Mutex::new(0));
```

## 실행
```bash
cd RUST/15-smart-pointers && cargo run
```

## 핵심 요점
- `Box<T>`는 단일 소유권 힙 값, `Rc<T>`는 여러 소유권 가능
- `RefCell<T>`는 컴파일 타임이 아닌 런타임에 대여 규칙 검사
- `Arc<Mutex<T>>`는 스레드 간 안전한 공유 상태 구현
- 스마트 포인터는 `Deref`와 `Drop` 트레이트로 동작
