#include <stdio.h>
#include <stdlib.h>
#include <time.h>

// 선형 탐색
int linearSearch(int arr[], int n, int target) {
    for (int i = 0; i < n; i++) {
        if (arr[i] == target) return i;
    }
    return -1;
}

// 이진 탐색 (반복문)
int binarySearchIter(int arr[], int n, int target) {
    int left = 0, right = n - 1;

    while (left <= right) {
        int mid = left + (right - left) / 2;

        if (arr[mid] == target) return mid;
        if (arr[mid] < target) left = mid + 1;
        else right = mid - 1;
    }
    return -1;
}

// 이진 탐색 (재귀)
int binarySearchRec(int arr[], int left, int right, int target) {
    if (left > right) return -1;

    int mid = left + (right - left) / 2;

    if (arr[mid] == target) return mid;
    if (arr[mid] < target)
        return binarySearchRec(arr, mid + 1, right, target);
    return binarySearchRec(arr, left, mid - 1, target);
}

// 정렬 확인
int isSorted(int arr[], int n) {
    for (int i = 1; i < n; i++) {
        if (arr[i] < arr[i - 1]) return 0;
    }
    return 1;
}

// qsort 비교 함수
int cmp(const void* a, const void* b) {
    return (*(int*)a - *(int*)b);
}

int main() {
    int n = 20;
    int arr[20];

    srand(time(NULL));
    printf("=== 탐색 알고리즘 ===\n\n");

    // 선형 탐색 (정렬 불필요)
    printf("=== 선형 탐색 ===\n");
    for (int i = 0; i < n; i++) {
        arr[i] = rand() % 100;
    }
    printf("배열: ");
    for (int i = 0; i < n; i++) printf("%d ", arr[i]);
    printf("\n");

    int target = arr[5];  // 무조건 있는 값
    int idx = linearSearch(arr, n, target);
    printf("linearSearch(%d) = %d\n", target, idx);

    idx = linearSearch(arr, n, 999);
    printf("linearSearch(999) = %d (없음)\n", idx);

    // 이진 탐색 (정렬 필요)
    printf("\n=== 이진 탐색 ===\n");

    // 정렬
    qsort(arr, n, sizeof(int), cmp);
    printf("정렬: ");
    for (int i = 0; i < n; i++) printf("%d ", arr[i]);
    printf("\n");

    target = arr[n / 2];
    idx = binarySearchIter(arr, n, target);
    printf("binarySearchIter(%d) = %d\n", target, idx);

    idx = binarySearchRec(arr, 0, n - 1, target);
    printf("binarySearchRec(%d) = %d\n", target, idx);

    idx = binarySearchIter(arr, n, 999);
    printf("binarySearchIter(999) = %d (없음)\n", idx);

    // 경계값 테스트
    printf("\n=== 경계값 테스트 ===\n");
    int sorted[] = {2, 5, 8, 12, 16, 23, 38, 56, 72, 91};
    int m = sizeof(sorted) / sizeof(sorted[0]);

    int tests[] = {sorted[0], sorted[m-1], 1, 100, 23};
    int numTests = sizeof(tests) / sizeof(tests[0]);

    for (int i = 0; i < numTests; i++) {
        idx = binarySearchIter(sorted, m, tests[i]);
        printf("binarySearch(%d) = %d\n", tests[i], idx);
    }

    // 성능 비교
    printf("\n=== 성능 비교 ===\n");
    int largeN = 100000;
    int* largeArr = (int*)malloc(largeN * sizeof(int));
    for (int i = 0; i < largeN; i++) largeArr[i] = i;

    clock_t start = clock();
    for (int i = 0; i < 10000; i++) {
        linearSearch(largeArr, largeN, largeN - 1);
    }
    clock_t end = clock();
    printf("선형 탐색 x10000: %ld ms\n",
           (end - start) * 1000 / CLOCKS_PER_SEC);

    start = clock();
    for (int i = 0; i < 10000; i++) {
        binarySearchIter(largeArr, largeN, largeN - 1);
    }
    end = clock();
    printf("이진 탐색 x10000: %ld ms\n",
           (end - start) * 1000 / CLOCKS_PER_SEC);

    free(largeArr);
    return 0;
}
