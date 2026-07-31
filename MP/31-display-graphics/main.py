# 31: 디스플레이 그래픽 — framebuffer, 도형, 폰트
# 대상: ESP32 + SSD1306(128x64) I2C OLED (SDA=21, SCL=22)
from machine import Pin, I2C
import ssd1306
import time

i2c = I2C(0, scl=Pin(22), sda=Pin(21), freq=400_000)
oled = ssd1306.SSD1306_I2C(128, 64, i2c)

# --- 커스텀 비트맵 (하트, 8x8) --------------------------------------------
HEART = [
    0b01100110,
    0b11111111,
    0b11111111,
    0b01111110,
    0b00111100,
    0b00011000,
    0b00000000,
    0b00000000,
]

ARROW_UP = [
    0b00011000,
    0b00111100,
    0b01111110,
    0b11111111,
    0b00011000,
    0b00011000,
    0b00011000,
    0b00000000,
]

BITMAPS = {"heart": HEART, "arrow": ARROW_UP}


def draw_bitmap(oled, x, y, name, color=1):
    """8x8 비트맵을 좌표(x, y)에 그리기"""
    bitmap = BITMAPS[name]
    for row, bits in enumerate(bitmap):
        for col in range(8):
            if bits & (1 << (7 - col)):
                oled.pixel(x + col, y + row, color)


# --- 커스텀 숫자 폰트 (4x7 세그먼트 스타일) -----------------------------------
DIGITS = {
    "0": (0b01110, 0b10001, 0b10011, 0b10101, 0b11001, 0b10001, 0b01110),
    "1": (0b00100, 0b01100, 0b00100, 0b00100, 0b00100, 0b00100, 0b01110),
    "2": (0b01110, 0b10001, 0b00001, 0b00010, 0b00100, 0b01000, 0b11111),
}


def draw_digit(oled, x, y, digit, color=1):
    """4x7 커스텀 숫자 그리기"""
    for row, bits in enumerate(DIGITS[digit]):
        for col in range(4):
            if bits & (1 << (3 - col)):
                oled.pixel(x + col, y + row, color)


def shapes_demo():
    """도형 그리기 데모"""
    oled.fill(0)
    oled.text("Shapes", 0, 0)
    oled.line(0, 10, 127, 10, 1)
    oled.rect(4, 16, 40, 40, 1)          # 테두리 사각형
    oled.fill_rect(52, 16, 40, 40, 1)    # 채운 사각형
    oled.pixel(110, 36, 1)               # 점
    oled.text("+", 106, 28)              # 텍스트로 십자가
    oled.show()
    time.sleep(2)


def bitmap_demo():
    """비트맵 아이콘 데모"""
    oled.fill(0)
    oled.text("Bitmaps", 0, 0)
    for col, name in enumerate(BITMAPS):
        draw_bitmap(oled, 8 + col * 20, 20, name)
        draw_bitmap(oled, 8 + col * 20, 36, name, color=0)  # 흑반전 예시 없이
        draw_bitmap(oled, 8 + col * 20, 36, name)
    oled.show()
    time.sleep(2)


def custom_font_demo():
    """커스텀 폰트 데모"""
    oled.fill(0)
    oled.text("Custom font", 0, 0)
    draw_digit(oled, 8, 16, "0")
    draw_digit(oled, 16, 16, "1")
    draw_digit(oled, 24, 16, "2")
    oled.show()
    time.sleep(2)


def animation_demo():
    """프레임버퍼를 이용한 공 애니메이션"""
    x, y = 0, 0
    dx, dy = 2, 1
    for _ in range(100):
        oled.fill(0)
        oled.fill_rect(x, y, 6, 6, 1)     # 공
        x += dx
        y += dy
        if x > 121 or x < 0:
            dx = -dx
        if y > 57 or y < 0:
            dy = -dy
        oled.show()
        time.sleep_ms(20)
    oled.fill(0)
    oled.text("Done!", 40, 28)
    oled.show()


def main():
    print("디스플레이 그래픽 데모 시작")
    shapes_demo()
    bitmap_demo()
    custom_font_demo()
    animation_demo()
    print("완료")


if __name__ == "__main__":
    main()
