"""
26: 비동기 프로그래밍 — async/await, create_task, gather, semaphore
"""
import asyncio
import time


async def fetch_url(name, delay):
    """네트워크/DB 호출처럼 대기가 오래 걸리는 작업을 흉내 냅니다."""
    print(f"  [{name}] 요청 시작 (지연 {delay}s)")
    await asyncio.sleep(delay)
    print(f"  [{name}] 응답 완료")
    return f"{name} 데이터"


async def sequential():
    print("--- 순차 실행 (총 3초) ---")
    start = time.perf_counter()
    a = await fetch_url("A", 1)
    b = await fetch_url("B", 1)
    c = await fetch_url("C", 1)
    elapsed = time.perf_counter() - start
    print(f"결과: {a}, {b}, {c}  / 소요 {elapsed:.2f}s\n")
    return elapsed


async def with_gather():
    print("--- gather 동시 실행 (총 1초) ---")
    start = time.perf_counter()
    results = await asyncio.gather(
        fetch_url("A", 1),
        fetch_url("B", 1),
        fetch_url("C", 1),
    )
    elapsed = time.perf_counter() - start
    print(f"결과: {results}  / 소요 {elapsed:.2f}s\n")
    return elapsed


async def with_tasks():
    print("--- create_task: 요청을 한꺼번에 던지고 나중에 회수 ---")
    start = time.perf_counter()
    task_a = asyncio.create_task(fetch_url("A", 1))
    task_b = asyncio.create_task(fetch_url("B", 1))
    print("  (백그라운드로 두 작업 실행 중...)")
    result_a = await task_a
    result_b = await task_b
    elapsed = time.perf_counter() - start
    print(f"결과: {result_a}, {result_b}  / 소요 {elapsed:.2f}s\n")
    return elapsed


async def with_semaphore():
    print("--- Semaphore로 동시성 제한 (최대 2개씩) ---")
    sem = asyncio.Semaphore(2)

    async def limited(name):
        async with sem:
            return await fetch_url(name, 1)

    start = time.perf_counter()
    results = await asyncio.gather(*(limited(n) for n in "ABCD"))
    elapsed = time.perf_counter() - start
    print(f"결과: {results}  / 소요 {elapsed:.2f}s (2배로 나누어 실행)\n")
    return elapsed


async def wait_with_timeout():
    print("--- timeout 처리 ---")
    try:
        await asyncio.wait_for(fetch_url("SLOW", 5), timeout=2)
    except asyncio.TimeoutError:
        print("  SLOW 요청이 2초 안에 끝나지 않아 TimeoutError 발생\n")


async def main():
    await sequential()
    await with_gather()
    await with_tasks()
    await with_semaphore()
    await wait_with_timeout()


if __name__ == "__main__":
    asyncio.run(main())
