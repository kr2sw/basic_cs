# 34: Testing Advanced — 고급 테스팅

## Mockito 스타일 Mock

외부 의존성을 가짜(mock)로 대체해 단위 테스트를 격리합니다.

```java
OrderRepository repo = Mockito.mock(OrderRepository.class);
Mockito.when(repo.findById(1L)).thenReturn(Optional.of(order));
Mockito.verify(repo).findById(1L);   // 호출 검증
```

| Mockito 기능 | 의미 |
|--------------|------|
| `mock(클래스)` | 목 객체 생성 |
| `when(...).thenReturn(...)` | 동작 지정 (stub) |
| `verify(mock).메서드()` | 호출 여부 검증 |
| `spy(객체)` | 실제 객체의 부분 목 |
| `@Mock`, `@InjectMocks` | 어노테이션 주입 |

## AssertJ 체이닝

가독성 좋은 assert 문을 체이닝으로 사용합니다.

```java
assertThat(result)
    .isNotNull()
    .hasSize(3)
    .contains("Java")
    .allMatch(s -> s.length() > 2);
```

## 파라미터 테스트

같은 검증을 여러 입력으로 반복합니다.

```java
@ParameterizedTest
@CsvSource({"1,2,3", "10,20,30"})
void add(int a, int b, int expected) {
    assertThat(calculator.add(a, b)).isEqualTo(expected);
}
```

## 테스트 피라미드

```
     /\   E2E (소수)
    /  \
   /----\  통합 테스트
  /------\
 / Unit   \  단위 테스트 (다수, 빠름)
```

## 실행

```bash
cd JAVA/34-testing-advanced
javac Main.java && java Main
```

> JUnit/Mockito/AssertJ 없이 mock, 검증, 파라미터 테스트의 원리를 직접 구현해 봅니다.
