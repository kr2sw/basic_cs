# 26: 비동기 프로그래밍 (Asyncio) — async/await, Task, gather, asyncio.run

## async/await
`async def`는 코루틴(coroutine)을 만듭니다. `await`는 I/O가 끝날 때까지 실행을 양보(yield)합니다. 스레드 없이 이벤트 루프 하나로 동시성을 구현합니다.

```python
import asyncio

async def main():
    await asyncio.sleep(1)

asyncio.run(main())
```

## `asyncio.run`
이벤트 루프를 생성/실행/정리까지 한 번에 처리하는 진입점입니다.

## Task
`asyncio.create_task()`로 코루틴을 백그라운드 작업으로 올립니다. 여러 작업이 동시에 진행됩니다.

## `asyncio.gather`
여러 코루틴/태스크를 동시에 실행하고 결과를 모아 반환합니다.

## 동시성 vs 병렬성
asyncio는 하나의 스레드에서 논블로킹 I/O를 번갈아 실행하는 **동시성**입니다. CPU 계산은 블로킹하므로 `asyncio.to_thread()` 등을 활용합니다.

## 실행

```bash
python main.py
```
