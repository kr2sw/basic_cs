#include <stdio.h>
#include "math_utils.h"

// extern 변수 (다른 파일에서 정의)
extern int globalCounter;

// static 함수 (이 파일에서만 접근 가능)
static void printSeparator() {
    printf("----------------------\n");
}

int main() {
    printf("=== 다중 파일 프로그래밍 ===\n\n");

    printSeparator();
    printf("기본 연산:\n");
    printf("add(10, 5) = %d\n", add(10, 5));
    printf("subtract(10, 5) = %d\n", subtract(10, 5));
    printf("multiply(10, 5) = %d\n", multiply(10, 5));
    printf("divide(10, 3) = %.2f\n", divide(10.0, 3.0));

    printSeparator();
    printf("factorial(6) = %d\n", factorial(6));
    printf("gcd(48, 18) = %d\n", gcd(48, 18));

    printSeparator();
    printf("컴파일 방법:\n");
    printf("  gcc -c math_utils.c -o math_utils.o\n");
    printf("  gcc -c main.c -o main.o\n");
    printf("  gcc math_utils.o main.o -o program\n");
    printf("  또는: gcc math_utils.c main.c -o program\n");

    printSeparator();
    printf("컴파일 및 실행:\n");
    printf("  gcc -o program main.c math_utils.c\n");
    printf("  .\\program.exe\n");

    return 0;
}
