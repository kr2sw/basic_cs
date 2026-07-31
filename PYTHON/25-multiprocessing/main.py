"""
25: 멀티프로세싱 — Process, Pool, ProcessPoolExecutor, Queue/Pipe, 공유 메모리
Windows는 spawn 방식이므로 반드시 if __name__ == "__main__": 가드가 필요합니다.
"""
import multiprocessing as mp
import os
import time
from concurrent.futures import ProcessPoolExecutor


def cpu_work(n):
    """CPU 바운드 작업: n번 루프 합계"""
    total = 0
    for i in range(n):
        total += i ** 2
    return total


def worker_process(pid_name, result_queue):
    """자식 프로세스에서 실행되는 함수"""
    result = cpu_work(2_000_000)
    result_queue.put((pid_name, os.getpid(), result))


def pipe_send(conn):
    conn.send("hello from child")
    conn.close()


def add_value(counter, lk, count):
    """공유 메모리에 값을 누적합니다. 공유 객체는 인자로 전달받아야 합니다."""
    for _ in range(count):
        with lk:
            counter.value += 1


if __name__ == "__main__":
    print(f"부모 프로세스 PID: {os.getpid()}")
    print()

    print("=== 1) Process + Queue (결과 회수) ===")
    q = mp.Queue()
    procs = [mp.Process(target=worker_process, args=(f"worker{i}", q)) for i in range(3)]
    for p in procs:
        p.start()
    results = [q.get() for _ in procs]
    for p in procs:
        p.join()
    for name, pid, value in results:
        print(f"  {name}: PID={pid}, 결과={value:,}")

    print()
    print("=== 2) Pipe (2자 간 통신) ===")
    parent_conn, child_conn = mp.Pipe()
    p = mp.Process(target=pipe_send, args=(child_conn,))
    p.start()
    print("  부모가 받은 메시지:", parent_conn.recv())
    p.join()

    print()
    print("=== 3) Pool.map (작업 분배) ===")
    with mp.Pool(processes=2) as pool:
        values = pool.map(cpu_work, [200_000, 300_000, 400_000, 500_000])
    print("  결과:", [f"{v:,}" for v in values])

    print()
    print("=== 4) ProcessPoolExecutor ===")
    with ProcessPoolExecutor(max_workers=3) as pool:
        futures = [pool.submit(cpu_work, 500_000) for _ in range(6)]
        for f in futures:
            print(f"  future 결과: {f.result():,}")

    print()
    print("=== 5) 공유 메모리 Value / Array ===")
    shared_counter = mp.Value("i", 0)
    lock = mp.Lock()

    procs = [mp.Process(target=add_value, args=(shared_counter, lock, 50_000))
             for _ in range(4)]
    for p in procs:
        p.start()
    for p in procs:
        p.join()
    print(f"  공유 카운터: {shared_counter.value:,} (기대값: 200,000)")

    print()
    print("=== 성능 비교 (순차 vs 멀티프로세스) ===")
    seq_start = time.perf_counter()
    seq_result = [cpu_work(2_000_000) for _ in range(4)]
    seq_time = time.perf_counter() - seq_start

    par_start = time.perf_counter()
    with mp.Pool(processes=4) as pool:
        par_result = pool.map(cpu_work, [2_000_000] * 4)
    par_time = time.perf_counter() - par_start
    print(f"  순차: {seq_time:.2f}초 / 병렬: {par_time:.2f}초 / 가속: {seq_time/par_time:.2f}x")
    print(f"  결과 일치: {seq_result == par_result}")
