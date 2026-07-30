#include <stdio.h>
#include <stdint.h>

// 비트 플래그 정의
#define FLAG_READ   (1 << 0)  // 0001
#define FLAG_WRITE  (1 << 1)  // 0010
#define FLAG_EXEC   (1 << 2)  // 0100
#define FLAG_ADMIN  (1 << 3)  // 1000

// 비트 플래그를 문자열로 변환
void printFlags(uint8_t flags) {
    printf("flags (0x%02x): ", flags);
    if (flags & FLAG_READ)   printf("READ ");
    if (flags & FLAG_WRITE)  printf("WRITE ");
    if (flags & FLAG_EXEC)   printf("EXEC ");
    if (flags & FLAG_ADMIN)  printf("ADMIN ");
    printf("\n");
}

int main() {
    printf("=== 비트 연산자 ===\n\n");

    // 기본 비트 연산
    uint8_t a = 0b1100;  // 12
    uint8_t b = 0b1010;  // 10

    printf("a = 0b1100 (%d)\n", a);
    printf("b = 0b1010 (%d)\n", b);
    printf("a & b = 0b%04x (%d)\n", a & b, a & b);  // 1000 (8)
    printf("a | b = 0b%04x (%d)\n", a | b, a | b);  // 1110 (14)
    printf("a ^ b = 0b%04x (%d)\n", a ^ b, a ^ b);  // 0110 (6)
    printf("~a    = 0b%04x (%d)\n", (uint8_t)~a, (uint8_t)~a);

    // 시프트 연산
    uint8_t val = 1;
    printf("\n=== 시프트 연산 ===\n");
    for (int i = 0; i < 8; i++) {
        printf("1 << %d = %d (0b%08x)\n", i, val << i, val << i);
    }

    printf("\n50 >> 2 = %d\n", 50 >> 2);  // 12 (50/4)
    printf("5 << 3  = %d\n", 5 << 3);     // 40 (5*8)

    // 비트 플래그
    printf("\n=== 비트 플래그 ===\n");
    uint8_t permissions = 0;

    // 플래그 설정 (OR)
    permissions |= FLAG_READ;
    permissions |= FLAG_WRITE;
    printFlags(permissions);

    // 플래그 확인 (AND)
    if (permissions & FLAG_READ) {
        printf("읽기 권한 있음\n");
    }
    if (permissions & FLAG_EXEC) {
        printf("실행 권한 있음\n");
    } else {
        printf("실행 권한 없음\n");
    }

    // 플래그 토글 (XOR)
    permissions ^= FLAG_WRITE;
    printFlags(permissions);  // WRITE 제거

    permissions ^= FLAG_WRITE;
    printFlags(permissions);  // WRITE 다시 추가

    // 플래그 제거 (AND NOT)
    permissions &= ~FLAG_READ;
    printFlags(permissions);  // READ 제거

    // 비트 마스킹
    printf("\n=== 비트 마스킹 ===\n");
    uint16_t packed = 0;
    // 앞 8비트: x 좌표, 뒤 8비트: y 좌표
    uint8_t x = 35, y = 200;
    packed = (x << 8) | y;
    printf("packed = 0x%04x\n", packed);

    uint8_t unpackedX = (packed >> 8) & 0xFF;
    uint8_t unpackedY = packed & 0xFF;
    printf("x = %d, y = %d\n", unpackedX, unpackedY);

    // 2의 보수
    printf("\n=== 2의 보수 ===\n");
    int8_t pos = 42;
    int8_t neg = -42;
    printf("%d = 0b", pos);
    for (int i = 7; i >= 0; i--) {
        printf("%d", (pos >> i) & 1);
    }
    printf("\n");

    printf("%d = 0b", neg);
    for (int i = 7; i >= 0; i--) {
        printf("%d", (neg >> i) & 1);
    }
    printf("\n");

    // 짝수/홀수 확인 (최하위 비트)
    printf("\n=== 짝수/홀수 ===\n");
    for (int i = 1; i <= 10; i++) {
        printf("%d: %s\n", i, (i & 1) ? "홀수" : "짝수");
    }

    return 0;
}
