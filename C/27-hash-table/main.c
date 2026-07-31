#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define TABLE_SIZE 16

// --- 해시 함수 (djb2) ---
unsigned long hashDjb2(const char* str) {
    unsigned long h = 5381;
    int c;
    while ((c = (unsigned char)*str++) != '\0') {
        h = h * 33 + (unsigned)c;
    }
    return h;
}

// --- 1. 체이닝 (Chaining) 구현 ---
typedef struct KVPair {
    char key[32];
    int value;
    struct KVPair* next;
} KVPair;

typedef struct {
    KVPair* head;
} ChainTable;

ChainTable chain[TABLE_SIZE];

void chainPut(const char* key, int value) {
    KVPair* p = chain[hashDjb2(key) % TABLE_SIZE].head;
    while (p) {                       // 이미 있으면 갱신
        if (strcmp(p->key, key) == 0) { p->value = value; return; }
        p = p->next;
    }
    KVPair* n = (KVPair*)malloc(sizeof(KVPair));
    strcpy(n->key, key);
    n->value = value;
    n->next = chain[hashDjb2(key) % TABLE_SIZE].head;
    chain[hashDjb2(key) % TABLE_SIZE].head = n;
}

int chainGet(const char* key, int* out) {
    for (KVPair* p = chain[hashDjb2(key) % TABLE_SIZE].head; p; p = p->next) {
        if (strcmp(p->key, key) == 0) { *out = p->value; return 1; }
    }
    return 0;
}

void chainPrint(void) {
    for (int i = 0; i < TABLE_SIZE; i++) {
        if (chain[i].head) {
            printf("[%2d] ", i);
            for (KVPair* p = chain[i].head; p; p = p->next) {
                printf("%s=%d -> ", p->key, p->value);
            }
            printf("NULL\n");
        }
    }
}

// --- 2. 오픈 어드레싱 (선형 탐사) ---
typedef struct {
    char key[32];
    int value;
    int occupied;   // 1: 사용 중, 0: 빈 슬롯, -1: 톰스톤(삭제됨)
} OpenSlot;

OpenSlot openTable[TABLE_SIZE];

int openPut(const char* key, int value) {
    unsigned long h = hashDjb2(key);
    for (int i = 0; i < TABLE_SIZE; i++) {
        int idx = (int)((h + i) % TABLE_SIZE);     // 선형 탐사
        if (openTable[idx].occupied == 0 || openTable[idx].occupied == -1) {
            strcpy(openTable[idx].key, key);
            openTable[idx].value = value;
            openTable[idx].occupied = 1;
            return 1;
        }
        if (openTable[idx].occupied == 1 && strcmp(openTable[idx].key, key) == 0) {
            openTable[idx].value = value;          // 갱신
            return 1;
        }
    }
    return 0;   // 테이블 가득 참
}

int openGet(const char* key, int* out) {
    unsigned long h = hashDjb2(key);
    for (int i = 0; i < TABLE_SIZE; i++) {
        int idx = (int)((h + i) % TABLE_SIZE);
        if (openTable[idx].occupied == 0) return 0;         // 빈 슬롯 = 없음
        if (openTable[idx].occupied == 1 && strcmp(openTable[idx].key, key) == 0) {
            *out = openTable[idx].value;
            return 1;
        }
    }
    return 0;
}

int openDelete(const char* key) {
    unsigned long h = hashDjb2(key);
    for (int i = 0; i < TABLE_SIZE; i++) {
        int idx = (int)((h + i) % TABLE_SIZE);
        if (openTable[idx].occupied == 0) return 0;
        if (openTable[idx].occupied == 1 && strcmp(openTable[idx].key, key) == 0) {
            openTable[idx].occupied = -1;   // 톰스톤 표시
            return 1;
        }
    }
    return 0;
}

void openPrint(void) {
    for (int i = 0; i < TABLE_SIZE; i++) {
        if (openTable[i].occupied == 1) {
            printf("[%2d] %s=%d\n", i, openTable[i].key, openTable[i].value);
        } else if (openTable[i].occupied == -1) {
            printf("[%2d] (삭제됨)\n", i);
        }
    }
}

int main() {
    printf("=== 1. 체이닝 방식 ===\n");
    chainPut("apple", 5);
    chainPut("banana", 3);
    chainPut("grape", 8);
    chainPut("orange", 2);
    chainPut("banana", 99);   // 값 갱신
    chainPrint();

    int v;
    printf("apple -> %s\n", chainGet("apple", &v) ? "있음" : "없음");
    printf("banana = %d\n", v);
    printf("melon -> %s\n", chainGet("melon", &v) ? "있음" : "없음");

    printf("\n=== 2. 오픈 어드레싱 (선형 탐사) ===\n");
    openPut("apple", 5);
    openPut("banana", 3);
    openPut("grape", 8);
    openPut("orange", 2);
    openPrint();

    openDelete("banana");
    printf("\nbanana 삭제 후:\n");
    openPrint();
    printf("\nbanana 조회: %s\n", openGet("banana", &v) ? "있음" : "없음 (톰스톤 뒤에도 탐색 계속)");

    printf("\n※ 부하율(load factor)이 높아지면 두 방식 모두 성능이 저하됩니다.\n");
    return 0;
}
