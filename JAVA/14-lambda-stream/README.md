# 14: Lambda & Stream — 람다와 스트림

## 람다 표현식 (Lambda Expression)

Java 8에 도입된 함수형 프로그래밍 방식입니다.

```java
(파라미터) -> { 본문 }
// 예: (x, y) -> x + y
```

## 함수형 인터페이스 (Functional Interface)

추상 메서드가 1개인 인터페이스입니다. (`@FunctionalInterface`)

| 인터페이스 | 메서드 | 설명 |
|-----------|--------|------|
| `Predicate<T>` | `boolean test(T)` | 조건 검사 |
| `Consumer<T>` | `void accept(T)` | 소비 (출력 등) |
| `Function<T,R>` | `R apply(T)` | 변환 |
| `Supplier<T>` | `T get()` | 공급 |
| `BinaryOperator<T>` | `T apply(T,T)` | 이항 연산 |

## Stream API

컬렉션/배열의 데이터를 함수형으로 처리합니다.

### 주요 중간 연산
- `filter()`, `map()`, `flatMap()`, `sorted()`, `distinct()`, `limit()`, `skip()`

### 주요 최종 연산
- `collect()`, `forEach()`, `count()`, `reduce()`, `anyMatch()`, `allMatch()`, `findFirst()`

## Optional

`NullPointerException`을 방지하는 컨테이너 클래스입니다.
