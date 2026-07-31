#include <stdio.h>
#include <stdarg.h>
#include <string.h>

// --- 1. 기본: 개수를 첫 인자로 전달하는 패턴 ---
int sum(int count, ...) {
    va_list ap;
    va_start(ap, count);
    int total = 0;
    for (int i = 0; i < count; i++) {
        total += va_arg(ap, int);
    }
    va_end(ap);
    return total;
}

// --- 2. 최댓값 구하기 (개수 전달 패턴) ---
int maxInt(int count, ...) {
    va_list ap;
    va_start(ap, count);
    int max = va_arg(ap, int);
    for (int i = 1; i < count; i++) {
        int x = va_arg(ap, int);
        if (x > max) max = x;
    }
    va_end(ap);
    return max;
}

// --- 3. NULL 센티널 패턴 (문자열) ---
// 마지막 인자가 NULL이 될 때까지 문자열을 이어 붙임
void printAll(const char* first, ...) {
    va_list ap;
    va_start(ap, first);
    const char* s = first;
    while (s != NULL) {
        printf("[%s] ", s);
        s = va_arg(ap, const char*);
    }
    printf("\n");
    va_end(ap);
}

// --- 4. vprintf 재사용: 서식 문자열 패턴 ---
// printf와 동일한 서식을 받아 내부에서 vprintf로 처리
void logMessage(const char* level, const char* fmt, ...) {
    printf("[%s] ", level);
    va_list ap;
    va_start(ap, fmt);
    vprintf(fmt, ap);   // vprintf: va_list를 받는 printf
    va_end(ap);
    printf("\n");
}

// --- 5. vsnprintf로 동적 문자열 생성 ---
// 반환값: 실제로 필요한 문자 수 (vsprintf 계열의 규칙)
int formatName(char* buf, size_t cap, const char* fmt, ...) {
    va_list ap;
    va_start(ap, fmt);
    int n = vsnprintf(buf, cap, fmt, ap);
    va_end(ap);
    return n;
}

int main() {
    printf("=== 1. 가변 인자 sum ===\n");
    printf("sum(3, 1, 2, 3) = %d\n", sum(3, 1, 2, 3));
    printf("sum(5, 10, 20, 30, 40, 50) = %d\n", sum(5, 10, 20, 30, 40, 50));

    printf("\n=== 2. 가변 인자 maxInt ===\n");
    printf("maxInt(4, 3, 9, 2, 7) = %d\n", maxInt(4, 3, 9, 2, 7));

    printf("\n=== 3. NULL 센티널 패턴 ===\n");
    printAll("사과", "배", "포도", NULL);

    printf("\n=== 4. vprintf 재사용 ===\n");
    logMessage("INFO", "사용자 %s 로그인 (id: %d)", "alice", 42);
    logMessage("ERROR", "값 %d가 범위를 벗어남 (%.2f%%)", 999, 12.5);

    printf("\n=== 5. vsnprintf 동적 문자열 ===\n");
    char buf[64];
    int n = formatName(buf, sizeof(buf), "%s-%d-%s", "user", 7, "kr");
    printf("생성된 문자열: \"%s\" (필요 문자 수: %d)\n", buf, n);

    printf("\n※ 인자 개수/타입이 틀리면 정의되지 않은 동작이므로 신중히 사용하세요.\n");
    return 0;
}
