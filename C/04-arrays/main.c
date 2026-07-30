#include <stdio.h>

#define LENGTH(arr) (sizeof(arr) / sizeof(arr[0]))

int main() {
    // 1차원 배열
    int scores[] = {90, 85, 78, 92, 88};
    int n = LENGTH(scores);

    printf("배열 길이: %d\n", n);
    printf("첫 번째: %d\n", scores[0]);

    printf("모든 점수: ");
    for (int i = 0; i < n; i++) {
        printf("%d ", scores[i]);
    }
    printf("\n");

    // 총점 / 평균
    int sum = 0;
    for (int i = 0; i < n; i++) {
        sum += scores[i];
    }
    printf("총점: %d, 평균: %.2f\n", sum, (double)sum / n);

    // 배열 초기화
    int zeros[5] = {0};  // 모두 0
    printf("zeros: ");
    for (int i = 0; i < 5; i++) printf("%d ", zeros[i]);
    printf("\n");

    // 2차원 배열
    int matrix[3][3] = {
        {1, 2, 3},
        {4, 5, 6},
        {7, 8, 9}
    };

    printf("\n2차원 배열:\n");
    for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
            printf("%d ", matrix[i][j]);
        }
        printf("\n");
    }

    // 배열과 포인터 관계
    int arr[] = {10, 20, 30, 40, 50};
    printf("\narr = %p\n", arr);
    printf("&arr[0] = %p\n", &arr[0]);
    printf("*arr = %d\n", *arr);
    printf("*(arr+2) = %d\n", *(arr + 2));  // arr[2]와 동일

    // 배열 복사
    int source[] = {1, 2, 3, 4, 5};
    int dest[5];
    for (int i = 0; i < LENGTH(source); i++) {
        dest[i] = source[i];
    }

    printf("\n복사된 배열: ");
    for (int i = 0; i < LENGTH(dest); i++) {
        printf("%d ", dest[i]);
    }
    printf("\n");

    // 가변 길이 배열 (VLA, C99+)
    int size;
    printf("\n배열 크기 입력: ");
    scanf("%d", &size);

    int vla[size];
    for (int i = 0; i < size; i++) {
        vla[i] = i * i;
    }
    printf("VLA: ");
    for (int i = 0; i < size; i++) {
        printf("%d ", vla[i]);
    }
    printf("\n");

    return 0;
}
