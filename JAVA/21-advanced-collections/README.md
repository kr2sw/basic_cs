# 21: Advanced Collections — 고급 컬렉션과 스트림

## Comparator 심화

`Comparator`는 정렬 기준을 유연하게 정의하는 함수형 인터페이스입니다.

```java
list.sort(Comparator.comparing(Employee::name)
    .thenComparing(Employee::age)      // 2차 정렬
    .reversed());                       // 역순
```

- `comparing`, `thenComparing` 으로 복합 정렬
- `reversed()` 로 역순 정렬
- `nullsFirst()`, `nullsLast()` 로 null 처리
- `naturalOrder()`, `reverseOrder()` 로 기본 순서

## groupingBy (그룹핑)

`Collectors.groupingBy` 로 키 기준 그룹핑을 수행합니다.

```java
Map<String, List<Employee>> byDept = employees.stream()
    .collect(Collectors.groupingBy(Employee::dept));
```

하위 수집기(downstream)로 그룹 데이터를 가공할 수 있습니다.

| 하위 수집기 | 설명 |
|------------|------|
| `counting()` | 그룹별 개수 |
| `summingInt(...)` | 그룹별 합계 |
| `averagingDouble(...)` | 그룹별 평균 |
| `mapping(변환, collector)` | 그룹 내 요소 변환 |
| `maxBy(comparator)` | 그룹별 최대값 |

`partitioningBy` 는 `true/false` 두 그룹으로 나누는 특수한 형태입니다.

## parallelStream (병렬 스트림)

```java
long sum = LongStream.rangeClosed(1, 1_000_000)
    .parallel()                       // 병렬 처리
    .sum();
```

- `parallelStream()` 또는 `.parallel()` 로 병렬 처리
- 큰 데이터, 독립적인 연산에 효과적
- 공유 가변 상태(shared mutable state)는 동기화가 필요하므로 주의

## 고급 Collectors

- `joining("구분자")` : 문자열 결합
- `toMap(키, 값)` : Map 생성
- `collectingAndThen` : 수집 후 후처리
- `teeing` : 두 수집기 결과 합치기 (Java 12+)

## 실행

```bash
cd JAVA/21-advanced-collections
javac Main.java && java Main
```
