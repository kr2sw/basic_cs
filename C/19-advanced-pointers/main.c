#include <stdio.h>
#include <stdlib.h>

// --- 이중 포인터 ---
void allocateArray(int** arr, int n) {
    *arr = (int*)malloc(n * sizeof(int));
    for (int i = 0; i < n; i++) {
        (*arr)[i] = (i + 1) * 10;
    }
}

// --- 함수 포인터 ---
int add(int a, int b) { return a + b; }
int subtract(int a, int b) { return a - b; }
int multiply(int a, int b) { return a * b; }
int divide(int a, int b) { return b ? a / b : 0; }

// 함수 포인터를 파라미터로 받는 함수
int calculate(int a, int b, int (*operation)(int, int)) {
    return operation(a, b);
}

// 함수 포인터 배열
int (*operations[])(int, int) = {add, subtract, multiply, divide};
char* opNames[] = {"add", "subtract", "multiply", "divide"};

// --- void 포인터 ---
void printValue(void* ptr, char type) {
    switch (type) {
        case 'i': printf("%d", *(int*)ptr); break;
        case 'f': printf("%.2f", *(float*)ptr); break;
        case 'c': printf("%c", *(char*)ptr); break;
        case 's': printf("%s", (char*)ptr); break;
        default: printf("알 수 없는 타입");
    }
}

// void 포인터 qsort 비교 함수
int compareInt(const void* a, const void* b) {
    return (*(int*)a - *(int*)b);
}

// --- const 포인터 ---
void demonstrateConst() {
    int x = 10, y = 20;

    const int* p1 = &x;   // 값 수정 불가, 주소 변경 가능
    // *p1 = 30;  // 컴파일 에러
    p1 = &y;              // OK

    int* const p2 = &x;   // 주소 변경 불가, 값 수정 가능
    *p2 = 30;             // OK
    // p2 = &y;  // 컴파일 에러

    const int* const p3 = &x;  // 둘 다 불가
}

int main() {
    printf("=== 이중 포인터 ===\n");
    int* dynamicArr = NULL;
    allocateArray(&dynamicArr, 5);

    printf("동적 할당 배열: ");
    for (int i = 0; i < 5; i++) {
        printf("%d ", dynamicArr[i]);
    }
    printf("\n");
    free(dynamicArr);

    // 이중 포인터 기초
    int value = 42;
    int* ptr = &value;
    int** dptr = &ptr;

    printf("\nvalue=%d, *ptr=%d, **dptr=%d\n", value, *ptr, **dptr);
    printf("&value=%p, ptr=%p, *dptr=%p\n", &value, ptr, *dptr);

    printf("\n=== 함수 포인터 ===\n");
    printf("calculate(10, 5, add) = %d\n", calculate(10, 5, add));
    printf("calculate(10, 5, subtract) = %d\n", calculate(10, 5, subtract));
    printf("calculate(10, 5, multiply) = %d\n", calculate(10, 5, multiply));
    printf("calculate(10, 5, divide) = %d\n", calculate(10, 5, divide));

    // 함수 포인터 배열
    printf("\n함수 포인터 배열:\n");
    for (int i = 0; i < 4; i++) {
        printf("  %s(20, 5) = %d\n", opNames[i], operations[i](20, 5));
    }

    printf("\n=== void 포인터 ===\n");
    int vi = 42;
    float vf = 3.14f;
    char vc = 'A';
    char* vs = "Hello void pointer";

    printf("int: ");    printValue(&vi, 'i'); printf("\n");
    printf("float: ");  printValue(&vf, 'f'); printf("\n");
    printf("char: ");   printValue(&vc, 'c'); printf("\n");
    printf("string: "); printValue(vs, 's');  printf("\n");

    // void 포인터와 qsort
    printf("\n=== qsort (void 포인터) ===\n");
    int arr[] = {5, 3, 1, 4, 2};
    int n = sizeof(arr) / sizeof(arr[0]);
    qsort(arr, n, sizeof(int), compareInt);
    printf("qsort 정렬: ");
    for (int i = 0; i < n; i++) printf("%d ", arr[i]);
    printf("\n");

    printf("\n=== 포인터 배열 vs 배열 포인터 ===\n");
    // 포인터 배열: 배열의 각 요소가 포인터
    int* ptrArr[3] = {&vi, &vf, &vi};  // arr of 3 int pointers

    // 배열 포인터: 배열을 가리키는 포인터
    int(*arrPtr)[5] = &arr;  // pointer to array of 5 ints
    printf("배열 포인터: (*arrPtr)[2] = %d\n", (*arrPtr)[2]);

    return 0;
}
