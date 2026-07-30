#include <stdio.h>

// 팩토리얼
int factorial(int n) {
    if (n <= 1) return 1;        // 종료 조건
    return n * factorial(n - 1); // 재귀 호출
}

// 피보나치
int fibonacci(int n) {
    if (n <= 1) return n;        // F(0)=0, F(1)=1
    return fibonacci(n - 1) + fibonacci(n - 2);
}

// 거듭제곱
int power(int base, int exp) {
    if (exp == 0) return 1;
    if (exp % 2 == 0) {
        int half = power(base, exp / 2);
        return half * half;
    }
    return base * power(base, exp - 1);
}

// 최대공약수 (유클리드 호제법)
int gcd(int a, int b) {
    if (b == 0) return a;
    return gcd(b, a % b);
}

// 하노이 탑
void hanoi(int n, char from, char to, char aux) {
    if (n == 1) {
        printf("  원판 1: %c -> %c\n", from, to);
        return;
    }
    hanoi(n - 1, from, aux, to);
    printf("  원판 %d: %c -> %c\n", n, from, to);
    hanoi(n - 1, aux, to, from);
}

// 배열 합계 (재귀)
int sumArray(int arr[], int n) {
    if (n <= 0) return 0;
    return arr[n - 1] + sumArray(arr, n - 1);
}

// 이진 탐색 (재귀)
int binarySearch(int arr[], int left, int right, int target) {
    if (left > right) return -1;

    int mid = left + (right - left) / 2;

    if (arr[mid] == target) return mid;
    if (target < arr[mid])
        return binarySearch(arr, left, mid - 1, target);
    return binarySearch(arr, mid + 1, right, target);
}

// 문자열 길이 (재귀)
int strLength(char* s) {
    if (*s == '\0') return 0;
    return 1 + strLength(s + 1);
}

int main() {
    printf("=== 재귀 함수 예제 ===\n\n");

    printf("factorial(5) = %d\n", factorial(5));
    printf("factorial(10) = %d\n", factorial(10));

    printf("\nfibonacci(10) = %d\n", fibonacci(10));
    printf("fibonacci sequence: ");
    for (int i = 0; i <= 10; i++) {
        printf("%d ", fibonacci(i));
    }
    printf("\n");

    printf("\npower(2, 10) = %d\n", power(2, 10));
    printf("power(3, 4) = %d\n", power(3, 4));

    printf("\ngcd(48, 18) = %d\n", gcd(48, 18));
    printf("gcd(1071, 462) = %d\n", gcd(1071, 462));

    printf("\n하노이 탑 (n=3):\n");
    hanoi(3, 'A', 'C', 'B');

    int arr[] = {1, 2, 3, 4, 5};
    printf("\nsumArray = %d\n", sumArray(arr, 5));

    int sorted[] = {2, 5, 8, 12, 16, 23, 38, 56, 72, 91};
    int n = sizeof(sorted) / sizeof(sorted[0]);
    int target = 23;
    int idx = binarySearch(sorted, 0, n - 1, target);
    printf("\nbinarySearch(%d) = %d\n", target, idx);

    printf("\nstrLength(\"Hello\") = %d\n", strLength("Hello"));
    printf("strLength(\"Recursion\") = %d\n", strLength("Recursion"));

    // 스택 오버플로우 주의
    printf("\n※ 주의: 큰 입력은 스택 오버플로우 위험\n");

    return 0;
}
