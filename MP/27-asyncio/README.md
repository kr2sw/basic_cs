# 27: uasyncio — Async Tasks, Event Loop

## 개요

마이크로컨트롤러는 코어가 하나뿐이지만, **uasyncio**로 여러 일을 "겉보기에 동시에" 처리할 수 있습니다. 이벤트 루프가 태스크들을 스케줄링하고, `await` 지점에서 대기 중인 작업 사이를 오가며 CPU를 효율적으로 사용합니다.

## 기존 코드의 문제

```python
# 동기 코드: 버튼을 확인하는 동안 온도 표시가 멈춘다
while True:
    time.sleep(2)          # 2초 동안 아무것도 못 함
    display.update()
    if button.is_pressed():
        handle()
```

`time.sleep()` 동안 CPU가 놀고, 다른 작업이 블로킹됩니다.

## 비동기 태스크로 전환

```python
import uasyncio as asyncio

async def blink_led():
    while True:
        led.toggle()
        await asyncio.sleep(0.5)   # 대기하는 동안 CPU 양보

async def read_sensor():
    while True:
        print(sensor.read())
        await asyncio.sleep(1)

async def main():
    asyncio.create_task(blink_led())   # 백그라운드 태스크 등록
    asyncio.create_task(read_sensor())
    await asyncio.sleep(30)            # 30초 동안 두 태스크가 번갈아 실행
```

`await asyncio.sleep()`에서 "지금은 대기 중"을 알리고 다른 태스크가 실행됩니다. 이것이 **협력적 멀티태스킹**입니다.

## 주요 API

- `asyncio.create_task()` — 백그라운드 태스크 등록
- `asyncio.sleep()` — 비블로킹 대기
- `asyncio.gather()` — 여러 태스크 완료 대기
- `task.cancel()` / `asyncio.CancelledError` — 태스크 취소
- `asyncio.Event` / `asyncio.Queue` — 태스크 간 통신

```python
results = await asyncio.gather(read_a(), read_b(), read_c())
```

## 실행/업로드 방법

1. **Thonny IDE**: `MP/27-asyncio/main.py`를 열어 실행(F5)하면, LED 깜빡임과 온도 표시가 동시에 동작하는 것을 확인합니다.
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/27-asyncio/main.py
   ampy --port COM3 run MP/27-asyncio/main.py
   ```
3. 버튼을 누르면 이벤트로 상태를 바꾸는 태스크가 즉시 반응합니다.

## 핵심 개념 요약

- `async def`로 태스크 정의, `await`로 대기 중 양보
- 이벤트 루프가 스케줄링 → 하나의 CPU로 동시성 구현
- `create_task`로 백그라운드 실행, `Event`/`Queue`로 태스크 간 통신
