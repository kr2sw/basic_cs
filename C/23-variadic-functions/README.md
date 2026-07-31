# 23: 가변 인자 — stdarg.h, va_list, 안전한 구현 패턴

## va_list 기본 패턴

함수 인자 개수가 정해지지 않은 함수를 만들 수 있습니다. `stdarg.h`의 매크로를 사용합니다.

```c
#include <stdarg.h>

int sum(int count, ...) {
    va_list ap;
    va_start(ap, count);      // count 다음부터 가변 인자 시작
    for (int i = 0; i < count; i++)
        result += va_arg(ap, int);
    va_end(ap);               // 반드시 정리
    return result;
}
```

- `va_start`, `va_arg`, `va_copy`, `va_end` 네 가지 매크로
- 가변 인자는 기본형 승격(promotion)이 일어남: `float`→`double`, `char`/`short`→`int`

## 안전한 구현 패턴

| 패턴 | 설명 |
|------|------|
| 개수 전달 | 첫 인자로 개수를 명시 (`printf("%d", n)`처럼) |
| 센티널 | 마지막 인자에 `NULL`/`-1` 같은 종료 표시 |
| 서식 문자열 | `printf`처럼 형식을 명시하고 형식에 맞게 읽기 |

```c
int maxInt(int n, ...) { /* 첫 인자는 개수 */ }
void printAll(const char* fmt, ...) { va_list ap; vprintf(fmt, ap); }
```

- `vprintf`, `vsnprintf`처럼 `v`가 붙은 함수로 넘겨 재사용 가능
- 잘못된 타입으로 `va_arg`를 호출하면 **정의되지 않은 동작** (크래시 가능)

## 실행

```bash
gcc main.c -o main && ./main
```
