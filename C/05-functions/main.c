#include <stdio.h>

// 함수 프로토타입
int add(int a, int b);
double divide(double a, double b);
void printInfo(char* name, int age);
int factorial(int n);
void swap(int* a, int* b);
int sumArray(int arr[], int n);

int main() {
    // 기본 함수
    printf("add(3, 5) = %d\n", add(3, 5));
    printf("divide(10, 3) = %lf\n", divide(10.0, 3.0));

    // void 반환
    printInfo("Alice", 25);

    // 재귀 함수
    printf("factorial(5) = %d\n", factorial(5));

    // Call by reference (포인터)
    int x = 10, y = 20;
    printf("\n교환 전: x=%d, y=%d\n", x, y);
    swap(&x, &y);
    printf("교환 후: x=%d, y=%d\n", x, y);

    // 배열을 파라미터로
    int arr[] = {1, 2, 3, 4, 5};
    int n = sizeof(arr) / sizeof(arr[0]);
    printf("배열 합계: %d\n", sumArray(arr, n));

    // static 변수
    for (int i = 0; i < 3; i++) {
        printf("counter() = %d\n", counter());
    }

    // 인라인 함수 (성능 최적화 힌트)
    printf("square(7) = %d\n", square(7));

    return 0;
}

int add(int a, int b) {
    return a + b;
}

double divide(double a, double b) {
    if (b == 0) return 0;
    return a / b;
}

void printInfo(char* name, int age) {
    printf("이름: %s, 나이: %d\n", name, age);
}

int factorial(int n) {
    if (n <= 1) return 1;
    return n * factorial(n - 1);
}

void swap(int* a, int* b) {
    int temp = *a;
    *a = *b;
    *b = temp;
}

int sumArray(int arr[], int n) {
    int sum = 0;
    for (int i = 0; i < n; i++) {
        sum += arr[i];
    }
    return sum;
}

// static 변수 (호출 간 값 유지)
int counter() {
    static int count = 0;  // 한 번만 초기화
    return ++count;
}

// 인라인 함수 (컴파일러 최적화 힌트)
static inline int square(int x) {
    return x * x;
}
