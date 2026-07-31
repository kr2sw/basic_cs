#include <stdio.h>
#include <stdint.h>
#include <string.h>

/*
 * [extern "C" 패턴 - 헤더 파일에서 주로 사용]
 * C++ 컴파일러에서 이 헤더를 포함하면 C ABI로 심볼을 내보냅니다.
 *
 * #ifdef __cplusplus
 * extern "C" {
 * #endif
 *
 * int add(int a, int b);
 * double mul(double a, double b);
 *
 * #ifdef __cplusplus
 * }
 * #endif
 */

// --- 1. C로 만든 "라이브러리 함수" (다른 언어에서 호출될 함수들) ---
// 이름/타입이 그대로 바이너리 심볼이 됩니다 (Python ctypes가 접근 가능)
int cAdd(int a, int b) { return a + b; }
int cMul(int a, int b) { return a * b; }

// 버퍼에 문자열 작성 후 길이 반환 (ctypes에서 c_char_p 버퍼 전달)
int cWriteName(char* buf, int cap, const char* name) {
    int n = snprintf(buf, (size_t)cap, "Hello, %s!", name);
    return n;
}

// --- 2. 구조체 레이아웃 (ABI의 핵심) ---
#pragma pack(push, 1)   // 패딩 제거
typedef struct {
    uint16_t magic;      // 2바이트
    uint32_t length;     // 4바이트
    uint8_t  flags;      // 1바이트
} PackedHeader;
#pragma pack(pop)

typedef struct {
    uint16_t magic;
    uint32_t length;
    uint8_t  flags;
} NormalHeader;

void demoStructLayout(void) {
    printf("=== 구조체 레이아웃 (패딩) ===\n");
    printf("NormalHeader 크기: %zu 바이트 (패딩 포함)\n", sizeof(NormalHeader));
    printf("PackedHeader 크기: %zu 바이트 (pack(1))\n", sizeof(PackedHeader));
    printf("  각 멤버: 2+4+1 = 7 → pack으로 7바이트 보장\n");
    printf("※ 파일/네트워크 프로토콜은 패딩 차이 때문에 pack이 필요할 수 있습니다.\n");
}

// --- 3. 엔디언 (바이트 순서) ---
void demoEndian(void) {
    uint32_t x = 0x01020304;
    unsigned char* b = (unsigned char*)&x;
    int little = (b[0] == 0x04);

    printf("\n=== 엔디언 ===\n");
    printf("바이트 순서: %02X %02X %02X %02X\n", b[0], b[1], b[2], b[3]);
    printf("현재 시스템: %s 엔디언\n", little ? "리틀" : "빅");
    printf("※ 호스트 엔디언과 파일/네트워크(빅)가 다르면 변환이 필요합니다.\n");
}

// --- 4. 호출 규약 (Windows x86) ---
// 32비트 Windows에서 __cdecl(기본) / __stdcall 구분
// 64비트에서는 단일 호출 규약만 존재
#if defined(_MSC_VER) && defined(_M_IX86)
#define MY_CALL __cdecl
#else
#define MY_CALL
#endif

int MY_CALL exportedFunction(int a) {
    return a * 2;
}

int main(void) {
    printf("=== C ABI / 언어 간 연동 ===\n\n");

    printf("ctypes에서 호출될 C 함수:\n");
    printf("  cAdd(3, 4) = %d\n", cAdd(3, 4));
    printf("  cMul(6, 7) = %d\n", cMul(6, 7));

    char buf[64];
    int n = cWriteName(buf, sizeof(buf), "Python");
    printf("  cWriteName → \"%s\" (%d자)\n", buf, n);
    printf("  exportedFunction(21) = %d\n", exportedFunction(21));

    demoStructLayout();
    demoEndian();

    printf("\n=== Python ctypes 호출 예시 ===\n");
    printf("  lib = ctypes.CDLL('./libinterop.so')\n");
    printf("  lib.cAdd.argtypes = [c_int, c_int]\n");
    printf("  lib.cAdd.restype = c_int\n");
    printf("  print(lib.cAdd(3, 4))  # → 7\n");

    return 0;
}
