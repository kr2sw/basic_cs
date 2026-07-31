#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/*
 * [POSIX 전용 예제 - 주석으로만 제공합니다]
 * gcc로 컴파일할 때 -lpthread 링크가 필요합니다.
 * Windows에서는 CreateThread / _beginthreadex를 사용합니다.
 *
 * #include <pthread.h>
 *
 * static int balance = 0;
 * static pthread_mutex_t mtx = PTHREAD_MUTEX_INITIALIZER;
 *
 * void* deposit(void* arg) {
 *     int amount = *(int*)arg;
 *     pthread_mutex_lock(&mtx);     // 임계 구역 보호
 *     balance += amount;
 *     pthread_mutex_unlock(&mtx);
 *     return NULL;
 * }
 *
 * pthread_t t1, t2;
 * pthread_create(&t1, NULL, deposit, &a);
 * pthread_create(&t2, NULL, deposit, &b);
 * pthread_join(t1, NULL);
 * pthread_join(t2, NULL);
 */

// --- 1. 경쟁 상태(Race Condition) 개념 시뮬레이션 ---
// 실제 스레드 없이, 인터리빙(교차 실행)이 틀린 값을 만드는 과정을 흉내
static int balance = 0;

void demoRaceCondition(void) {
    printf("--- 경쟁 상태 시뮬레이션 ---\n");
    printf("스레드 2개가 balance를 각각 100씩 증가시키면 기대값: 200\n");

    // 각 "스레드"는 읽고/쓰기를 분리해 진행 (교차 시 값 손실 발생)
    int b1 = balance;
    int b2 = balance;      // 두 스레드가 동시에 같은 값 읽음 (개념)
    int result1 = 0, result2 = 0;
    for (int i = 0; i < 100; i++) result1 = ++b1;
    for (int i = 0; i < 100; i++) result2 = ++b2;
    balance = result1;     // 늦게 쓰는 쪽이 이전 값을 덮어씀
    balance = result2;

    printf("실제 결과(교차): %d  ← 손실 발생 (race condition)\n", balance);
    printf("  → 뮤텍스로 임계 구역을 보호하면 200이 보장됩니다.\n");
    balance = 0;
}

// --- 2. 뮤텍스 패턴 시뮬레이션 (잠금 플래그) ---
typedef struct {
    int locked;
} MutexSim;

void lockSim(MutexSim* m) {
    while (m->locked);    // 개념적으로는 busy-wait (실제는 OS가 블록)
    m->locked = 1;
}

void unlockSim(MutexSim* m) {
    m->locked = 0;
}

void demoMutexSim(void) {
    MutexSim mtx = {0};
    lockSim(&mtx);
    balance += 100;        // 임계 구역: 공유 자원 수정
    unlockSim(&mtx);
    printf("뮤텍스로 보호한 결과: balance = %d\n", balance);
}

// --- 3. 생산자-소비자 패턴 (원형 버퍼) ---
#define BUFFER_SIZE 5

typedef struct {
    int buf[BUFFER_SIZE];
    int head, tail, count;
} RingBuffer;

void rbPut(RingBuffer* r, int v) {
    if (r->count == BUFFER_SIZE) { printf("버퍼 가득 (소비 대기)\n"); return; }
    r->buf[r->tail] = v;
    r->tail = (r->tail + 1) % BUFFER_SIZE;
    r->count++;
}

int rbGet(RingBuffer* r, int* out) {
    if (r->count == 0) return 0;       // 버퍼 비어 있음
    *out = r->buf[r->head];
    r->head = (r->head + 1) % BUFFER_SIZE;
    r->count--;
    return 1;
}

void demoProducerConsumer(void) {
    printf("\n--- 생산자-소비자 (원형 버퍼) ---\n");
    RingBuffer rb = {{0}, 0, 0, 0};

    printf("생산: 1,2,3,4,5\n");
    for (int i = 1; i <= 5; i++) rbPut(&rb, i * 10);

    printf("생산 1개 추가 시도: ");
    rbPut(&rb, 99);   // 가득 참

    printf("소비: ");
    int v;
    while (rbGet(&rb, &v)) printf("%d ", v);
    printf("\n※ 실제 구현에서는 동기화 없이 동시 접근하면 데이터가 깨집니다.\n");
}

int main() {
    printf("=== 1. 경쟁 상태 ===\n");
    demoRaceCondition();

    printf("\n=== 2. 뮤텍스 시뮬레이션 ===\n");
    demoMutexSim();

    demoProducerConsumer();

    printf("\n※ 실제 스레드 생성/동기화는 pthread(POSIX) 또는 Windows API를 사용합니다.\n");
    printf("  main.c 상단 주석의 pthread 예제를 참고하세요.\n");
    return 0;
}
