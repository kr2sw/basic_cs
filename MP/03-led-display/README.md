# LED 디스플레이

이 레슨에서는 마이크로비트의 LED 디스플레이를 사용하는 방법을 학습합니다.

## display.show()

텍스트나 이모티콘 패턴을 디스플레이에 표시합니다.
```python
from microbit import *

def setup():
    display.show("Hi")      # 텍스트 표시
    sleep(1000)
    display.clear()
    
    # 이모티콘 표시
    display.show(Image.HEART)
    sleep(1000)
    display.clear()
```

## display.scroll()

텍스트를 좌우로 스크롤하여 표시합니다.
```python
display.scroll("Hello World!")
```

## 패턴, 하트, 화살표 등

마이크로비트에는 여러 이모티콘이 내장되어 있습니다:
- Image.HEART, Image.SMILE, Image.SAD
- Image.UP, Image.DOWN, Image.LEFT, Image.RIGHT
- Image.DIAMOND, Image.SQUARE, Image.TRIANGLE
- Image.MUSIC_NOTE
- 사용자 정의 패턴 생성

```python
# 화살표 패턴 표시
display.show(Image.UP)
sleep(1000)
display.show(Image.DOWN)
sleep(1000)

# 음악 노트 표시
display.show(Image.MUSIC_NOTE)
sleep(1000)
```

## 사용자 정의 패턴

8x5 픽셀 크기의 패턴을 생성할 수 있습니다.
```python
# 마음 모양 패턴 생성
heart = Image("""
    . . # . . . . . .
    . # # # . . . . .
    # # # # # . . . .
    # # # # # . . . .
    . # # # # . . . .
    . . # # . . . . .
    . . . . . . . . .
    . . . . . . . . .
""")
```