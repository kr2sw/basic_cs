#include <stdio.h>

// 한 줄 주석
/*
  여러 줄 주석
*/

int main() {
    // 기본 출력
    printf("Hello, World!\n");
    printf("C 언어 공부 시작!\n");

    // 서식 출력
    printf("정수: %d\n", 42);
    printf("실수: %f\n", 3.14);
    printf("문자: %c\n", 'A');
    printf("문자열: %s\n", "Hello C");
    printf("혼합: %d + %d = %d\n", 10, 20, 10 + 20);

    // scanf 입력
    int age;
    char name[50];

    printf("이름을 입력하세요: ");
    scanf("%s", name);  // 문자열은 & 불필요 (배열 = 주소)

    printf("나이를 입력하세요: ");
    scanf("%d", &age);  // 변수는 & 필요

    printf("안녕하세요, %s님! (%d세)\n", name, age);

    // getchar / putchar
    printf("문자 하나를 입력하세요: ");
    getchar();  // 이전 입력의 개행 문자 제거
    char ch = getchar();
    printf("입력한 문자: ");
    putchar(ch);
    putchar('\n');

    return 0;
}
