# 07 Collections — 컬렉션

러스트 표준 라이브러리의 주요 컬렉션 타입: Vec, HashMap, HashSet, String, VecDeque.

## 주요 개념
- `Vec<T>`: 가변 배열 — push, pop, sort, dedup, get (안전 접근)
- `HashMap<K, V>`: 키-값 저장 — entry API, or_insert
- `HashSet<T>`: 중복 없는 집합 — 합/교/차집합 연산
- `String`: UTF-8 문자열 — push_str, format!, chars(), bytes()
- `VecDeque<T>`: 양방향 큐 — push_front/back, rotate_left/right
- 이종 컬렉션: enum으로 여러 타입을 벡터에 저장

```rust
let mut scores = HashMap::new();
scores.insert(String::from("Blue"), 100);
scores.entry(String::from("Red")).or_insert(200);

let mut set = HashSet::new();
set.insert(1);
let union: HashSet<_> = set_a.union(&set_b).copied().collect();

let mut deque = VecDeque::new();
deque.push_back(1);
deque.push_front(0);
```

## 실행
```bash
cd RUST/07-collections && cargo run
```

## 핵심 요점
- `Vec::get()`은 범위 초과 시 `None` 반환 (안전 접근)
- HashMap의 `entry` API로 키 존재 여부에 따른 삽입
- HashSet은 집합 연산 (union, intersection 등) 지원
- 문자열은 UTF-8, 인덱싱 대신 chars()/bytes() 반복
- `VecDeque`는 앞/뒤 양방향 추가/제거에 최적화
