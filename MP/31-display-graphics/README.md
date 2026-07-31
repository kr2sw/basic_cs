# 31: 디스플레이 그래픽 — Framebuffer, Shapes, Fonts

## 개요

SSD1306 OLED 같은 디스플레이는 픽셀 단위로만 제어합니다. **framebuffer**는 화면 전체를 메모리에 그린 뒤 한 번에 전송해 깜빡임 없이 그래픽을 그리는 핵심 기법입니다. 이번 레슨에서는 도형 그리기, 커스텀 폰트, 프레임버퍼 활용을 다룹니다.

## 프레임버퍼 개념

```python
# 128x64 1비트(흑백) 화면 = 128*64/8 = 1024 바이트
frame = bytearray(1024)
```

- CPU는 메모리 버퍼에 먼저 그리고, `show()`에서 버퍼 전체를 OLED에 전송
- 전송이 화면 리프레시와 동기화되어 화면이 깜빡이지 않음
- 도형/폰트/애니메이션은 모두 이 버퍼를 조작하는 방식

## SSD1306 프레임버퍼 API

`machine`의 `ssd1306` 라이브러리는 framebuffer를 상속받아 다음 함수를 제공합니다.

```python
oled.fill(0)                 # 전체 지우기
oled.pixel(x, y, 1)          # 점 하나
oled.line(x1, y1, x2, y2, 1) # 선
oled.rect(x, y, w, h, 1)     # 사각형 테두리
oled.fill_rect(x, y, w, h, 1)# 채워진 사각형
oled.text("Hi", x, y)        # 내장 8x8 폰트
oled.show()                  # 버퍼 → 디스플레이 전송
```

## 커스텀 폰트

내장 폰트는 8x8뿐입니다. 16x16 한글 같은 큰 글자는 **비트맵**으로 직접 정의해 그립니다.

```python
HEART = [
    0b01100110,
    0b11111111,
    0b11111111,
    0b01111110,
    0b00111100,
    0b00011000,
]

def draw_bitmap(oled, x, y, bitmap, color=1):
    for row, bits in enumerate(bitmap):
        for col in range(8):
            if bits & (1 << (7 - col)):
                oled.pixel(x + col, y + row, color)
```

## 더블 버퍼 애니메이션

두 버퍼를 번갈아 사용하면 프레임 완성 전에 전송되는 **테어링(tearing)** 을 방지할 수 있습니다.

```python
buffers = [bytearray(1024), bytearray(1024)]
current = 0
# 그리기 → swap → show() 반복
```

## 실행/업로드 방법

1. **Thonny IDE**: `MP/31-display-graphics/main.py`를 실행(F5).
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/31-display-graphics/main.py
   ampy --port COM3 run MP/31-display-graphics/main.py
   ```
3. OLED에 도형, 하트 비트맵, 애니메이션이 순서대로 표시됩니다.

## 핵심 개념 요약

- 프레임버퍼: 화면 전체를 메모리에 그려 한 번에 전송
- 점/선/사각형/텍스트를 조합해 그래픽 구성
- 비트맵 배열로 커스텀 폰트·아이콘 표현
- 더블 버퍼링으로 깜빡임·테어링 방지
