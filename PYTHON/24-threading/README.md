# 24: 스레드와 GIL (Threads & GIL) — Thread, Lock, Queue, 동기화

## 스레드 (Thread)
`threading.Thread`로 병렬 실행 단위를 만들 수 있습니다. I/O 대기(파일, 네트워크)가 많은 작업에 효과적입니다.

```python
import threading
t = threading.Thread(target=work, args=(1,))
t.start()
t.join()
```

## GIL (Global Interpreter Lock)
CPython은 한 번에 하나의 스레드만 Python 바이트코드를 실행하도록 GIL을 유지합니다. 따라서 순수 계산(CPU 바운드) 작업은 스레드로 빨라지지 않습니다. I/O 바운드 작업은 GIL이 대기 시간 동안 풀리므로 유리합니다.

## `Lock` 동기화
여러 스레드가 같은 변수를 수정하면 경쟁 상태(race condition)가 발생할 수 있습니다. `Lock`으로 임계 구역을 보호합니다.

## `Queue`
`queue.Queue`는 스레드 간 안전한 데이터 교환을 제공합니다. 생산자-소비자 패턴에 유용합니다.

## `ThreadPoolExecutor`
`concurrent.futures`로 스레드 풀을 간편하게 사용할 수 있습니다.

## 실행

```bash
python main.py
```
