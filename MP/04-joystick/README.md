# 조이스틱

이 레슨에서는 조이스틱(아날로그 입력)을 사용하여 마이크로비트를 제어하는 방법을 학습합니다.

## 아날로그 입력 읽기

마이크로비트에는 built-in 아날로그 입력이 있습니다:
- 핀 0-19: ADC 아날로그 입력
- ADC.read() 또는 핀.read_analog()로 값 읽기 (0-4095)

## x/y 축

X, Y, Z 가속도 센서를 사용하여 조이스틱과 유사한 값을 얻습니다.
```python
from microbit import *

def setup():
    x = accelerometer.get_x()  # X축 값 반환 (-1024 to 1024)
    y = accelerometer.get_y()  # Y축 값 반환 (-1024 to 1024)
    z = accelerometer.get_z()  # Z축 값 반환 (-1024 to 1024)
    
    display.show(str(x) + str(y))
    sleep(1000)
```

## 임계값

조이스틱의 움직임을 감지하기 위해 임계값을 설정합니다.
```python
threshold = 100  # 임계값 설정

if abs(x) < threshold:  # X축이 임계값보다 작으면 (중립 위치)
    # 중립 위치 동작
elif x < 0:  # X축이 음수이면 (왼쪽으로 움직임)
    # 왼쪽으로 움직임 동작
else:
    # 오른쪽으로 움직임 동작
```

## 조이스틱 예제

```python
from microbit import *

def setup():
    display.scroll("Joystick Test")
    sleep(2000)
    display.clear()
    
    x = accelerometer.get_x()
    y = accelerometer.get_y()
    
    if abs(x) > 100 and abs(y) < 100:
        if x < 0:
            # 왼쪽으로 움직임
            led.plot(0, 0)
            display.show(Image.LEFT)
        else:
            # 오른쪽으로 움직임
            led.plot(4, 0)
            display.show(Image.RIGHT)
    elif abs(y) > 100 and abs(x) < 100:
        if y < 0:
            # 위쪽으로 움직임
            led.plot(2, 0)
            display.show(Image.UP)
        else:
            # 아래쪽으로 움직임
            led.plot(2, 4)
            display.show(Image.DOWN)
    else:
        led.clear()
        display.clear()
```