# 25: 멀티프로세싱 (Multiprocessing) — Process, Pool, ProcessPoolExecutor, IPC

## 왜 필요할까?
GIL 때문에 CPU 바운드 계산은 스레드로 병렬화되지 않습니다. 멀티프로세싱은 별도의 프로세스를 만들어 각자의 GIL을 가지므로 CPU 코어를 실제로 병렬 활용합니다.

```python
import multiprocessing as mp
p = mp.Process(target=work, args=(x,))
p.start()
p.join()
```

## `Pool`
여러 프로세스에 작업을 분배하는 풀입니다. `map`, `starmap`, `apply` 등을 지원합니다.

## `ProcessPoolExecutor`
`concurrent.futures`의 고수준 인터페이스입니다. `submit`/`map`으로 작업을 던지고 `Future`로 결과를 받습니다.

## IPC (프로세스 간 통신)
- `Queue`: 프로세스 간 안전한 데이터 전송
- `Pipe`: 두 프로세스 간 단일 연결
- 공유 메모리 `Value` / `Array`: 효율적이지만 Lock 관리 필요

## Windows 주의
Windows에서는 `spawn` 방식이라 프로세스 생성 시 `if __name__ == "__main__":` 가드가 필수입니다.

## 실행

```bash
python main.py
```
