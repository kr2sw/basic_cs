#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// --- 1. XOR 암호화 (패턴 키) ---
void xorEncryptDecrypt(char* data, const char* key) {
    int klen = (int)strlen(key);
    for (int i = 0; data[i]; i++) {
        data[i] ^= key[i % klen];   // 같은 연산으로 암/복호화
    }
}

void demoXor(void) {
    printf("=== 1. XOR 암호화 ===\n");
    char msg[] = "Top Secret";
    const char* key = "key";
    printf("원문: %s\n", msg);
    xorEncryptDecrypt(msg, key);
    printf("암호문: %s\n", msg);
    xorEncryptDecrypt(msg, key);
    printf("복호화: %s\n", msg);
}

// --- 2. 해시 함수 구현 ---
// FNV-1a
unsigned long hashFnv1a(const char* s) {
    unsigned long h = 2166136261UL;
    while (*s) {
        h ^= (unsigned char)*s++;
        h *= 16777619UL;
    }
    return h;
}

// djb2
unsigned long hashDjb2(const char* s) {
    unsigned long h = 5381;
    while (*s) h = h * 33 + (unsigned char)*s++;
    return h;
}

void demoHash(void) {
    printf("\n=== 2. 해시 함수 (FNV-1a / djb2) ===\n");
    const char* words[] = {"cat", "dog", "car", "care"};
    for (int i = 0; i < 4; i++) {
        printf("%-5s FNV=%08lX  djb2=%08lX\n",
               words[i], hashFnv1a(words[i]), hashDjb2(words[i]));
    }
    printf("※ 비슷한 입력도 완전히 다른 해시값을 만듭니다.\n");
}

// --- 3. HMAC 개념 (단순 해시 기반) ---
// 실전 표준 HMAC(SHA)과 구조는 같지만 학습용으로 FNV 해시를 사용
unsigned long simpleHmac(const char* key, const char* msg) {
    // 블록 크기 64바이트 가정 (개념 시연)
    unsigned char ipad[64], opad[64];
    size_t klen = strlen(key);
    for (size_t i = 0; i < 64; i++) {
        unsigned char k = (i < klen) ? (unsigned char)key[i] : 0;
        ipad[i] = k ^ 0x36;   // ipad = key ^ 0x36 반복
        opad[i] = k ^ 0x5C;   // opad = key ^ 0x5C 반복
    }

    // inner = H((ipad || msg))
    char innerInput[160];
    memcpy(innerInput, ipad, 64);
    strcpy(innerInput + 64, msg);
    unsigned long inner = hashFnv1a(innerInput);

    // outer = H((opad || inner))
    char outerInput[80];
    memcpy(outerInput, opad, 64);
    memcpy(outerInput + 64, &inner, sizeof(inner));
    return hashFnv1a(outerInput);
}

void demoHmac(void) {
    printf("\n=== 3. HMAC 개념 ===\n");
    unsigned long mac1 = simpleHmac("secret-key", "hello");
    unsigned long mac2 = simpleHmac("secret-key", "hello");   // 같은 입력
    unsigned long mac3 = simpleHmac("secret-key", "hellx");   // 입력 변경
    printf("MAC(hello)  = %08lX\n", mac1);
    printf("MAC(hello)  = %08lX (같으면 무결성 확인)\n", mac2);
    printf("MAC(hellx)  = %08lX (메시지 변경 시 달라짐)\n", mac3);
}

// --- 4. 난수 생성기 (LCG) ---
static unsigned long lcgState;

void lcgSeed(unsigned long seed) { lcgState = seed; }

unsigned long lcgNext(void) {
    lcgState = (lcgState * 1103515245UL + 12345UL) % 2147483648UL;
    return lcgState;
}

void demoRandom(void) {
    printf("\n=== 4. 난수 (LCG - 예측 가능하므로 학습용) ===\n");
    lcgSeed(42);
    printf("LCG 난수 5개: ");
    for (int i = 0; i < 5; i++) printf("%lu ", lcgNext() % 1000);
    printf("\n");

    printf("rand() 난수 5개: ");
    srand(42);
    for (int i = 0; i < 5; i++) printf("%d ", rand() % 1000);
    printf("\n");

    printf("※ 암호화에 쓸 난수는 rand가 아니라 OS 제공 난수 소스를 사용하세요.\n");
    printf("  POSIX: /dev/urandom, Windows: CryptGenRandom\n");
}

int main(void) {
    demoXor();
    demoHash();
    demoHmac();
    demoRandom();

    printf("\n※ 본 강의 코드는 교육용입니다. 실전 보안에는 검증된 라이브러리(OpenSSL 등)를 사용하세요.\n");
    return 0;
}
