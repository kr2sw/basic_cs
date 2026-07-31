# 35: 임베디드 C — 레지스터 접근, volatile, ISR 개념, 비트 마스킹

## 레지스터 접근

하드웨어 레지스터는 특정 메모리 주소에 매핑되어 있습니다. 포인터로 직접 접근합니다.

```c
#define GPIO_BASE  0x40020000UL
#define GPIO_ODR  (*(volatile uint32_t*)GPIO_BASE)   // 레지스터 맵핑
```

- 실제로는 MCU 헤더에 `#define`된 매크로를 사용 (STM32: `GPIOA->ODR` 등)

## volatile

컴파일러가 **읽기/쓰기를 최적화하지 못하게** 막는 키워드입니다. 하드웨어 레지스터나 ISR(인터럽트)이 바꾸는 변수에 필수입니다.

```c
volatile uint8_t flag;   // ISR이 값을 바꾸는 변수
```

volatile이 없으면 컴파일러가 루프를 제거하거나 값을 캐시해 잘못된 동작이 발생할 수 있습니다.

## ISR (Interrupt Service Routine)

인터럽트가 발생하면 실행되는 함수입니다. 인터럽트 벡터 테이블에 주소를 등록합니다.

```c
void TIMER_IRQHandler(void) { ... }   // 타이머 인터럽트
```

## 비트 마스킹

```c
#define SET_BIT(reg, bit)   ((reg) |= (1UL << (bit)))
#define CLEAR_BIT(reg, bit) ((reg) &= ~(1UL << (bit)))
#define TOGGLE_BIT(reg, bit) ((reg) ^= (1UL << (bit)))
#define BIT_IS_SET(reg, bit) (((reg) >> (bit)) & 1)
```

본 강의 main.c는 표준 C로 하드웨어를 **시뮬레이션**합니다 (실제 레지스터 접근은 주석으로 제공).

## 실행

```bash
gcc main.c -o main && ./main
```
