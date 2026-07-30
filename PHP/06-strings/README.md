# 06: Strings — 문자열 처리

## 문자열 함수

PHP는 풍부한 문자열 내장 함수를 제공합니다.

| 함수 | 설명 |
|------|------|
| `strlen()` | 문자열 길이 |
| `strpos()` / `strrpos()` | 위치 찾기 |
| `substr()` | 부분 문자열 |
| `str_replace()` | 문자열 치환 |
| `strtolower()` / `strtoupper()` | 대소문자 변환 |
| `trim()` / `ltrim()` / `rtrim()` | 공백 제거 |
| `explode()` / `implode()` | 분할/결합 |
| `str_split()` | 문자 배열로 분할 |
| `nl2br()` | 개행 → `<br>` 변환 |
| `htmlspecialchars()` | HTML 특수문자 이스케이프 |
| `sprintf()` / `printf()` | 서식 출력 |

## Heredoc / Nowdoc

- **Heredoc**: `<<<EOD` ... `EOD;` (변수 파싱)
- **Nowdoc**: `<<<'EOD'` ... `EOD;` (변수 파싱 안 함)
