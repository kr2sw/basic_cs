#include <stdio.h>

int main() {
    // 기본 포인터
    int x = 42;
    int* ptr = &x;  // x의 주소 저장

    printf("x = %d\n", x);
    printf("&x = %p\n", &x);
    printf("ptr = %p\n", ptr);
    printf("*ptr = %d\n", *ptr);  // 역참조

    // 포인터로 값 변경
    *ptr = 100;
    printf("x after *ptr = 100: %d\n", x);

    // 포인터 크기
    printf("\n포인터 크기: %zu bytes\n", sizeof(ptr));

    // NULL 포인터
    int* nullPtr = NULL;
    if (nullPtr == NULL) {
        printf("NULL 포인터입니다.\n");
    }

    // 포인터와 배열
    int arr[] = {10, 20, 30, 40, 50};
    int* arrPtr = arr;  // == &arr[0]

    printf("\n=== 포인터와 배열 ===\n");
    printf("arr[0] = %d, *arrPtr = %d\n", arr[0], *arrPtr);
    printf("arr[2] = %d, *(arrPtr+2) = %d\n", arr[2], *(arrPtr + 2));

    // 포인터 연산
    printf("\n=== 포인터 연산 ===\n");
    for (int i = 0; i < 5; i++) {
        printf("arrPtr+%d = %p, *(arrPtr+%d) = %d\n",
               i, arrPtr + i, i, *(arrPtr + i));
    }

    // 포인터로 배열 순회
    printf("\n포인터로 순회: ");
    for (int* p = arr; p < arr + 5; p++) {
        printf("%d ", *p);
    }
    printf("\n");

    // Call by reference
    int a = 10, b = 20;
    printf("\n교환 전: a=%d, b=%d\n", a, b);
    swap(&a, &b);
    printf("교환 후: a=%d, b=%d\n", a, b);

    // 포인터 배열
    int* ptrs[3] = {&a, &b, &x};
    printf("\n포인터 배열: ");
    for (int i = 0; i < 3; i++) {
        printf("%d ", *ptrs[i]);
    }
    printf("\n");

    return 0;
}

void swap(int* a, int* b) {
    int temp = *a;
    *a = *b;
    *b = temp;
}
