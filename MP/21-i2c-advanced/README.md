# 21: 고급 I2C — Multi-Device, Scan, Direct Register Access

## 개요

기초 과정에서 단일 I2C 센서를 다뤘다면, 이번 레슨에서는 하나의 I2C 버스에 **여러 디바이스**를 연결하고, **레지스터를 직접 읽고 쓰는** 고급 기법을 배웁니다. I2C는 SDA(데이터), SCL(클록) 두 가닥만으로 최대 128개 디바이스를 주소로 구분해 통신할 수 있습니다.

## I2C 버스 스캔

보드에 어떤 디바이스가 어떤 주소로 연결되어 있는지 먼저 확인합니다.

```python
from machine import Pin, I2C
i2c = I2C(0, scl=Pin(22), sda=Pin(21), freq=400_000)
print(i2c.scan())  # 예: [60, 104] → MPU6050(0x68), BMP280(0x76)
```

- 주소가 겹치면 디바이스의 AD0 핀으로 주소를 바꿀 수 있습니다 (예: 0x68 → 0x69).
- 서로 다른 주소면 버스에 몇 개든 병렬로 연결할 수 있습니다.

## 레지스터 직접 접근

모든 I2C 디바이스는 내부에 **레지스터(register)** 배열을 갖고 있습니다. 센서는 여기에 데이터를 쓰고 읽습니다.

```python
from machine import I2C
from time import sleep

def read_u8(i2c, addr, reg):
    i2c.writeto(addr, bytes([reg]))       # 읽을 레지스터 주소 전송
    data = i2c.readfrom(addr, 1)          # 1바이트 읽기
    return data[0]

def write_u8(i2c, addr, reg, value):
    i2c.writeto(addr, bytes([reg, value]))  # 레지스터+값을 한 번에 전송

def read_regs(i2c, addr, reg, n):
    i2c.writeto(addr, bytes([reg]))
    return i2c.readfrom(addr, n)          # n바이트 연속 읽기
```

## 레지스터 비트 연산

설정 레지스터는 보통 몇 개의 비트 필드로 나뉩니다. `& | >>` 연산으로 특정 비트만 바꿉니다.

```python
# 예: POWER_MANAGEMENT 레지스터(0x6B)의 DEVICE_RESET 비트(비트 7)만 1로
val = read_u8(i2c, 0x68, 0x6B)
write_u8(i2c, 0x68, 0x6B, val | (1 << 7))
```

## 실제 예제: MPU6050 자이로/가속도

main.py는 버스 스캔 → WHO_AM_I 확인 → 설정 → 센서 데이터를 레지스터로 직접 읽어 출력하는 전체 과정을 보여줍니다.

## 실행/업로드 방법

1. **Thonny IDE**: 보드를 연결하고 `MP/21-i2c-advanced/main.py`를 열어 실행(F5)합니다.
2. **ampy** (명령줄):
   ```bash
   ampy --port COM3 put MP/21-i2c-advanced/main.py
   ampy --port COM3 run MP/21-i2c-advanced/main.py
   ```
3. 시리얼 모니터(115200)에서 스캔 결과와 센서 값을 확인합니다.

## 핀 연결

- VCC → 3.3V, GND → GND, SDA → GPIO21, SCL → GPIO22 (ESP32 기준)
- 여러 센서를 병렬로 연결하면 각각 다른 주소로 스캔됩니다.
