#include <stdio.h>

int main() {
    int score = 85;

    // if-else if-else
    if (score >= 90) {
        printf("A\n");
    } else if (score >= 80) {
        printf("B\n");
    } else if (score >= 70) {
        printf("C\n");
    } else {
        printf("F\n");
    }

    // switch
    int day = 3;
    switch (day) {
        case 1: printf("월\n"); break;
        case 2: printf("화\n"); break;
        case 3: printf("수\n"); break;
        case 4: printf("목\n"); break;
        case 5: printf("금\n"); break;
        default: printf("주말\n");
    }

    // 삼항 연산자
    int age = 20;
    char* status = (age >= 18) ? "성인" : "미성년자";
    printf("%s\n", status);

    // for문
    printf("for: ");
    for (int i = 1; i <= 5; i++) {
        printf("%d ", i);
    }
    printf("\n");

    // while문
    printf("while: ");
    int j = 1;
    while (j <= 5) {
        printf("%d ", j);
        j++;
    }
    printf("\n");

    // do-while문
    printf("do-while: ");
    int k = 1;
    do {
        printf("%d ", k);
        k++;
    } while (k <= 5);
    printf("\n");

    // 중첩 반복문 (구구단)
    printf("\n구구단:\n");
    for (int i = 2; i <= 9; i++) {
        for (int j = 1; j <= 9; j++) {
            printf("%d x %d = %d\n", i, j, i * j);
        }
        printf("\n");
    }

    // break / continue
    printf("break/continue: ");
    for (int i = 1; i <= 10; i++) {
        if (i % 2 == 0) continue;   // 짝수 건너뛰기
        if (i > 7) break;           // 7 초과면 종료
        printf("%d ", i);           // 1 3 5 7
    }
    printf("\n");

    // goto (비권장, 하지만 가끔 유용)
    int error = 0;
    if (error) {
        goto cleanup;
    }
    printf("정상 실행\n");

cleanup:
    printf("정리 작업\n");

    return 0;
}
