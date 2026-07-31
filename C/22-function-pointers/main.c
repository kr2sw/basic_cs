#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// --- 1. 콜백 (Callback) ---
// 배열 각 원소에 콜백 함수를 적용
void forEach(int arr[], int n, void (*fn)(int)) {
    for (int i = 0; i < n; i++) {
        fn(arr[i]);
    }
}

void printSquare(int x) {
    printf("%d² = %d\n", x, x * x);
}

// --- 2. qsort ---
int cmpInt(const void* a, const void* b) {
    int x = *(const int*)a;
    int y = *(const int*)b;
    return (x > y) - (x < y);   // 오름차순
}

int cmpStr(const void* a, const void* b) {
    // const void* → char**로 캐스팅한 뒤 역참조
    const char* sa = *(const char* const*)a;
    const char* sb = *(const char* const*)b;
    return strcmp(sa, sb);
}

// --- 3. 커맨드 테이블 (Command Table) ---
void cmdHelp(void) {
    printf("사용 가능한 명령: help, add, quit\n");
}

void cmdAdd(void) {
    printf("add 명령 실행: 두 수를 더합니다.\n");
}

void cmdQuit(void) {
    printf("quit 명령 실행: 종료합니다.\n");
}

typedef struct {
    const char* name;
    void (*fn)(void);
} Command;

const Command commandTable[] = {
    {"help", cmdHelp},
    {"add",  cmdAdd},
    {"quit", cmdQuit}
};

void runCommand(const char* name) {
    for (int i = 0; i < 3; i++) {
        if (strcmp(name, commandTable[i].name) == 0) {
            commandTable[i].fn();
            return;
        }
    }
    printf("알 수 없는 명령: %s\n", name);
}

// --- 4. 함수를 반환하는 함수 (팩토리) ---
typedef int (*BinOp)(int, int);

int add(int a, int b) { return a + b; }
int sub(int a, int b) { return a - b; }
int mul(int a, int b) { return a * b; }
int divide(int a, int b) { return b ? a / b : 0; }

BinOp getOperation(char op) {
    switch (op) {
        case '+': return add;
        case '-': return sub;
        case '*': return mul;
        case '/': return divide;
        default:  return NULL;
    }
}

int main() {
    printf("=== 1. 콜백 ===\n");
    int arr[] = {1, 2, 3, 4};
    forEach(arr, 4, printSquare);

    printf("\n=== 2. qsort ===\n");
    int nums[] = {42, 7, 19, 3, 100, -5};
    qsort(nums, 6, sizeof(int), cmpInt);
    printf("정렬된 정수: ");
    for (int i = 0; i < 6; i++) printf("%d ", nums[i]);
    printf("\n");

    const char* words[] = {"banana", "apple", "cherry", "date"};
    qsort(words, 4, sizeof(char*), cmpStr);
    printf("정렬된 문자열: ");
    for (int i = 0; i < 4; i++) printf("%s ", words[i]);
    printf("\n");

    printf("\n=== 3. 커맨드 테이블 ===\n");
    runCommand("help");
    runCommand("add");
    runCommand("quit");
    runCommand("unknown");

    printf("\n=== 4. 함수 반환 (팩토리) ===\n");
    char ops[] = {'+', '*', '/', '?'};
    for (int i = 0; i < 4; i++) {
        BinOp op = getOperation(ops[i]);
        if (op) {
            printf("8 %c 3 = %d\n", ops[i], op(8, 3));
        } else {
            printf("'%c': 지원하지 않는 연산자\n", ops[i]);
        }
    }

    return 0;
}
