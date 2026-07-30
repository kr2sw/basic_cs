#include <stdio.h>
#include <stdlib.h>
#include <time.h>

// 배열 출력
void printArray(int arr[], int n) {
    for (int i = 0; i < n; i++) {
        printf("%d ", arr[i]);
    }
    printf("\n");
}

// 배열 복사
void copyArray(int src[], int dest[], int n) {
    for (int i = 0; i < n; i++) dest[i] = src[i];
}

// --- 버블 정렬 ---
void bubbleSort(int arr[], int n) {
    for (int i = 0; i < n - 1; i++) {
        int swapped = 0;
        for (int j = 0; j < n - 1 - i; j++) {
            if (arr[j] > arr[j + 1]) {
                int temp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = temp;
                swapped = 1;
            }
        }
        if (!swapped) break;  // 최적화: 이미 정렬됨
    }
}

// --- 선택 정렬 ---
void selectionSort(int arr[], int n) {
    for (int i = 0; i < n - 1; i++) {
        int minIdx = i;
        for (int j = i + 1; j < n; j++) {
            if (arr[j] < arr[minIdx]) minIdx = j;
        }
        if (minIdx != i) {
            int temp = arr[i];
            arr[i] = arr[minIdx];
            arr[minIdx] = temp;
        }
    }
}

// --- 삽입 정렬 ---
void insertionSort(int arr[], int n) {
    for (int i = 1; i < n; i++) {
        int key = arr[i];
        int j = i - 1;
        while (j >= 0 && arr[j] > key) {
            arr[j + 1] = arr[j];
            j--;
        }
        arr[j + 1] = key;
    }
}

// --- 퀵 정렬 ---
int partition(int arr[], int low, int high) {
    int pivot = arr[high];
    int i = low - 1;

    for (int j = low; j < high; j++) {
        if (arr[j] <= pivot) {
            i++;
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
    int temp = arr[i + 1];
    arr[i + 1] = arr[high];
    arr[high] = temp;
    return i + 1;
}

void quickSortRec(int arr[], int low, int high) {
    if (low < high) {
        int pi = partition(arr, low, high);
        quickSortRec(arr, low, pi - 1);
        quickSortRec(arr, pi + 1, high);
    }
}

void quickSort(int arr[], int n) {
    quickSortRec(arr, 0, n - 1);
}

// --- 병합 정렬 ---
void merge(int arr[], int left, int mid, int right) {
    int n1 = mid - left + 1;
    int n2 = right - mid;

    int* L = (int*)malloc(n1 * sizeof(int));
    int* R = (int*)malloc(n2 * sizeof(int));

    for (int i = 0; i < n1; i++) L[i] = arr[left + i];
    for (int j = 0; j < n2; j++) R[j] = arr[mid + 1 + j];

    int i = 0, j = 0, k = left;
    while (i < n1 && j < n2) {
        if (L[i] <= R[j]) arr[k++] = L[i++];
        else arr[k++] = R[j++];
    }
    while (i < n1) arr[k++] = L[i++];
    while (j < n2) arr[k++] = R[j++];

    free(L);
    free(R);
}

void mergeSortRec(int arr[], int left, int right) {
    if (left < right) {
        int mid = left + (right - left) / 2;
        mergeSortRec(arr, left, mid);
        mergeSortRec(arr, mid + 1, right);
        merge(arr, left, mid, right);
    }
}

void mergeSort(int arr[], int n) {
    mergeSortRec(arr, 0, n - 1);
}

// 정렬 성능 측정
long measureTime(void (*sortFunc)(int[], int), int arr[], int n) {
    clock_t start = clock();
    sortFunc(arr, n);
    clock_t end = clock();
    return end - start;
}

int main() {
    int n = 10000;
    int* original = (int*)malloc(n * sizeof(int));
    int* temp = (int*)malloc(n * sizeof(int));

    // 랜덤 배열 생성
    srand(time(NULL));
    for (int i = 0; i < n; i++) {
        original[i] = rand() % 10000;
    }

    printf("=== 정렬 알고리즘 성능 비교 ===\n");
    printf("데이터 크기: %d\n\n", n);

    // 작은 배열로 정확성 검증
    int small[] = {64, 34, 25, 12, 22, 11, 90};
    int smallN = 7;

    printf("정렬 전: ");
    printArray(small, smallN);

    copyArray(small, temp, smallN);
    bubbleSort(temp, smallN);
    printf("버블 정렬: "); printArray(temp, smallN);

    copyArray(small, temp, smallN);
    selectionSort(temp, smallN);
    printf("선택 정렬: "); printArray(temp, smallN);

    copyArray(small, temp, smallN);
    insertionSort(temp, smallN);
    printf("삽입 정렬: "); printArray(temp, smallN);

    copyArray(small, temp, smallN);
    quickSort(temp, smallN);
    printf("퀵 정렬:   "); printArray(temp, smallN);

    copyArray(small, temp, smallN);
    mergeSort(temp, smallN);
    printf("병합 정렬: "); printArray(temp, smallN);

    // 성능 측정
    printf("\n=== 성능 측정 (n=%d) ===\n", n);

    copyArray(original, temp, n);
    long t1 = measureTime(bubbleSort, temp, n);
    printf("버블 정렬: %ld ms\n", t1 * 1000 / CLOCKS_PER_SEC);

    copyArray(original, temp, n);
    long t2 = measureTime(selectionSort, temp, n);
    printf("선택 정렬: %ld ms\n", t2 * 1000 / CLOCKS_PER_SEC);

    copyArray(original, temp, n);
    long t3 = measureTime(insertionSort, temp, n);
    printf("삽입 정렬: %ld ms\n", t3 * 1000 / CLOCKS_PER_SEC);

    copyArray(original, temp, n);
    long t4 = measureTime(quickSort, temp, n);
    printf("퀵 정렬:   %ld ms\n", t4 * 1000 / CLOCKS_PER_SEC);

    copyArray(original, temp, n);
    long t5 = measureTime(mergeSort, temp, n);
    printf("병합 정렬: %ld ms\n", t5 * 1000 / CLOCKS_PER_SEC);

    free(original);
    free(temp);

    return 0;
}
