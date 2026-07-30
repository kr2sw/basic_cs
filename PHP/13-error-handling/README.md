# 13: Error Handling — 오류 처리

## 오류 vs 예외

- **Error**: PHP 엔진 수준 오류 (복구 불가능할 수 있음)
- **Exception**: 프로그램 수준 예외 (복구 가능)

## try-catch-finally (PHP 5+)

```php
try {
    // 예외 발생 가능 코드
} catch (SpecificException $e) {
    // 특정 예외 처리
} catch (Exception $e) {
    // 일반 예외 처리
} finally {
    // 항상 실행
}
```

## 사용자 정의 예외

```php
class MyException extends Exception {
    // 커스텀 예외 클래스
}
```

## Error vs Exception (PHP 7+)

PHP 7부터 대부분의 오류가 `Error` 클래스로 throw됩니다.
`Throwable` 인터페이스는 `Error`와 `Exception`의 공통 부모입니다.
