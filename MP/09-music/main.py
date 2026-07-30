# Music

from microbit import *

def setup():
    display.scroll("Music")
    sleep(2000)
    display.clear()

def play_note(frequency, duration):
    # 주기 제어 루프를 사용한 음높이 재생 (간소화된 예제)
    display.show("O")
    sleep(duration)
    display.clear()

# 멜로디 재생
melody = [
    (262, 100),  # Do
    (294, 100),  # Re
    (330, 100),  # Mi
    (262, 200),  # Do
]

for freq, dur in melody:
    play_note(freq, dur)

while True:
    sleep(100)