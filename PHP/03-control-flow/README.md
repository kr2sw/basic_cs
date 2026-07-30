# 03: Control Flow — 조건문과 반복문

## 조건문

### if / elseif / else
```php
if (조건) {
    // 코드
} elseif (다른 조건) {
    // 코드
} else {
    // 코드
}
```

### switch
```php
switch ($value) {
    case 1:
        // 코드
        break;
    default:
        // 코드
}
```

### match (PHP 8+)
PHP 8.0부터 도입된 표현식 기반 조건문입니다.

## 반복문

- **for**: 반복 횟수가 정해져 있을 때
- **while**: 조건이 true인 동안
- **do-while**: 최소 1회 실행
- **foreach**: 배열 순회에 특화

## break / continue / goto
