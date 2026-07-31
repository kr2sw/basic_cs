#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/*
 * [POSIX 전용 예제 - 주석으로만 제공합니다]
 * Windows에서는 fork 대신 CreateProcess를 사용합니다.
 *
 * #include <unistd.h>
 * #include <sys/wait.h>
 *
 * int main(void) {
 *     pid_t pid = fork();          // 프로세스 복제
 *     if (pid < 0) return 1;
 *     if (pid == 0) {              // 자식 프로세스
 *         execlp("echo", "echo", "자식 프로세스 실행", NULL);
 *         return 1;
 *     }
 *     int status;
 *     wait(&status);               // 부모는 자식 종료 대기
 *     return 0;
 * }
 */

// --- 1. system(): 표준 C로 명령 실행 ---
void demoSystem(void) {
    printf("system(NULL) = %d (셸 사용 가능 여부)\n", system(NULL));
    printf("명령 실행: echo hello\n");
    int rc = system("echo hello from system()");
    printf("명령 종료 코드: %d (0이면 성공)\n", rc);
}

// --- 2. 환경 변수 (표준 C: getenv) ---
void demoEnv(void) {
    const char* paths = getenv("PATH");
    printf("PATH 환경 변수:\n%s\n", paths ? paths : "(없음)");

    const char* lang = getenv("LANG");
    printf("LANG: %s\n", lang ? lang : "(설정 안 됨)");
}

// --- 3. 종료 코드 패턴 ---
int isEven(int n) {
    if (n % 2 == 0) return EXIT_SUCCESS;   // 0
    return EXIT_FAILURE;                    // 1
}

// --- 4. 프로세스 트리 개념 시뮬레이션 ---
// fork 없이 개념만: "부모가 자식을 만든다"는 흐름을 함수 호출로 표현
int childWork(const char* name, int n) {
    int sum = 0;
    for (int i = 1; i <= n; i++) sum += i;
    printf("  [자식 프로세스 %s] 1..%d 합 = %d (PID 흉내)\n", name, n, sum);
    return sum;
}

void simulateProcessTree(void) {
    printf("\n프로세스 트리 시뮬레이션 (실제 fork 아님):\n");
    printf("[부모] 자식 두 개 생성 대기...\n");
    int r1 = childWork("A", 10);
    int r2 = childWork("B", 20);
    printf("[부모] 자식 결과 수집: %d + %d = %d\n", r1, r2, r1 + r2);
}

int main() {
    printf("=== 1. system()으로 외부 명령 실행 ===\n");
    demoSystem();

    printf("\n=== 2. 환경 변수 읽기 ===\n");
    demoEnv();

    printf("\n=== 3. 종료 코드 (EXIT_SUCCESS/EXIT_FAILURE) ===\n");
    printf("isEven(4) = %d, isEven(7) = %d\n", isEven(4), isEven(7));

    printf("\n=== 4. 프로세스 트리 개념 ===\n");
    simulateProcessTree();

    printf("\n※ 실제 프로세스 생성(fork/exec, CreateProcess)은 플랫폼 API를 사용합니다.\n");
    printf("  POSIX 예제는 main.c 상단 주석을 참고하세요.\n");
    return 0;
}
