# LED Display

from microbit import *

# LED 표시
led.toggle(0, 0)
sleep(100)

# LED 클리어
led.clear()
sleep(100)

# 0,0에 LED 점 표시
led.plot(0, 0)
sleep(100)

led.clear()
sleep(100)

# 3,3에 LED 점 표시
led.plot(3, 3)
sleep(100)

led.clear()
sleep(100)

# 디스플레이에 텍스트 표시
display.show("Hello")
sleep(1000)
display.clear()
sleep(1000)

# 디스플레이에 하트 표시
display.show(Image.HEART)
sleep(1000)
display.clear()
sleep(1000)

# 디스플레이에 위쪽 화살표 표시
display.show(Image.UP)
sleep(1000)
display.clear()
sleep(1000)

# 디스플레이에 음악 노트 표시
display.show(Image.MUSIC_NOTE)
sleep(1000)
display.clear()
sleep(1000)

while True:
    sleep(100)