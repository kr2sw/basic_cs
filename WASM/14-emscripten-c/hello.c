#include <stdio.h>
#include <emscripten/emscripten.h>

// JS에서 호출 가능한 함수
EMSCRIPTEN_KEEPALIVE
int add(int a, int b) {
    return a + b;
}

EMSCRIPTEN_KEEPALIVE
int factorial(int n) {
    if (n <= 1) return 1;
    return n * factorial(n - 1);
}

EMSCRIPTEN_KEEPALIVE
int fibonacci(int n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

int main() {
    printf("Hello from C compiled to WebAssembly!\n");
    printf("add(10, 20) = %d\n", add(10, 20));
    printf("factorial(10) = %d\n", factorial(10));
    printf("fibonacci(10) = %d\n", fibonacci(10));
    return 0;
}
