#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include <assert.h>
#include <signal.h>

// --- 1. errno / strerror / perror ---
void demoErrno(void) {
    errno = 0;                     // 항상 먼저 0으로 초기화
    FILE* fp = fopen("no_such_file.txt", "r");
    if (!fp) {
        printf("errno = %d\n", errno);
        printf("strerror: %s\n", strerror(errno));
        perror("perror -> fopen");
    } else {
        fclose(fp);
    }
}

// --- 2. 시그널 처리 (C 표준 signal/raise) ---
static int signalCount = 0;

void onSignal(int sig) {
    signalCount++;
    printf("  [핸들러] 시그널 %d 수신 (count=%d)\n", sig, signalCount);
    // 핸들러에서 돌아오면 프로그램은 계속 실행됩니다.
}

void demoSignal(void) {
    signal(SIGINT, onSignal);      // SIGINT: Ctrl+C
    printf("raise(SIGINT) 발생...\n");
    raise(SIGINT);
    printf("raise(SIGINT) 발생...\n");
    raise(SIGINT);
    printf("시그널 핸들러가 처리 후 프로그램 계속 실행됨 (총 %d회)\n", signalCount);
}

// --- 3. assert (불변식 검증) ---
int divide(int a, int b) {
    assert(b != 0);                // 0으로 나누기 방지
    return a / b;
}

void demoAssert(void) {
    printf("divide(10, 2) = %d\n", divide(10, 2));
    printf("divide(7, 1) = %d\n", divide(7, 1));

    printf("\n※ assert(0으로 나누기)는 실패 시 abort()로 종료되므로 주석 처리:\n");
    // printf("divide(10, 0) = %d\n", divide(10, 0));  // 이 줄은 실행하면 중단됨
}

// --- 4. abort / atexit ---
void onExit(void) {
    printf("atexit: 정상 종료 시 실행되는 콜백\n");
}

void demoExitHandling(void) {
    atexit(onExit);
    printf("main이 return 하면 atexit 콜백이 실행됩니다.\n");
    printf("\n※ abort()는 SIGABRT를 발생시켜 비정상 종료하므로 주석 처리:\n");
    // abort();   // 이 줄을 살리면 프로그램이 강제 종료됨
}

int main() {
    printf("=== 1. errno 오류 처리 ===\n");
    demoErrno();

    printf("\n=== 2. 시그널 처리 ===\n");
    demoSignal();

    printf("\n=== 3. assert ===\n");
    demoAssert();

    printf("\n=== 4. abort / atexit ===\n");
    demoExitHandling();

    printf("\n오류 처리 규칙: errno는 성공한 함수가 건드리지 않으므로\n");
    printf("사용 전에 반드시 errno = 0으로 초기화하세요.\n");
    return 0;
}
