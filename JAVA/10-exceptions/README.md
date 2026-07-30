# 10: Exceptions — 예외 처리

## 예외 계층 구조

```
Object → Throwable
          ├── Error (JVM 레벨, 복구 불가)
          └── Exception
               ├── RuntimeException (Unchecked)
               └── IOException, SQLException 등 (Checked)
```

## try-catch-finally

```java
try {
    // 예외 발생 가능 코드
} catch (SpecificException e) {
    // 예외 처리
} catch (Exception e) {
    // 여러 catch 가능 (상위는 아래로)
} finally {
    // 항상 실행 (생략 가능)
}
```

## try-with-resources (Java 7+)

`AutoCloseable` 구현체를 자동으로 close합니다.

```java
try (BufferedReader br = new BufferedReader(new FileReader("file.txt"))) {
    // 사용 후 자동 close
}
```

## throws / throw

- `throws`: 메서드가 예외를 호출자에게 전가
- `throw`: 직접 예외 발생
