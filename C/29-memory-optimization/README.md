# 29: 메모리 최적화 — 메모리 풀, 커스텀 할당자, 캐시 친화적 코드

## 메모리 풀 (Memory Pool)

미리 큰 블록을 할당해 두고 작은 객체를 고정 크기 블록으로 나눠 재사용합니다. `malloc` 호출 횟수를 줄여 단편화와 오버헤드를 줄입니다.

```c
typedef struct {
    void* block;     // 미리 할당한 메모리
    size_t blockSize;
    int* freeList;   // 빈 블록 인덱스 스택
} MemoryPool;
```

- 빈 블록 관리는 프리 리스트(연결 리스트 또는 스택)로
- 게임, 임베디드, 네트워크 서버 등에서 자주 사용

## 커스텀 할당자 (Custom Allocator)

`malloc`/`free`를 감싸서 할당 횟수 추적, 크기 검증, 정렬 보장 등을 추가합니다.

```c
void* myAlloc(size_t size) { allocCount++; return malloc(size); }
void myFree(void* p) { allocCount--; free(p); }
```

- 디버깅용 래퍼, 정렬 요구(align)가 있는 경우에 유용
- C11에는 `aligned_alloc`이 표준으로 존재

## 캐시 친화적 코드 (Cache-Friendly)

배열을 **행 우선(row-major)으로 순회**하면 메모리 접근이 연속적이라 CPU 캐시 효율이 높습니다.

```c
for (int i = 0; i < N; i++)          // 행 우선: cache hit
    for (int j = 0; j < N; j++)
        sum += mat[i][j];
```

## 실행

```bash
gcc main.c -o main && ./main
```
