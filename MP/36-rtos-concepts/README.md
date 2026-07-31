# 36: RTOS 개념 — Tasks, Queues, Semaphores (uasyncio 관점)

## 개요

실제 펌웨어는 FreeRTOS 같은 **RTOS(실시간 운영체제)** 위에서 동작하며, 그 핵심 요소는 **태스크, 큐, 세마포어**입니다. MicroPython의 ESP32 포트는 내부적으로 FreeRTOS 위에서 동작하지만, Python에서는 `uasyncio`로 같은 개념을 직접 다루는 편이 쉽습니다. 이번 레슨에서는 RTOS의 세 핵심 개념을 uasyncio로 재현해 이해합니다.

## 태스크 (Task)

독립적으로 실행되는 작업 단위입니다. FreeRTOS는 **선점형(preemptive)** 스케줄링으로 타임슬라이스를 나눠 실행하고, uasyncio는 **협력형(cooperative)** 으로 `await` 지점에서 양보합니다.

```python
async def sensor_task():
    while True:
        read_sensor()
        await asyncio.sleep(1)

async def display_task():
    while True:
        update_display()
        await asyncio.sleep(0.5)

asyncio.create_task(sensor_task())
asyncio.create_task(display_task())
```

## 큐 (Queue)

태스크 간 데이터 전달 통로입니다. **생산자**가 넣고 **소비자**가 꺼냅니다.

```python
queue = asyncio.Queue(maxsize=10)

# 생산자
await queue.put(data)
# 소비자
item = await queue.get()
```

큐가 가득 차면 `put`이, 비면 `get`이 대기합니다. 센서 수집과 전송/표시 사이의 버퍼 역할을 합니다.

## 세마포어 (Semaphore)

공유 자원(LED, UART, 센서 버스)에 **동시 접근을 제한**합니다. 이진 세마포어는 자원 하나를 보호합니다.

```python
semaphore = asyncio.Semaphore(1)

async def use_shared():
    async with semaphore:     # 획득 → 임계 구역 → 해제
        led.toggle()
        await asyncio.sleep(0.5)
```

- **뮤텍스**: 소유자가 해제하는 특수 세마포어
- **바이너리 세마포어**: 0/1 값으로 신호 전달에도 사용
- **카운팅 세마포어**: 여러 개의 동일 자원(N개) 관리

## 이벤트와 우선순위

- `asyncio.Event`: 신호가 올 때까지 대기 → 이벤트 기반 설계의 기본
- 우선순위 스케줄링은 실제 RTOS의 영역이며, uasyncio에서는 태스크 수를 줄이고 대기 시간을 짧게 하는 것이 핵심
- **우선순위 반전**: 낮은 우선순위 태스크가 세마포어를 쥐고 있으면 높은 태스크가 막히는 문제 — 임계 구역을 짧게 유지해 완화

## 실행/업로드 방법

1. **Thonny IDE**: `MP/36-rtos-concepts/main.py`를 열어 실행(F5).
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/36-rtos-concepts/main.py
   ampy --port COM3 run MP/36-rtos-concepts/main.py
   ```
3. 시리얼 로그에서 큐 통신, 세마포어 임계 구역, 이벤트 신호 흐름을 확인합니다.

## 핵심 개념 요약

- 태스크: 독립 작업 단위 (선점형 vs 협력형)
- 큐: 생산자-소비자 데이터 전달
- 세마포어: 공유 자원 동시 접근 제한 (임계 구역)
- 이벤트: 태스크 간 신호 전달
