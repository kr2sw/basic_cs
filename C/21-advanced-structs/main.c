#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// --- 1. 비트 필드 (Bit Field) ---
// 3비트씩만 사용해 RGB 값을 담는 구조체
typedef struct {
    unsigned int red   : 3;   // 0~7
    unsigned int green : 3;
    unsigned int blue  : 3;
} RGB3;

// 1비트 플래그 구조체 (상태 플래그용)
typedef struct {
    unsigned int isAlive  : 1;
    unsigned int isActive : 1;
    unsigned int hasAdmin : 1;
    unsigned int reserved : 29;
} StatusFlags;

// --- 2. union ---
// 같은 메모리를 여러 타입으로 해석
typedef union {
    int  i;
    float f;
    unsigned char bytes[4];
} Value;

// --- 3. 열거형 심화 ---
typedef enum {
    COLOR_RED = 1,   // 명시적 값
    COLOR_GREEN,     // 자동으로 2
    COLOR_BLUE,      // 3
    COLOR_MAX
} Color;

// 비트 플래그 열거형
typedef enum {
    PERM_READ  = 1 << 0,
    PERM_WRITE = 1 << 1,
    PERM_EXEC  = 1 << 2
} Permission;

// --- 4. 유연 배열 멤버 (Flexible Array Member) ---
typedef struct {
    int len;
    int data[];   // 크기가 없는 배열 (C99+)
} IntVec;

IntVec* intVecCreate(int n) {
    IntVec* v = (IntVec*)malloc(sizeof(IntVec) + n * sizeof(int));
    v->len = n;
    return v;
}

void intVecDestroy(IntVec* v) {
    free(v);
}

int main() {
    printf("=== 1. 비트 필드 ===\n");
    RGB3 rgb = {5, 6, 7};
    printf("RGB(%d, %d, %d) 크기: %zu 바이트\n", rgb.red, rgb.green, rgb.blue, sizeof(rgb));

    StatusFlags flags;
    memset(&flags, 0, sizeof(flags));
    flags.isAlive  = 1;
    flags.isActive = 1;
    printf("flags: alive=%d active=%d admin=%d, 크기: %zu 바이트\n",
           flags.isAlive, flags.isActive, flags.hasAdmin, sizeof(flags));

    printf("\n=== 2. union ===\n");
    Value v;
    v.f = 1.5f;
    printf("float로 읽기: %.1f\n", v.f);
    printf("int로 해석: %d (0x%08X)\n", v.i, v.i);
    printf("바이트 순서(엔디언): ");
    for (int i = 0; i < 4; i++) {
        printf("%02X ", v.bytes[i]);
    }
    printf("\nunion 크기: %zu (가장 큰 멤버 기준)\n", sizeof(v));

    printf("\n=== 3. 열거형 심화 ===\n");
    printf("COLOR_GREEN = %d, COLOR_BLUE = %d\n", COLOR_GREEN, COLOR_BLUE);

    int perm = PERM_READ | PERM_EXEC;   // 비트 OR로 여러 권한 조합
    printf("권한: %s\n", (perm & PERM_WRITE) ? "쓰기 가능" : "쓰기 불가");
    printf("권한: %s\n", (perm & PERM_EXEC) ? "실행 가능" : "실행 불가");

    printf("\n=== 4. Flexible Array Member ===\n");
    IntVec* vec = intVecCreate(5);
    for (int i = 0; i < vec->len; i++) {
        vec->data[i] = i * 10;
    }
    printf("vec 크기(헤더만): %zu\n", sizeof(IntVec));
    printf("vec 데이터: ");
    for (int i = 0; i < vec->len; i++) {
        printf("%d ", vec->data[i]);
    }
    printf("\n");
    intVecDestroy(vec);

    printf("\n※ 비트 필드/유연 배열의 정확한 메모리 배치는 컴파일러마다 다를 수 있습니다.\n");
    return 0;
}
