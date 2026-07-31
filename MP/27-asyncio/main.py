# 27: uasyncio — 비동기 태스크, 이벤트 루프
# 대상: ESP32 (LED=GPIO2, 버튼=GPIO0), 어떤 보드에서도 동작
import uasyncio as asyncio
import time
from machine import Pin

led = Pin(2, Pin.OUT)
button = Pin(0, Pin.IN, Pin.PULL_UP)

# --- 태스크 1: LED 점멸 -------------------------------------------------
async def blink_task():
    """0.3초 주기로 LED 깜빡임 (대기 동안 CPU 양보)"""
    print("blink 태스크 시작")
    while True:
        led.value(not led.value())
        await asyncio.sleep(0.3)


# --- 태스크 2: 버튼 감시 ---------------------------------------------------
async def button_task():
    """버튼을 눌렀을 때 이벤트를 설정 (폴링을 비동기로)"""
    print("버튼 태스크 시작")
    while True:
        if button.value() == 0:          # ESP32 버튼: 눌리면 LOW
            event.set()                  # 다른 태스크에 신호
            print("버튼 누름 → 이벤트 발생")
            await asyncio.sleep(1.0)     # 중복 인식 방지
        await asyncio.sleep(0.05)


# --- 태스크 3: 이벤트 대기 ---------------------------------------------------
async def event_waiter():
    """버튼 이벤트를 기다렸다가 반응 (3초 동안 3번)"""
    count = 0
    while count < 3:
        await event.wait()               # 이벤트 발생까지 블로킹하지 않고 대기
        event.clear()                    # 이벤트 초기화
        count += 1
        print(f"[이벤트] {count}번째 눌림 — 2초간 고속 점멸")
        for _ in range(10):
            led.value(not led.value())
            await asyncio.sleep(0.1)
    print("3회 완료 — 이벤트 대기 종료")


# --- 태스크 4: 시간 제한 -----------------------------------------------------
async def timeout_task():
    """5초 후 실행되는 타임아웃 처리"""
    try:
        await asyncio.sleep(5)
        print("타임아웃: 5초 경과 (취소 시도)")
    except asyncio.CancelledError:
        print("타임아웃 태스크 취소됨")


async def main():
    global event
    event = asyncio.Event()

    print("=== uasyncio 이벤트 루프 시작 ===")
    t_blink = asyncio.create_task(blink_task())
    t_button = asyncio.create_task(button_task())
    t_timeout = asyncio.create_task(timeout_task())

    # 이벤트 대기는 main 안에서 직접
    count = 0
    while count < 3:
        await event.wait()
        event.clear()
        count += 1
        print(f"[main] 이벤트 {count}회 수신")

    # 타임아웃 태스크 취소
    t_timeout.cancel()
    try:
        await t_timeout
    except asyncio.CancelledError:
        pass

    print("=== 3초 후 종료 ===")
    t_blink.cancel()
    t_button.cancel()
    print("완료")


if __name__ == "__main__":
    try:
        asyncio.run(main())
    except KeyboardInterrupt:
        print("중단됨")
