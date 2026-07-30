# 음악

이 레슨에서는 사운드 출력, 음계, 주파수, 멜로디 생성을 학습합니다.

## 사운드 출력

마이크로비트의 사운드 출력은 내장 스피커와 함께 사용할 수 있습니다:
- PWM 신호를 통해 타이밍 제어
- 내장 제어 루프를 통해 주파수 제어
- 내장 배터리(1.5V-AA x3)로 구동

## 사운드 제어

마이크로비트의 내장 주기 제어 루프를 사용하여 음높이를 생성합니다:
```python
from microbit import *

def play_tone(frequency, duration):
    """주어진 주파수와 지속 시간으로 음높이 재생"""
    # 주기 = 1 / 주파수
    # 마이크로비트는 내장 주기 제어 루프를 가짐
    
def setup():
    display.scroll("Music")
    sleep(2000)
    display.clear()
```

## 음높이

마이크로비트의 주기 제어 루프를 사용하여 음높이를 저장할 수 있는 배열을 생성합니다:
```python
# 음계를 정의합니다 (중간 C = Do = 261.63 Hz)
C = 262   # Do
D = 294   # Re
E = 330   # Mi
F = 349   # Fa
G = 392   # Sol
A = 440   # La
B = 494   # Si

notes = [C, D, E, F, G, A, B]
```

## 주파수

전파 신호의 일반적인 주파수는 다음과 같습니다:
```python
A4(보편적인 기준 음높이) = 440 Hz
C5(중간 Do) = 523.25 Hz
```

## 멜로디 생성



```python
# C major 도레미
melody = [
    (C, 100),  # Do
    (D, 100),  # Re
    (E, 100),  # Mi
    (C, 200),  # Do
]
```

melody.play();

## 음악 예제 프로그램

```python
from microbit import *

def setup():
    display.scroll("Music")
    sleep(2000)
    display.clear()

def play_note(frequency, duration):
    # 주기 제어 루프 사용 (간소화된 예제)
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
```