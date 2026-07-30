#include <stdio.h>
#include <limits.h>
#include <float.h>

int main() {
    // 기본 자료형
    int i = 2147483647;
    short s = 32767;
    long l = 2147483647L;
    long long ll = 9223372036854775807LL;
    float f = 3.14f;
    double d = 3.1415926535;
    char c = 'A';
    unsigned int ui = 4294967295U;

    printf("int: %d\n", i);
    printf("short: %hd\n", s);
    printf("long: %ld\n", l);
    printf("long long: %lld\n", ll);
    printf("float: %f\n", f);
    printf("double: %lf\n", d);
    printf("char: %c (%d)\n", c, c);
    printf("unsigned int: %u\n", ui);

    // sizeof 연산자
    printf("\n=== sizeof ===\n");
    printf("char: %zu bytes\n", sizeof(char));
    printf("short: %zu bytes\n", sizeof(short));
    printf("int: %zu bytes\n", sizeof(int));
    printf("long: %zu bytes\n", sizeof(long));
    printf("long long: %zu bytes\n", sizeof(long long));
    printf("float: %zu bytes\n", sizeof(float));
    printf("double: %zu bytes\n", sizeof(double));

    // 한계값 (limits.h, float.h)
    printf("\n=== 한계값 ===\n");
    printf("INT_MAX: %d\n", INT_MAX);
    printf("INT_MIN: %d\n", INT_MIN);
    printf("UINT_MAX: %u\n", UINT_MAX);

    // 형변환
    printf("\n=== 형변환 ===\n");
    double pi = 3.14159;
    int intPi = (int)pi;
    printf("double -> int: %d\n", intPi);  // 3

    int a = 10, b = 3;
    double result = (double)a / b;  // 명시적 형변환
    printf("10 / 3 = %lf\n", result);

    // 상수
    const int MAX_STUDENTS = 100;
    #define PI 3.14159
    #define GREETING "Hello, C!"

    printf("\nconst: %d\n", MAX_STUDENTS);
    printf("define PI: %f\n", PI);
    printf("define GREETING: %s\n", GREETING);

    return 0;
}
