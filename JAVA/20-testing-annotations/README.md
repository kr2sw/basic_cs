# 20: Testing & Annotations — 테스트와 어노테이션

## JUnit 5

Java 표준 단위 테스트 프레임워크입니다.

```xml
<!-- pom.xml -->
<dependency>
    <groupId>org.junit.jupiter</groupId>
    <artifactId>junit-jupiter</artifactId>
    <version>5.10.0</version>
    <scope>test</scope>
</dependency>
```

### 주요 어노테이션

| 어노테이션 | 설명 |
|-----------|------|
| `@Test` | 테스트 메서드 |
| `@BeforeEach` | 각 테스트 전 실행 |
| `@AfterEach` | 각 테스트 후 실행 |
| `@BeforeAll` | 모든 테스트 전 (static) |
| `@AfterAll` | 모든 테스트 후 (static) |
| `@DisplayName` | 테스트 이름 지정 |
| `@Disabled` | 테스트 비활성화 |

### 주요 Assertions

| 메서드 | 설명 |
|--------|------|
| `assertEquals(expected, actual)` | 값 비교 |
| `assertTrue(condition)` | 조건이 true |
| `assertFalse(condition)` | 조건이 false |
| `assertNull(obj)` | null 확인 |
| `assertThrows(exception, exec)` | 예외 발생 확인 |

## 커스텀 어노테이션 (Custom Annotation)

```java
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.METHOD)
public @interface MyAnnotation {
    String value() default "";
}
```

## Reflection 기초

런타임에 클래스의 정보를 분석하고 조작하는 기능입니다.
