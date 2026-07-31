# 30: Emscripten 고급 — pthreads, Optimize 플래그

Emscripten은 Web Worker 기반으로 POSIX 스레드(`pthread.h`)를 지원합니다. 공유 메모리(WASM threads)가 바탕이 되므로 컴파일 시 `-pthread`를 켜야 하고, 이 경우 SharedArrayBuffer가 사용됩니다.

## pthreads 예제

```c
#include <pthread.h>

static pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
static int counter = 0;

void *worker(void *arg) {
  for (int i = 0; i < 100000; i++) {
    pthread_mutex_lock(&mutex);
    counter++;
    pthread_mutex_unlock(&mutex);
  }
  return NULL;
}
```

```bash
emcc pthreads.c -o pthreads.html -pthread -sPTHREAD_POOL_SIZE=4
```

- `-pthread`: 스레드 + 공유 메모리 지원
- `-sPTHREAD_POOL_SIZE=4`: 시작 시 4개 워커 생성(풀)
- `-sPTHREAD_POOL_SIZE_STRICT=2`: 생성 시 부족한 워커 추가 생성 허용

## 최적화 플래그

| 플래그 | 용도 |
|--------|------|
| `-O0` | 디버그 (속도 느림, 크기 큼) |
| `-O2` | 기본 최적화 |
| `-O3` | 적극적 최적화 |
| `-Os` | 크기 우선 |
| `-Oz` | 최대 크기 축소 |
| `--closure 1` | JS 코드 Closure 압축 |
| `-sMODULARIZE=1` | JS 모듈화 |
| `-sALLOW_MEMORY_GROWTH=1` | 런타임 메모리 확장 허용 |

## 이진 최적화 (wasm-opt)

```bash
wasm-opt pthreads.wasm -O3 -o pthreads.opt.wasm
wasm-strip pthreads.opt.wasm
```

## 실행

```bash
emcc pthreads.c -o pthreads.html -pthread -sPTHREAD_POOL_SIZE=4 -O3
npx http-server .
```

주의: `-pthread`를 쓰면 브라우저에서 SharedArrayBuffer를 위해 COOP/COEP 헤더가 필요합니다.
