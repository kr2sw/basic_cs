#include <stdio.h>
#include <string.h>

#define MAX_LISTENERS 4
#define POOL_SIZE 3

// --- 1. 상태 머신: 신호등 ---
typedef enum { LIGHT_GREEN, LIGHT_YELLOW, LIGHT_RED } LightState;

const char* lightName(LightState s) {
    switch (s) {
        case LIGHT_GREEN:  return "초록";
        case LIGHT_YELLOW: return "노랑";
        case LIGHT_RED:    return "빨강";
    }
    return "?";
}

// 상태 전이 함수 (switch 기반 상태 머신)
LightState lightTick(LightState s) {
    switch (s) {
        case LIGHT_GREEN:  return LIGHT_YELLOW;
        case LIGHT_YELLOW: return LIGHT_RED;
        case LIGHT_RED:    return LIGHT_GREEN;
    }
    return s;
}

void demoStateMachine(void) {
    printf("=== 1. 상태 머신 (신호등) ===\n");
    LightState s = LIGHT_GREEN;
    for (int i = 0; i < 7; i++) {
        printf("  t+%d초: %s\n", i, lightName(s));
        s = lightTick(s);
    }
}

// --- 2. 옵저버: 이벤트 리스너 ---
typedef struct {
    const char* name;
    void (*onEvent)(const char* source, const char* msg);
} Listener;

typedef struct {
    Listener* listeners[MAX_LISTENERS];
    int count;
} EventBus;

void busSubscribe(EventBus* b, Listener* l) {
    if (b->count < MAX_LISTENERS) b->listeners[b->count++] = l;
}

void busNotify(EventBus* b, const char* source, const char* msg) {
    for (int i = 0; i < b->count; i++) {
        b->listeners[i]->onEvent(source, msg);
    }
}

void logToConsole(const char* src, const char* msg) {
    printf("  [콘솔 리스너] %s: %s\n", src, msg);
}

void logToFile(const char* src, const char* msg) {
    printf("  [파일 리스너] %s: %s (파일에 기록)\n", src, msg);
}

void demoObserver(void) {
    printf("\n=== 2. 옵저버 (이벤트 버스) ===\n");
    static Listener console = {"console", logToConsole};
    static Listener fileLog = {"file", logToFile};

    EventBus bus = {{0}, 0};
    busSubscribe(&bus, &console);
    busSubscribe(&bus, &fileLog);

    busNotify(&bus, "센서", "온도 25도 도달");
    busNotify(&bus, "시스템", "사용자 로그인");
}

// --- 3. 리소스 풀: DB 커넥션 ---
typedef struct {
    int connectionId;
    int used;
    char client[20];
} Connection;

typedef struct {
    Connection pool[POOL_SIZE];
} ConnectionPool;

int poolAvailable(ConnectionPool* p);

void poolInit(ConnectionPool* p) {
    for (int i = 0; i < POOL_SIZE; i++) {
        p->pool[i].connectionId = i;
        p->pool[i].used = 0;
        p->pool[i].client[0] = '\0';
    }
}

// 풀에서 커넥션 빌려주기
Connection* poolAcquire(ConnectionPool* p, const char* client) {
    for (int i = 0; i < POOL_SIZE; i++) {
        if (!p->pool[i].used) {
            p->pool[i].used = 1;
            strcpy(p->pool[i].client, client);
            printf("  [풀] %s에 커넥션#%d 할당 (남은 여유: %d)\n",
                   client, p->pool[i].connectionId,
                   poolAvailable(p));
            return &p->pool[i];
        }
    }
    printf("  [풀] %s 요청 거부: 커넥션 소진 (대기 또는 오류)\n", client);
    return NULL;
}

// 반납
void poolRelease(ConnectionPool* p, Connection* c) {
    if (!c || !c->used) return;
    printf("  [풀] %s가 커넥션#%d 반납\n", c->client, c->connectionId);
    c->used = 0;
    c->client[0] = '\0';
}

void demoResourcePool(void) {
    printf("\n=== 3. 리소스 풀 (DB 커넥션) ===\n");
    ConnectionPool pool;
    poolInit(&pool);

    Connection* c1 = poolAcquire(&pool, "앱-서버A");
    Connection* c2 = poolAcquire(&pool, "앱-서버B");
    Connection* c3 = poolAcquire(&pool, "앱-서버C");
    Connection* c4 = poolAcquire(&pool, "앱-서버D");   // 소진 → 거부

    poolRelease(&pool, c1);
    Connection* c5 = poolAcquire(&pool, "앱-서버D");   // 반납분 재사용
    poolRelease(&pool, c2);
    poolRelease(&pool, c3);
    poolRelease(&pool, c5);
}

int main(void) {
    demoStateMachine();
    demoObserver();
    demoResourcePool();

    printf("\n※ 세 패턴 모두 '구조체 + 함수 포인터'로 C에서 표현됩니다.\n");
    return 0;
}
