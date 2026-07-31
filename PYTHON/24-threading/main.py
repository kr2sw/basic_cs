"""
24: 스레드와 GIL — Thread, Lock, Queue, ThreadPoolExecutor
"""
import threading
import time
from queue import Queue
from concurrent.futures import ThreadPoolExecutor

LOCK_ITERATIONS = 200_000

# 1) Lock 없이 공유 변수 증가 -> 경쟁 상태
counter_no_lock = 0


def increment_no_lock():
    global counter_no_lock
    for _ in range(LOCK_ITERATIONS):
        counter_no_lock += 1


# 2) Lock으로 보호한 공유 변수 증가
counter_lock = 0
lock = threading.Lock()


def increment_with_lock():
    global counter_lock
    for _ in range(LOCK_ITERATIONS):
        with lock:  # 임계 구역
            counter_lock += 1


# 3) Queue 기반 생산자-소비자
def producer(q, items, producer_id):
    for item in items:
        q.put((producer_id, item))
        time.sleep(0.001)
    q.put(None)  # 종료 신호


def consumer(q, results, consumer_id):
    while True:
        item = q.get()
        if item is None:
            q.put(None)  # 다른 소비자를 위해 신호 재전달
            break
        producer_id, value = item
        time.sleep(0.002)
        results.append(f"C{consumer_id} <- P{producer_id}: {value}")


# 4) I/O 바운드 작업 -> 스레드가 효과적
def io_work(n):
    time.sleep(0.05)  # 네트워크/파일 I/O 대기 흉내
    return n * 10


if __name__ == "__main__":
    print("=== 1) Lock 없이 증가 (경쟁 상태) ===")
    threads = [threading.Thread(target=increment_no_lock) for _ in range(4)]
    for t in threads:
        t.start()
    for t in threads:
        t.join()
    print(f"결과: {counter_no_lock:,}  (기대값: {LOCK_ITERATIONS * 4:,})")
    print("  -> 값이 틀립니다. 여러 스레드가 동시에 덮어쓰기 때문 (GIL 타이밍에도 발생)")
    print()

    print("=== 2) Lock으로 보호 ===")
    threads = [threading.Thread(target=increment_with_lock) for _ in range(4)]
    for t in threads:
        t.start()
    for t in threads:
        t.join()
    print(f"결과: {counter_lock:,}  (기대값과 일치)")
    print()

    print("=== 3) Queue 생산자-소비자 ===")
    q = Queue()
    results = []
    threads = [
        threading.Thread(target=producer, args=(q, range(5), 1)),
        threading.Thread(target=producer, args=(q, range(5, 10), 2)),
        threading.Thread(target=consumer, args=(q, results, 1)),
        threading.Thread(target=consumer, args=(q, results, 2)),
    ]
    for t in threads:
        t.start()
    for t in threads:
        t.join()
    print(f"총 처리된 작업: {len(results)}개")
    for r in results[:5]:
        print("  ", r)
    print()

    print("=== 4) ThreadPoolExecutor (I/O 바운드) ===")
    start = time.perf_counter()
    with ThreadPoolExecutor(max_workers=4) as pool:
        values = list(pool.map(io_work, range(8)))
    elapsed = time.perf_counter() - start
    print(f"결과: {values}")
    print(f"소요 시간: {elapsed:.3f}초 (순차 처리였다면 약 0.4초)")
