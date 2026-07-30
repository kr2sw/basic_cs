# 11: Forms & Validation — 폼 처리와 유효성 검사

## 폼 처리

```php
<!-- form.php -->
<form method="POST" action="process.php">
    <input type="text" name="username">
    <input type="email" name="email">
    <input type="submit">
</form>
```

## 유효성 검사

| 함수 | 설명 |
|------|------|
| `filter_var()` | 필터 함수 |
| `filter_input()` | 외부 입력 필터링 |
| `empty()` | 비어있는지 확인 |
| `isset()` | 설정되었는지 확인 |
| `preg_match()` | 정규표현식 검사 |
| `htmlspecialchars()` | XSS 방지 |
| `trim()` | 공백 제거 |
| `strip_tags()` | HTML 태그 제거 |

## 주요 필터

- `FILTER_VALIDATE_EMAIL`, `FILTER_VALIDATE_URL`
- `FILTER_VALIDATE_INT`, `FILTER_VALIDATE_FLOAT`
- `FILTER_SANITIZE_STRING`, `FILTER_SANITIZE_EMAIL`
