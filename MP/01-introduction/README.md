# 마이크로비트 소개

이 레슨에서는 마이크로비트(Micro:Bit)의 기본 개념과 기능을 학습합니다.

## 물리적 핀

마이크로비트는 25개의 핀을 가지고 있습니다:
- 디지털 핀: GPIO 0-19 (디지털 입력/출력)
- 아날로그 핀: GPIO 20-29 (ADC 입력)
- 전원 핀: 3.3V, GND, 1.8V

## setup()

setup()는 한 번만 실행되며, 초기화 작업을 수행합니다.
```python
from microbit import *

def setup():
    display.scroll("Hello!")
    sleep(1000)
```

## first blink

LED 매트릭스를 사용하여 간단한 깜빡임 효과를 보여줍니다.
```python
from microbit import *

def setup():
    led.plot(0, 0)  # 0,0에 LED 점 표시
    sleep(1000)     # 1초 대기
    led.clear()     # LED 클리어
    sleep(1000)

main()