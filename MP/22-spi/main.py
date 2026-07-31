# 22: SPI 통신 — 하드웨어 SPI, 비트뱅킹, SSD1306 OLED
# 대상: ESP32, SSD1306(128x64) SPI OLED
from machine import Pin, SPI
from time import sleep_ms, ticks_ms
import ssd1306

# --- 핀 정의 -----------------------------------------------------
SCK_PIN, MOSI_PIN = 18, 23        # SPI 데이터 핀
DC_PIN, RES_PIN, CS_PIN = 4, 16, 5  # OLED 제어 핀


def bitbang_loopback_test():
    """비트뱅킹으로 두 핀을 직접 연결해 루프백 테스트"""
    import machine
    mosi = Pin(23, Pin.OUT)
    miso = Pin(19, Pin.IN)
    sck = Pin(18, Pin.OUT)

    ok = 0
    for value in range(256):          # 0~255 전 값 테스트
        rx = 0
        for bit in range(8):
            mosi.value((value >> (7 - bit)) & 1)   # MSB부터 송신
            sck.value(1)
            rx = (rx << 1) | miso.value()          # 수신
            sck.value(0)
        if rx == value:
            ok += 1
    print(f"비트뱅킹 루프백: {ok}/256 통과")
    return ok == 256


def write_byte_bitbang(spi_mosi, spi_sck, value):
    """비트뱅킹으로 1바이트 전송 (SSD1306은 MOSI만 필요)"""
    for bit in range(8):
        spi_mosi.value((value >> (7 - bit)) & 1)
        spi_sck.value(1)
        spi_sck.value(0)


def main():
    print("=== 1) 하드웨어 SPI 초기화 ===")
    spi = SPI(1, baudrate=10_000_000, polarity=0, phase=0,
              sck=Pin(SCK_PIN), mosi=Pin(MOSI_PIN), miso=Pin(19))
    print("SPI:", spi)

    print("=== 2) 비트뱅킹 루프백 테스트 ===")
    bitbang_loopback_test()

    print("=== 3) SSD1306 OLED 초기화 ===")
    dc = Pin(DC_PIN, Pin.OUT)
    res = Pin(RES_PIN, Pin.OUT)
    cs = Pin(CS_PIN, Pin.OUT)

    res.value(0)          # 리셋 후
    sleep_ms(10)
    res.value(1)
    sleep_ms(10)

    oled = ssd1306.SSD1306_SPI(128, 64, spi, dc, res, cs)
    oled.fill(0)
    oled.text("SPI OLED Demo", 0, 0)
    oled.text("HW SPI @10MHz", 0, 16)
    oled.rect(0, 32, 128, 16, 1)
    oled.fill_rect(4, 36, 120, 8, 1)
    oled.show()

    print("=== 4) 대역폭 측정 ===")
    buf = bytearray(128 * 64 // 8)    # 1KB 프레임 버퍼
    start = ticks_ms()
    for _ in range(50):
        oled.blit(buf, 0, 0)
        oled.show()
    elapsed = ticks_ms() - start
    total = 50 * len(buf)
    print(f"50회 전송: {elapsed}ms, 평균 {total // max(elapsed, 1) * 8} kbit/s")

    sleep_ms(2000)
    oled.fill(0)
    for x in range(0, 128, 8):        # 그리드 패턴
        oled.vline(x, 0, 64, 1)
    for y in range(0, 64, 8):
        oled.hline(0, y, 128, 1)
    oled.text("Bit-bang vs HW", 0, 28)
    oled.show()

    print("완료 — OLED에서 비트뱅킹/하드웨어 SPI 결과 확인")


if __name__ == "__main__":
    main()
