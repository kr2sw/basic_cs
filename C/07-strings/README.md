# 07: Strings — 문자열

## C 문자열

C는 문자열 타입이 없으며 `char` 배열로 처리합니다.
- 항상 NULL 문자(`\0`)로 끝납니다.
- `char str[] = "Hello";` (6 bytes: H e l l o \0)

## 문자열 함수 (<string.h>)

| 함수 | 설명 |
|------|------|
| `strlen(s)` | 문자열 길이 |
| `strcpy(d, s)` | 문자열 복사 |
| `strcat(d, s)` | 문자열 연결 |
| `strcmp(a, b)` | 비교 (0: 같음) |
| `strchr(s, c)` | 문자 찾기 |
| `strstr(s, sub)` | 부분 문자열 찾기 |
| `sprintf(buf, fmt, ...)` | 서식 문자열 생성 |

## 안전한 함수 (C11)

- `strcpy_s`, `strcat_s`, `strncpy`, `strncat`
