# 모터 제어

이 레슨에서는 DC 모터를 제어하는 방법을 학습합니다.

## DC 모터 제어용 H-브리지

H-브리지는 모터를 양방향으로 제어할 수 있게 해줍니다.
- L293D: 4개의 MOSFET와 제어 로직을 가진 IC
- 2개의 논리 레벨 신호로 PWM 신호 생성
- 전류 보호와 교차 결선 방지 기능 포함

## L293D

마이크로비트에서 4개의 핀을 사용하여 H-브리지와 모터를 제어할 수 있습니다.
```python
import machine

# 모터 제어용 PWM 핀 정의
M1_PWM = machine.Pin(0)  # PWM 핀
M1_DIR = machine.Pin(1)  # 방향 제어 핀

# H-브리지 IC 제어
motor = machine.PWM(M1_PWM)
motor.freq(1000)  # 1kHz PWM 주파수
```

## PWM 신호

마이크로비트의 PWM은 펄스 폭 변조로, 듀티 사이클을 제어합니다.
```python
# 듀티 사이클 50%
motor.duty(2047)  # 0-4095의 값 (부호 없음)
sleep(1000)

# 듀티 사이클 0% (모터 정지)
motor.duty(0)
sleep(1000)

# 듀티 사이클 100%
motor.duty(4095)
sleep(1000)
```

## 모터 제어 예제

```python
from microbit import *

def setup():
    display.scroll("Motor Control")
    sleep(2000)
    display.clear()

def forward():
    led.plot(0, 0)
    motor.duty(2047)  # 전진 (50% 듀티 사이클)

def backward():
    led.plot(4, 0)
    motor.duty(2047)  # 후진 (50% 듀티 사이클)

def stop():
    led.clear()
    motor.duty(0)  # 정지

button_a.on_pressed(forward)
button_b.on_pressed(backward)
button_ab.on_pressed(stop)
```