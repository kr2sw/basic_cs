# 28: 동적 문자열 — 문자열 빌더 구현, 토큰화, 정규화

## 문자열 빌더 (String Builder)

고정 배열의 한계(길이 고정, 재할당 불가)를 극복하려면 동적 버퍼를 관리해야 합니다.

```c
typedef struct {
    char* buf;      // 동적 버퍼
    size_t len;     // 사용 중인 길이
    size_t cap;     // 버퍼 용량
} StringBuilder;
```

- 부족하면 `realloc`으로 용량을 2배씩 확장
- 매번 `strcat`으로 이어붙이는 것보다 훨씬 빠름
- 항상 `\0`으로 끝나도록 유지

## 토큰화 (Tokenization)

`strtok`는 원본을 수정하고 정적 상태를 사용하므로 스레드에 안전하지 않습니다. C11에서는 `strtok_s`를, POSIX에서는 `strtok_r`을 사용합니다.

```c
char* tok = strtok(buf, " \t\n");
while (tok) { /* 처리 */ tok = strtok(NULL, " \t\n"); }
```

## 정규화 (Normalization)

- 앞뒤 공백 제거 (trim)
- 연속 공백 축소
- 대소문자 통일

```c
char* trim(char* s);     // 앞뒤 공백 제거
char* toLower(char* s);  // 소문자 변환
```

## 실행

```bash
gcc main.c -o main && ./main
```
