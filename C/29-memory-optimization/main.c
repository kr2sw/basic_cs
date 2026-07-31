#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

#define POOL_BLOCK_COUNT 64
#define POOL_BLOCK_SIZE  32

// --- 1. 메모리 풀 (고정 크기 블록) ---
typedef struct {
    char* blocks;
    int freeCount;
    int* freeList;    // 빈 블록 인덱스 스택
    int usedCount;
} MemoryPool;

void poolInit(MemoryPool* pool) {
    pool->blocks = (char*)malloc(POOL_BLOCK_COUNT * POOL_BLOCK_SIZE);
    pool->freeList = (int*)malloc(POOL_BLOCK_COUNT * sizeof(int));
    pool->freeCount = POOL_BLOCK_COUNT;
    pool->usedCount = 0;
    for (int i = 0; i < POOL_BLOCK_COUNT; i++) {
        pool->freeList[i] = i;    // 처음엔 전부 빈 블록
    }
}

void* poolAlloc(MemoryPool* pool) {
    if (pool->freeCount == 0) return NULL;   // 풀 소진
    int idx = pool->freeList[--pool->freeCount];
    pool->usedCount++;
    return pool->blocks + idx * POOL_BLOCK_SIZE;
}

void poolFree(MemoryPool* pool, void* p) {
    char* addr = (char*)p;
    if (addr < pool->blocks ||
        addr >= pool->blocks + POOL_BLOCK_COUNT * POOL_BLOCK_SIZE) {
        return;   // 이 풀에서 나온 포인터가 아님
    }
    int idx = (int)((addr - pool->blocks) / POOL_BLOCK_SIZE);
    pool->freeList[pool->freeCount++] = idx;
    pool->usedCount--;
}

void poolDestroy(MemoryPool* pool) {
    free(pool->blocks);
    free(pool->freeList);
}

// --- 2. 커스텀 할당자 (추적 래퍼) ---
static int allocCount = 0;

void* myAlloc(size_t size) {
    allocCount++;
    return malloc(size);
}

void myFree(void* p) {
    if (p) {
        allocCount--;
        free(p);
    }
}

// --- 3. 캐시 친화적 순회 비교 ---
#define MATRIX_N 2048

void rowMajorSum(double mat[MATRIX_N][MATRIX_N]) {
    double sum = 0.0;
    for (int i = 0; i < MATRIX_N; i++) {
        for (int j = 0; j < MATRIX_N; j++) {
            sum += mat[i][j];        // 연속 메모리 접근
        }
    }
    printf("행 우선 합: %.2f\n", sum);
}

void colMajorSum(double mat[MATRIX_N][MATRIX_N]) {
    double sum = 0.0;
    for (int j = 0; j < MATRIX_N; j++) {
        for (int i = 0; i < MATRIX_N; i++) {
            sum += mat[i][j];        // 캐시 라인을 매번 건너뜀
        }
    }
    printf("열 우선 합: %.2f\n", sum);
}

double timeIt(void (*fn)(double mat[MATRIX_N][MATRIX_N]), double mat[MATRIX_N][MATRIX_N]) {
    clock_t s = clock();
    fn(mat);
    clock_t e = clock();
    return (double)(e - s) * 1000.0 / CLOCKS_PER_SEC;
}

int main() {
    printf("=== 1. 메모리 풀 ===\n");
    MemoryPool pool;
    poolInit(&pool);

    char* objs[10];
    for (int i = 0; i < 10; i++) {
        objs[i] = (char*)poolAlloc(&pool);
        strcpy(objs[i], "pool object");
    }
    printf("할당된 객체: %d, 사용 중: %d\n", 10, pool.usedCount);
    printf("예시 데이터: %s\n", objs[3]);
    for (int i = 0; i < 10; i++) poolFree(&pool, objs[i]);
    printf("전부 반환 후 사용 중: %d (재사용 가능)\n", pool.usedCount);
    poolDestroy(&pool);

    printf("\n=== 2. 커스텀 할당자 (추적) ===\n");
    int* a = (int*)myAlloc(10 * sizeof(int));
    int* b = (int*)myAlloc(20 * sizeof(int));
    printf("현재 활성 할당: %d\n", allocCount);
    myFree(a);
    myFree(b);
    printf("해제 후: %d (0이면 누수 없음)\n", allocCount);

    printf("\n=== 3. 캐시 친화적 순회 (N=%d) ===\n", MATRIX_N);
    double(*mat)[MATRIX_N] = (double(*)[MATRIX_N])malloc(MATRIX_N * MATRIX_N * sizeof(double));
    for (int i = 0; i < MATRIX_N; i++) {
        for (int j = 0; j < MATRIX_N; j++) mat[i][j] = 0.000001;
    }

    double tRow = timeIt(rowMajorSum, mat);
    double tCol = timeIt(colMajorSum, mat);
    printf("행 우선(row-major): %.0f ms\n", tRow);
    printf("열 우선(column-major): %.0f ms\n", tCol);
    printf("속도 차이: %.1f배 (연속 접근이 캐시에 유리)\n", tCol / (tRow ? tRow : 1.0));
    free(mat);

    return 0;
}
