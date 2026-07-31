# 22: SPI 통신 — SPI Communication

SPI(Serial Peripheral Interface)는 고속 직렬 통신 프로토콜로, 시프트 레지스터, SD 카드, 디스플레이 등 많은 장치에서 사용됩니다.

## 학습 내용
- SPI 통신 원리와 4개 신호선 (MOSI, MISO, SCK, SS)
- SPI.h 라이브러리 사용
- 74HC595 시프트 레지스터로 LED 제어
- 여러 장치의 SS(Chip Select) 선택 방식

## SPI 원리

SPI는 Master가 클록(SCK)을 생성하고, 데이터를 MOSI로 보내고 MISO로 받습니다. 통신 대상은 SS(칩 선택) 핀을 LOW로 내려 지정합니다.

```
Uno  SPI 핀: SCK=13, MISO=12, MOSI=11, SS=10
```

```cpp
#include <SPI.h>
SPI.begin();
// 1바이트 송수신 (동시에 이루어짐)
byte result = SPI.transfer(0x55);
```

## 74HC595 시프트 레지스터

시프트 레지스터는 1바이트(8비트)를 직렬로 받아 8개의 병렬 출력으로 바꿔줍니다. Uno의 3개 핀(데이터/클록/래치)으로 8개 LED를 제어할 수 있습니다.

```cpp
digitalWrite(LATCH_PIN, LOW);  // 출력 갱신 전 래치 비활성화
SPI.transfer(pattern);         // 데이터 직렬 전송
digitalWrite(LATCH_PIN, HIGH); // 래치로 8개 출력을 한꺼번에 갱신
```

## Chip Select (SS)

SPI는 여러 슬레이브를 연결할 수 있지만, 각 장치마다 별도의 SS 핀이 필요합니다. 데이터를 보낼 장치의 SS만 LOW로 두면 됩니다.

```cpp
digitalWrite(SD_SS, LOW);   // SD 카드 선택
SPI.transfer(cmd);
digitalWrite(SD_SS, HIGH);  // 선택 해제
```

## 회로 연결 (74HC595)

| 74HC595 핀 | Arduino Uno |
|-----------|-------------|
| 14 (SER/DATA) | 11 (MOSI) |
| 11 (SRCLK) | 13 (SCK) |
| 12 (RCLK/LATCH) | 10 (SS) |
| 10, 16 (OE, VCC) | 5V |
| 8, 13 (GND, OE) | GND |

> OE 핀(13)은 LOW로 두어야 출력이 활성화됩니다. LED는 220Ω 저항을 거쳐 출력 핀(Q0~Q7)에 연결합니다.

## 실행 방법

1. Arduino IDE에서 이 챕터의 `.ino` 파일을 엽니다.
2. **도구 → 보드**에서 Arduino Uno를 선택합니다.
3. 위 회로대로 연결하고 **업로드** 후 시리얼 모니터(9600)를 확인합니다.
4. LED가 왼쪽→오른쪽, 오른쪽→왼쪽으로 이동하는 패턴이 보입니다.

## 응용 아이디어

- 7세그먼트 디스플레이 2자리 제어
- SD 카드(23장), TFT 디스플레이 등 SPI 장치 추가
- 다중 74HC595 캐스케이드 연결로 LED 매트릭스 구성
