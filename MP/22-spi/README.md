# 22: SPI 통신 — SPI Devices, Bit-Banging, OLED

## 개요

SPI는 I2C보다 빠른 **동기 직렬 통신**으로, 4가닥(MOSI, MISO, SCK, CS)을 사용합니다. 클록(SCK)이 흐를 때마다 데이터를 주고받으며, **CS 핀**으로 어떤 디바이스와 통신할지 선택합니다. 이번 레슨에서는 하드웨어 SPI와 직접 GPIO로 제어하는 **비트뱅킹(bit-banging)**, 그리고 SSD1306 OLED 디스플레이를 다룹니다.

## SPI vs I2C

| 항목 | SPI | I2C |
|------|-----|-----|
| 배선 | MOSI, MISO, SCK, CS(디바이스마다) | SDA, SCL (2가닥) |
| 속도 | 수십 MHz 가능 | 최대 수 MHz |
| 주소 | CS 핀으로 선택 | 7비트 주소 |
| 사용처 | OLED, SD카드, LoRa, FLASH | 센서, RTC |

## 하드웨어 SPI 사용

```python
from machine import Pin, SPI
spi = SPI(1, baudrate=8_000_000, polarity=0, phase=0,
          sck=Pin(18), mosi=Pin(23), miso=Pin(19))
cs = Pin(5, Pin.OUT)

cs.value(0)          # CS 활성화
data = spi.write_readinto(b"\x03\x00\x00", bytearray(3))  # 읽기
cs.value(1)          # CS 비활성화
```

- **polarity(CPOL)**: 클록의 유휴 레벨, **phase(CPHA)**: 데이터 샘플링 시점
- 디바이스 데이터시트의 SPI 모드(Mode 0~3)에 맞춥니다.

## 비트뱅킹 (Bit-Banging)

하드웨어 SPI가 없는 핀이나 모듈을 쓸 때는 GPIO로 직접 클록을 흘리며 비트를 주고받습니다.

```python
from machine import Pin

def bitbang_readbyte(sck, mosi, miso, msb_first=True):
    value = 0
    for i in range(8):
        if msb_first:
            value <<= 1
            value |= miso.value()
        else:
            value |= miso.value() << i
        sck.value(1); sck.value(0)   # 클록 토글
    return value
```

## SSD1306 OLED

SSD1306은 I2C와 SPI 두 가지로 통신할 수 있는 128x64 OLED입니다.

```python
from machine import Pin, SPI
import ssd1306

spi = SPI(1, baudrate=10_000_000, sck=Pin(18), mosi=Pin(23))
dc, res, cs = Pin(4, Pin.OUT), Pin(16, Pin.OUT), Pin(5, Pin.OUT)
oled = ssd1306.SSD1306_SPI(128, 64, spi, dc, res, cs)
oled.text("SPI OLED!", 0, 0)
oled.show()
```

## 실행/업로드 방법

1. **Thonny IDE**: 보드를 연결하고 `MP/22-spi/main.py`를 열어 실행(F5)합니다.
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/22-spi/main.py
   ampy --port COM3 run MP/22-spi/main.py
   ```
3. OLED에 그래픽이 그려지고, 시리얼로 SPI 대역폭 측정 결과가 출력됩니다.

## 핀 연결 (ESP32 ↔ SSD1306 SPI)

| SSD1306 | ESP32 |
|---------|-------|
| GND | GND |
| VCC | 3.3V |
| SCL(SCK) | GPIO18 |
| SDA(MOSI) | GPIO23 |
| RES | GPIO16 |
| DC | GPIO4 |
| CS | GPIO5 |
