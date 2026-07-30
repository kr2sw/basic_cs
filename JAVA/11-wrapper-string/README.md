# 11: Wrapper & String — 래퍼 클래스와 문자열

## Wrapper 클래스

기본형을 객체로 감싸는 클래스입니다.

| 기본형 | Wrapper |
|--------|---------|
| `byte` | `Byte` |
| `short` | `Short` |
| `int` | `Integer` |
| `long` | `Long` |
| `float` | `Float` |
| `double` | `Double` |
| `char` | `Character` |
| `boolean` | `Boolean` |

### 오토박싱 / 언박싱 (Java 5+)

```java
Integer num = 42;       // 오토박싱: int → Integer
int value = num;        // 언박싱: Integer → int
```

## String, StringBuilder, StringBuffer

| 클래스 | 불변성 | 스레드 안전 | 성능 |
|--------|--------|-----------|------|
| `String` | 불변 | 안전 | 느림 (연산 많을 때) |
| `StringBuffer` | 가변 | 안전 (synchronized) | 보통 |
| `StringBuilder` | 가변 | 불안전 | 빠름 |

- `String`: 짧은 문자열, 문자열 상수, 변경 적을 때
- `StringBuilder`: 단일 스레드에서 문자열 연산 많을 때
- `StringBuffer`: 멀티스레드 환경에서 문자열 연산 많을 때
