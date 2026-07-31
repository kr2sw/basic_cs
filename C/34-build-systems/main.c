#include <stdio.h>

// 빌드 시스템에서 자주 쓰이는 전처리/컴파일 기능 데모

// --- 1. 매크로로 버전/구성 정의 ---
#define APP_VERSION "2.1.0"
#define APP_NAME     "build-demo"

#ifdef NDEBUG
#define LOG_LEVEL "release (NDEBUG 정의됨)"
#else
#define LOG_LEVEL "debug"
#endif

#if defined(_WIN32)
#define PLATFORM "Windows"
#elif defined(__APPLE__)
#define PLATFORM "macOS"
#elif defined(__linux__)
#define PLATFORM "Linux"
#else
#define PLATFORM "Unknown"
#endif

// --- 2. 컴파일러가 제공하는 매크로 ---
// __FILE__, __LINE__, __DATE__, __TIME__, __STDC__
void showBuiltinMacros(void) {
    printf("컴파일 파일: %s\n", __FILE__);
    printf("이 함수 라인: %d\n", __LINE__);
    printf("빌드 날짜: %s %s\n", __DATE__, __TIME__);
#ifdef __STDC_VERSION__
    printf("표준 버전: C%d (1990=89, 1999=C99, 2011=C11)\n", __STDC_VERSION__ / 1000000);
#endif
}

// --- 3. #include 가드와 static inline (헤더 패턴) ---
// 실제 프로젝트에서는 헤더 파일에 넣는 패턴
#ifndef UTIL_H
#define UTIL_H

static inline int clamp(int v, int lo, int hi) {
    if (v < lo) return lo;
    if (v > hi) return hi;
    return v;
}

#endif

// --- 4. _Static_assert (C11): 컴파일 타임 검증 ---
_Static_assert(sizeof(int) >= 2, "int는 최소 2바이트여야 함");
_Static_assert(sizeof(long) >= 4, "long은 최소 4바이트여야 함");

typedef struct {
    int x;
    int y;
} Point;
_Static_assert(sizeof(Point) == 2 * sizeof(int), "Point 크기는 예상과 달라서는 안 됨");

// --- 5. 라이브러리 개념: 오브젝트 파일로 분리되는 함수들 ---
// 원래 프로젝트에서는 util.c에 분리되고 Makefile이 .o로 링크합니다.
int utilAdd(int a, int b) { return a + b; }
double utilAvg(int a, int b) { return (a + b) / 2.0; }

// --- 6. 프로파일링/트레이스용 매크로 패턴 ---
#ifdef TRACE
#define TRACE_MSG(msg) printf("[TRACE] %s\n", msg)
#else
#define TRACE_MSG(msg) ((void)0)
#endif

int main(void) {
    printf("=== 빌드 시스템 데모 ===\n");
    printf("앱: %s v%s (%s)\n", APP_NAME, APP_VERSION, LOG_LEVEL);
    printf("플랫폼: %s\n", PLATFORM);

    printf("\n=== 컴파일러 매크로 ===\n");
    showBuiltinMacros();

    printf("\n=== static inline / static_assert ===\n");
    printf("clamp(150, 0, 100) = %d\n", clamp(150, 0, 100));
    printf("utilAdd(3, 4) = %d, utilAvg(3, 4) = %.1f\n",
           utilAdd(3, 4), utilAvg(3, 4));

    TRACE_MSG("트레이스 활성화됨 (TRACE 매크로 정의 시 출력)");

    printf("\n=== 빌드 절차 (실제 프로젝트) ===\n");
    printf("gcc -c util.c   → util.o\n");
    printf("gcc -c main.c   → main.o\n");
    printf("gcc main.o util.o -o app   → 링크\n");
    printf("  (이 단계들을 Makefile이 자동화합니다)\n");

    return 0;
}
