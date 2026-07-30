# 버튼 사용법

이 레슨에서는 마이크로비트의 버튼을 사용하여 프로그램을 제어하는 방법을 학습합니다.

## 버튼_a, 버튼_b, 버튼_ab

마이크로비트에는 두 개의 물리적 버튼이 있습니다:
- 버튼_a: 왼쪽 상단 버튼
- 버튼_b: 오른쪽 상단 버튼
- 버튼_ab: 두 버튼이 동시에 눌렸을 때

## 버튼 이벤트 핸들러

버튼이 눌렸을 때 실행되는 코드를 작성합니다.
```python
from microbit import *

# 버튼_a 이벤트 핸들러
def on_button_a():
    led.toggle(0, 0)  # 0,0에 LED 점 토글

# 버튼_b 이벤트 핸들러
def on_button_b():
    display.scroll("Button B")
    sleep(500)
    display.clear()

# 버튼_ab 이벤트 핸들러
def on_button_ab():
    sound_level = accelerometer.get_x()
    display.show(sound_level)

# 이벤트 등록
button_a.on_pressed(on_button_a)
button_b.on_pressed(on_button_b)
button_ab.on_pressed(on_button_ab)
```