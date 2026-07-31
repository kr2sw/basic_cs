# 32: 전원 관리 — Deep Sleep, Periodic Wake, Current Optimization
# 대상: ESP32 (딥 슬립 및 RTC 메모리)
import machine
from machine import Pin, RTC, Timer, deepsleep, DEEPSLEEP_RESET
import time

# --- 저전력 설계 포인트 ------------------------------------------------------
# 1. 쓰지 않는 주변장치(ADC, WiFi, BT) 끄기
# 2. 딥 슬립: CPU와 대부분 주변장치 전원 차단, RTC/UART만 유지
# 3. 주기적 웨이크: 타이머(최소) 또는 외부 핀, RTC GPIO
# 4. RTC 메모리에 상태 저장 → 재시작 후 복원

# 상태 카운터는 RTC 메모리에 저장 (리셋 후에도 유지)
rtc = RTC()
SLEEP_SECONDS = 10
WAKEUP_COUNT = 3


def setup():
    """웨이크 원인과 RTC 메모리 확인"""
    if machine.reset_cause() == machine.DEEPSLEEP_RESET:
        print("원인: 딥 슬립에서 깨어남")
        count = rtc.memory()[0] if rtc.memory() else 0
        count = count + 1
        rtc.memory(bytes([count]))
        print(f"누적 웨이크 횟수: {count}")
        return count
    else:
        print("원인: 콜드 부트")
        rtc.memory(bytes([0]))
        return 0


def measure_battery():
    """ADC로 배터리 전압 측정 (배터리 핀 어댑터가 있을 때)"""
    adc = machine.ADC(Pin(35))            # 배터리 핀
    adc.atten(machine.ADC.ATTN_11DB)      # 0~3.3V 입력 범위
    adc_val = adc.read()
    voltage = adc_val * 3.3 / 4095 * 2    # 분압기 배율 2배 가정
    print(f"배터리 전압: {voltage:.2f}V")
    return voltage


def do_work():
    """각 웨이크에서 수행할 작업 (저전력 유지가 핵심)"""
    print("작업 수행 중...")
    led = Pin(2, Pin.OUT)
    led.value(1)                          # LED도 짧게만 켜기
    time.sleep_ms(100)
    led.value(0)
    print("작업 완료")


def enter_deep_sleep(seconds):
    """주변장치를 끄고 딥 슬립 진입"""
    print(f"{seconds}초 후 딥 슬립 진입")
    # Wi-Fi/BT 끄기 (쓰지 않았지만 확실히)
    try:
        import network
        wlan = network.WLAN(network.STA_IF)
        wlan.active(False)
        if hasattr(network, "bluetooth"):
            pass
    except Exception:
        pass

    # 타이머 웨이크 설정 후 딥 슬립
    machine.deepsleep(seconds * 1000)
    # 여기 아래 코드는 슬립 진입 전에 실행되지 않음


def main():
    # 주기적 웨이크 루프
    wakes = setup()
    measure_battery()
    do_work()

    if wakes >= WAKEUP_COUNT:
        print(f"{WAKEUP_COUNT}회 완료 — 시스템 종료 (웨이크 중지)")
        return

    enter_deep_sleep(SLEEP_SECONDS)
    # 딥 슬립에서 깨어나면 main()이 다시 실행됨


if __name__ == "__main__":
    main()
