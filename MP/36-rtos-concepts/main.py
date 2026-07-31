# 36: RTOS 개념 — Tasks, Queues, Semaphores (uasyncio 관점)
# 대상: ESP32/Pico — 실제 RTOS(FreeRTOS)는 없지만 uasyncio로 개념을 재현
import uasyncio as asyncio
import time
from machine import Pin, Timer

led = Pin(2, Pin.OUT)
button = Pin(0, Pin.IN, Pin.PULL_UP)

# =====================================================================
# RTOS 핵심 개념
#  - 태스크(Task): 독립적으로 실행되는 작업 단위
#  - 큐(Queue): 태스크 간 데이터 전달 (생산자-소비자 패턴)
#  - 세마포어(Semaphore): 공유 자원 접근 제어 / 이벤트 신호
#  - 우선순위 스케줄링: FreeRTOS는 선점형, uasyncio는 협력형
# =====================================================================

# --- 1) 큐 (생산자-소비자) ----------------------------------------------------
async def producer(queue):
    """센서 값을 주기적으로 큐에 넣는 생산자 태스크"""
    count = 0
    while True:
        value = {"seq": count, "temp": 20.0 + count * 0.5}
        await queue.put(value)          # 큐에 삽입 (가득 차면 블로킹)
        count += 1
        await asyncio.sleep(1)


async def consumer(queue):
    """큐에서 값을 꺼내 처리하는 소비자 태스크"""
    while True:
        item = await queue.get()        # 큐에서 꺼냄 (비면 블로킹)
        print(f"[소비자] seq={item['seq']} temp={item['temp']:.1f}°C")
        await asyncio.sleep(2)


# --- 2) 세마포어 (공유 자원 제어) -----------------------------------------------
async def shared_resource(name, semaphore):
    """LED(공유 자원)를 세마포어로 보호하며 사용"""
    for i in range(3):
        async with semaphore:           # 획득 → 자원 사용 → 해제
            print(f"[{name}] LED 켜짐 (임계 구역 진입)")
            led.value(1)
            await asyncio.sleep(0.5)
            led.value(0)
            print(f"[{name}] LED 꺼짐 (임계 구역 퇴장)")
        await asyncio.sleep(0.5)


# --- 3) 이벤트 플래그 (우선순위 반전의 간단한 해결) --------------------------------
async def low_priority_task():
    """낮은 우선순위 태스크가 이벤트 발생 전까지 대기"""
    print("[낮음] 대기 시작")
    await event.wait()
    print("[낮음] 이벤트 수신 — 처리 시작")
    await asyncio.sleep(1)
    print("[낮음] 처리 완료")


async def high_priority_task():
    """높은 우선순위 태스크가 주기적으로 실행"""
    for i in range(5):
        print("[높음] 실행")
        await asyncio.sleep(1.2)
    event.set()                          # 이벤트 신호 발생


async def main():
    global event
    event = asyncio.Event()

    print("=== RTOS 개념 데모 (uasyncio로 재현) ===\n")

    print("--- 큐: 생산자-소비자 (5초 실행) ---")
    queue = asyncio.Queue(maxsize=5)
    t1 = asyncio.create_task(producer(queue))
    t2 = asyncio.create_task(consumer(queue))
    await asyncio.sleep(5)
    t1.cancel()
    t2.cancel()
    print()

    print("--- 세마포어: LED 공유 자원 보호 ---")
    semaphore = asyncio.Semaphore(1)     # 동시 접근 허용 1개
    tasks = [
        asyncio.create_task(shared_resource("태스크A", semaphore)),
        asyncio.create_task(shared_resource("태스크B", semaphore)),
    ]
    await asyncio.gather(*tasks)
    print()

    print("--- 이벤트: 태스크 간 신호 전달 ---")
    t3 = asyncio.create_task(low_priority_task())
    await high_priority_task()
    await t3
    print("\n완료")


if __name__ == "__main__":
    try:
        asyncio.run(main())
    except KeyboardInterrupt:
        print("중단됨")
