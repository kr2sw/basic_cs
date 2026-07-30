# 05: Functions — 함수

## 함수 정의

```php
function 함수명(타입 $파라미터 = 기본값): 반환타입 {
    // 코드
    return $값;
}
```

## 특징

- PHP 7+부터 파라미터/반환 타입 선언 가능
- PHP 8+부터 명명된 인자(Named Arguments), Union Types 지원
- `...$args` 가변인자 (Variadic)
- `fn() =>` 화살표 함수 (PHP 7.4+, 짧은 클로저)

## 가변 함수 / 콜백

- 변수에 `()`를 붙여 함수 호출
- `call_user_func()`, `call_user_func_array()`
- 익명 함수 (Closure)
