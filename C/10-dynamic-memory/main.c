#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main() {
    // --- malloc ---
    printf("=== malloc ===\n");
    int* arr = (int*)malloc(5 * sizeof(int));
    if (!arr) {
        fprintf(stderr, "메모리 할당 실패\n");
        return 1;
    }

    // 할당된 메모리는 초기화되지 않음
    for (int i = 0; i < 5; i++) {
        arr[i] = (i + 1) * 10;
    }
    printf("arr: ");
    for (int i = 0; i < 5; i++) {
        printf("%d ", arr[i]);
    }
    printf("\n");

    // --- calloc ---
    printf("\n=== calloc ===\n");
    int* zeroArr = (int*)calloc(5, sizeof(int));
    if (!zeroArr) {
        fprintf(stderr, "메모리 할당 실패\n");
        free(arr);
        return 1;
    }
    printf("zeroArr (모두 0): ");
    for (int i = 0; i < 5; i++) {
        printf("%d ", zeroArr[i]);
    }
    printf("\n");

    // --- realloc ---
    printf("\n=== realloc ===\n");
    int* bigger = (int*)realloc(arr, 10 * sizeof(int));
    if (!bigger) {
        fprintf(stderr, "재할당 실패\n");
        free(arr);
        free(zeroArr);
        return 1;
    }
    arr = bigger;  // 포인터 갱신
    for (int i = 5; i < 10; i++) {
        arr[i] = (i + 1) * 10;
    }
    printf("realloc 후: ");
    for (int i = 0; i < 10; i++) {
        printf("%d ", arr[i]);
    }
    printf("\n");

    // --- 문자열 동적 할당 ---
    printf("\n=== 문자열 동적 할당 ===\n");
    char* str = (char*)malloc(50 * sizeof(char));
    if (!str) {
        fprintf(stderr, "메모리 할당 실패\n");
        free(arr);
        free(zeroArr);
        return 1;
    }
    strcpy(str, "Dynamic memory allocation in C");
    printf("str: %s\n", str);

    // --- 2차원 배열 동적 할당 ---
    printf("\n=== 2차원 동적 배열 ===\n");
    int rows = 3, cols = 4;
    int** matrix = (int**)malloc(rows * sizeof(int*));
    for (int i = 0; i < rows; i++) {
        matrix[i] = (int*)malloc(cols * sizeof(int));
        for (int j = 0; j < cols; j++) {
            matrix[i][j] = i * cols + j + 1;
        }
    }

    printf("Matrix:\n");
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            printf("%2d ", matrix[i][j]);
        }
        printf("\n");
    }

    // --- 메모리 해제 ---
    printf("\n=== 메모리 해제 ===\n");
    free(arr);
    free(zeroArr);
    free(str);

    for (int i = 0; i < rows; i++) {
        free(matrix[i]);
    }
    free(matrix);

    // 포인터 NULL 설정 (댕글링 포인터 방지)
    arr = NULL;
    zeroArr = NULL;
    str = NULL;
    matrix = NULL;

    printf("모든 메모리 해제 완료\n");

    return 0;
}
