# 30: 시그널과 오류 처리 — errno, strerror, assert, abort

## errno와 오류 함수

표준 라이브러리 함수는 실패 시 `errno`(전역 오류 번호)를 설정합니다.

```c
#include <errno.h>
#include <string.h>

FILE* fp = fopen("없는파일.txt", "r");
if (!fp) {
    printf("오류 번호: %d\n", errno);
    printf("오류 메시지: %s\n", strerror(errno));
    perror("fopen");   // "fopen: No such file or directory"
}
```

- `strerror(errno)`: 오류 번호 → 사람이 읽을 수 있는 메시지
- `perror(prefix)`: prefix + 메시지를 표준 오류로 출력

## assert와 abort

```c
#include <assert.h>

assert(조건);   // 조건이 거짓이면 메시지 출력 후 abort() 호출
abort();        // 비정상 종료 (SIGABRT 발생)
```

- `NDEBUG` 매크로를 정의하면 `assert`가 비활성화됨 (배포 빌드에서)
- `assert`는 불변식(invariant) 검증용이지 오류 처리 대체가 아님

## 시그널 (signal/raise)

C 표준의 `<signal.h>`로 간단한 시그널 처리 가능.

```c
void handler(int sig) { printf("시그널 %d 수신\n", sig); }
signal(SIGINT, handler);   // Ctrl+C 등
raise(SIGINT);             // 시그널 직접 발생
```

## 실행

```bash
gcc main.c -o main && ./main
```
