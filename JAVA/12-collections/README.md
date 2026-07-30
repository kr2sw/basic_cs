# 12: Collections — 컬렉션 프레임워크

## 컬렉션 계층 구조

```
Collection (인터페이스)
├── List (순서 O, 중복 O)
│   ├── ArrayList (배열 기반, 빠른 조회)
│   └── LinkedList (노드 기반, 빠른 삽입/삭제)
├── Set (순서 X, 중복 X)
│   ├── HashSet (Hash 기반, 빠름)
│   └── TreeSet (정렬, 느림)
└── Queue (FIFO)
    └── LinkedList, PriorityQueue

Map (키-값 쌍)
├── HashMap (Hash 기반, null 허용)
├── LinkedHashMap (입력 순서 유지)
└── TreeMap (정렬)
```

## Iterator

컬렉션의 요소를 순회하는 인터페이스입니다.

## Comparable / Comparator

- `Comparable`: 클래스 자체에 자연 순서 정의 (`compareTo`)
- `Comparator`: 별도의 비교자 클래스 (`compare`)
