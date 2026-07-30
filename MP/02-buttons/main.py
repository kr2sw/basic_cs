# Buttons

from microbit import *

# 버튼_a 이벤트 핸들러
def on_button_a():
    led.toggle(0, 0)

# 버튼_b 이벤트 핸들러
def on_button_b():
    display.scroll("Button B")
    sleep(500)
    display.clear()

# 버튼_ab 이벤트 핸들러
def on_button_ab():
    sound_level = accelerometer.get_x()
    display.show(sound_level)

button_a.on_pressed(on_button_a)
button_b.on_pressed(on_button_b)
button_ab.on_pressed(on_button_ab)

while True:
    sleep(100)