#include <stdio.h>

// 객체형 매크로
#define PI 3.14159
#define MAX_SIZE 100
#define APP_NAME "Preprocessor Demo"

// 함수형 매크로
#define SQUARE(x) ((x) * (x))
#define MAX(a, b) ((a) > (b) ? (a) : (b))
#define MIN(a, b) ((a) < (b) ? (a) : (b))

// # 연산자 (문자열화)
#define STRINGIFY(x) #x

// ## 연산자 (토큰 결합)
#define CONCAT(a, b) a ## b

// 가변 인자 매크로 (C99+)
#define LOG(format, ...) \
    printf("[%s:%d] " format "\n", __FILE__, __LINE__, ##__VA_ARGS__)

// 조건부 컴파일
#define DEBUG 1

int main() {
    // 매크로 상수
    printf("PI = %f\n", PI);
    printf("MAX_SIZE = %d\n", MAX_SIZE);
    printf("APP_NAME = %s\n", APP_NAME);

    // 함수형 매크로
    printf("\n=== 함수형 매크로 ===\n");
    printf("SQUARE(5) = %d\n", SQUARE(5));
    printf("MAX(10, 20) = %d\n", MAX(10, 20));
    printf("MIN(3.14, 2.71) = %f\n", MIN(3.14, 2.71));

    // 매크로 주의사항: SQUARE(1+2)는 ((1+2)*(1+2)) = 9
    printf("SQUARE(1+2) = %d\n", SQUARE(1+2));

    // # 연산자
    printf("\n=== # 연산자 ===\n");
    printf("%s\n", STRINGIFY(Hello World));
    printf("%s\n", STRINGIFY(3 + 5 = 8));

    // ## 연산자
    printf("\n=== ## 연산자 ===\n");
    int CONCAT(my, Variable) = 42;  // int myVariable = 42;
    printf("myVariable = %d\n", myVariable);

    // 미리 정의된 매크로
    printf("\n=== 미리 정의된 매크로 ===\n");
    printf("__FILE__: %s\n", __FILE__);
    printf("__LINE__: %d\n", __LINE__);
    printf("__DATE__: %s\n", __DATE__);
    printf("__TIME__: %s\n", __TIME__);
    printf("__STDC__: %d\n", __STDC__);

    // 가변 인자 매크로 (LOG)
    printf("\n=== LOG 매크로 ===\n");
    LOG("프로그램 시작");
    LOG("x = %d, y = %d", 10, 20);
    LOG("종료");

    // 조건부 컴파일
    printf("\n=== 조건부 컴파일 ===\n");
#if DEBUG
    printf("DEBUG 모드입니다.\n");
#else
    printf("릴리스 모드입니다.\n");
#endif

    // #ifdef / #ifndef
#ifdef MAX_SIZE
    printf("MAX_SIZE가 정의되어 있습니다: %d\n", MAX_SIZE);
#endif

#ifndef NOT_DEFINED
    printf("NOT_DEFINED는 정의되지 않았습니다.\n");
#endif

#undef PI  // 매크로 해제
// printf("PI = %f\n", PI);  // 컴파일 에러

    return 0;
}
