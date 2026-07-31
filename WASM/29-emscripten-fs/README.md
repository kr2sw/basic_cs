# 29: Emscripten 파일 시스템 — FS, IDBFS, MEMFS

Emscripten은 C 표준 파일 API(`fopen`, `fread`, ...)를 브라우저에서 동작하게 만드는 가상 파일 시스템을 제공합니다. `FS` 객체로 마운트를 제어하고, 백엔드를 바꿔가며 사용할 수 있습니다.

## 파일 시스템 종류

| 백엔드 | 특징 |
|--------|------|
| `MEMFS` | 메모리 위에 구현 (기본값). 빠르지만 새로고침 시 사라짐 |
| `IDBFS` | IndexedDB에 영속화. `FS.syncfs`로 동기화 |
| `NODEFS` | Node.js의 실제 파일 시스템 |
| `WORKERFS` | 워커에서 Blob/File을 파일처럼 사용 |

## C 코드에서 사용

```c
#include <stdio.h>

FILE *f = fopen("/hello.txt", "w");        // MEMFS에 파일 생성
fputs("Hello from MEMFS!\n", f);
fclose(f);

f = fopen("/hello.txt", "r");
char buf[128];
fgets(buf, sizeof(buf), f);                // 다시 읽기
fclose(f);
```

## IDBFS 동기화

영속화가 필요하면 `/persistent` 디렉터리에 IDBFS를 마운트하고 `FS.syncfs`로 저장/복원합니다.

```c
#include <emscripten/emscripten.h>

EM_ASM(
  FS.mkdir('/persistent');
  FS.mount(IDBFS, {}, '/persistent');
  FS.syncfs(true, function(err) {        // true = 저장
    if (err) console.error(err);
  });
);
```

## JS에서 접근

Emscripten 모듈의 `FS` 전역 객체로 JS에서도 같은 파일을 다룰 수 있습니다.

```js
Module.FS.writeFile('/hello.txt', 'from JS');
console.log(Module.FS.readFile('/hello.txt', { encoding: 'utf8' }));
```

## 실행

```bash
emcc fs_example.c -o fs.html -sUSE_IDBFS=1 -sFORCE_FILESYSTEM=1
npx http-server .
```

컴파일 플래그:

- `-sUSE_IDBFS=1`: IDBFS 지원 포함
- `-sFORCE_FILESYSTEM=1`: FS 코드를 트리 생략 없이 포함
- `-sEXPORTED_RUNTIME_METHODS=FS,IDBFS`: JS에서 `FS` 사용
