#include <stdio.h>
#include <stdint.h>
#include <string.h>

/*
 * [실제 하드웨어 예제 - 주석으로 제공]
 * ARM Cortex-M (예: STM32)에서 GPIO 출력 레지스터에 직접 접근
 *
 * #define GPIOB_BASE  0x40020400UL
 * #define GPIO_ODR    (*(volatile uint32_t*)(GPIOB_BASE + 0x14))
 * #define LED_PIN     5
 *
 * GPIO_ODR |= (1UL << LED_PIN);     // LED 켜기
 * GPIO_ODR &= ~(1UL << LED_PIN);    // LED 끄기
 */

// --- 메모리 맵 레지스터 시뮬레이션 ---
// 실제 MCU에서는 (volatile uint32_t*)주소 로 직접 접근하지만,
// 여기서는 배열로 하드웨어 레지스터 메모리를 흉내냅니다.
static volatile uint8_t regFile[0x100];   // 256바이트 "주소 공간"

#define REG_STATUS  ((volatile uint8_t*)&regFile[0x00])  // 상태 레지스터
#define REG_LED     ((volatile uint8_t*)&regFile[0x04])  // LED 제어 레지스터
#define REG_COUNT   ((volatile uint8_t*)&regFile[0x08])  // 카운터 레지스터

// --- 비트 마스킹 매크로 (임베디드 표준 패턴) ---
#define SET_BIT(reg, bit)      ((reg) |= (1UL << (bit)))
#define CLEAR_BIT(reg, bit)    ((reg) &= ~(1UL << (bit)))
#define TOGGLE_BIT(reg, bit)   ((reg) ^= (1UL << (bit)))
#define BIT_IS_SET(reg, bit)   (((reg) >> (bit)) & 1)

// --- ISR (인터럽트 서비스 루틴) 개념 시뮬레이션 ---
// 인터럽트 벡터 테이블: 함수 포인터 배열로 구현
#define IRQ_COUNT 4

typedef void (*IRQHandler)(void);

static IRQHandler vectorTable[IRQ_COUNT];
static volatile int interruptPending = 0;

// ISR 등록
void registerIRQ(int irq, IRQHandler h) {
    vectorTable[irq] = h;
}

// 하드웨어 인터럽트 발생 → 벡터 테이블에서 핸들러 호출
void triggerIRQ(int irq) {
    printf("  [하드웨어] IRQ %d 발생!\n", irq);
    interruptPending = 1;
    if (vectorTable[irq]) {
        vectorTable[irq]();
    }
    interruptPending = 0;
}

// 실제 ISR들
void timerIRQ(void) {
    printf("  [ISR] 타이머 인터럽트: 카운터 증가\n");
    (*REG_COUNT)++;
}

void buttonIRQ(void) {
    printf("  [ISR] 버튼 인터럽트: LED 토글\n");
    TOGGLE_BIT(*REG_LED, 0);
}

// --- volatile 시연 ---
// volatile이 없으면 컴파일러가 "무한 루프라도 값이 안 바뀐다"고 보고
// 루프를 제거해버릴 수 있습니다. (여기서는 ISR 시뮬레이션이 값을 바꿈)
static volatile int sharedFlag = 0;

void waitForInterrupt(void) {
    int loops = 0;
    while (!sharedFlag) { loops++; if (loops > 1000000) break; }
    printf("  대기 종료 (루프 %d회) sharedFlag = %d\n", loops, sharedFlag);
}

int main(void) {
    printf("=== 레지스터 시뮬레이션 ===\n");
    printf("MCU 주소공간 크기: %zu 바이트\n", sizeof(regFile));

    // 비트 마스킹으로 LED 제어
    printf("\n=== 비트 마스킹 ===\n");
    printf("LED 초기값: 0x%02X\n", *REG_LED);
    SET_BIT(*REG_LED, 0);
    printf("SET_BIT(0)  : 0x%02X (LED 켜짐)\n", *REG_LED);
    CLEAR_BIT(*REG_LED, 0);
    printf("CLEAR_BIT(0): 0x%02X (LED 꺼짐)\n", *REG_LED);
    TOGGLE_BIT(*REG_LED, 0);
    printf("TOGGLE_BIT(0): 0x%02X\n", *REG_LED);
    printf("BIT_IS_SET(0) = %d\n", BIT_IS_SET(*REG_LED, 0));

    printf("\n=== ISR (인터럽트 벡터 테이블) ===\n");
    registerIRQ(0, timerIRQ);
    registerIRQ(1, buttonIRQ);

    triggerIRQ(0);   // 타이머 인터럽트
    triggerIRQ(1);   // 버튼 인터럽트
    printf("  카운터 레지스터: %d\n", *REG_COUNT);

    printf("\n=== volatile 대기 루프 ===\n");
    printf("ISR이 flag를 바꿀 때까지 대기...\n");
    sharedFlag = 0;
    // ISR이 flag를 1로 설정 (개념)
    sharedFlag = 1;
    waitForInterrupt();

    printf("\n※ 실제 임베디드에서는 volatile 포인터로 하드웨어 주소에 직접 접근합니다.\n");
    printf("  main.c 상단 주석 예제를 참고하세요.\n");
    return 0;
}
