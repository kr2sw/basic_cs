"""
23: 컨텍스트 매니저 — __enter__/__exit__, @contextmanager, ExitStack, async with
"""
import asyncio
import contextlib
import time
from contextlib import contextmanager, ExitStack


# 1) 클래스 기반 컨텍스트 매니저: 임계 구역(락) 시뮬레이션
class Guard:
    """with 진입 시 enter, 종료 시 exit를 보장합니다."""
    def __init__(self, name):
        self.name = name

    def __enter__(self):
        print(f"[{self.name}] 진입 (acquire)")
        return self  # as로 받을 값

    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type:
            print(f"[{self.name}] 예외 발생: {exc_val}")
        print(f"[{self.name}] 종료 (release)")
        return False  # False -> 예외를 밖으로 전파


# 2) @contextmanager: yield 하나로 간단하게
@contextmanager
def measure(name):
    start = time.perf_counter()
    print(f"[{name}] 시작")
    try:
        yield
    finally:
        elapsed = time.perf_counter() - start
        print(f"[{name}] {elapsed:.4f}초 소요")


# 3) ExitStack: 여러 컨텍스트를 동시에 관리
@contextmanager
def tag(name):
    print(f"  <{name}> 진입")
    yield
    print(f"  </{name}> 종료")


# 4) 비동기 컨텍스트 매니저
class AsyncResource:
    def __init__(self, name):
        self.name = name

    async def __aenter__(self):
        print(f"[async:{self.name}] 연결 수립")
        return self

    async def __aexit__(self, exc_type, exc_val, exc_tb):
        print(f"[async:{self.name}] 연결 해제")


async def use_async_resource():
    async with AsyncResource("redis") as res:
        print(f"[async:{res.name}] 작업 중...")
        await asyncio.sleep(0.2)


if __name__ == "__main__":
    print("=== 1) 클래스 기반 with ===")
    with Guard("lock1") as g:
        print(f"as로 받은 값: {g.name}")
    try:
        with Guard("lock2") as g:
            raise ValueError("뭔가 잘못됨")
    except ValueError as e:
        print(f"외부에서 잡은 예외: {e}")
    print()

    print("=== 2) @contextmanager ===")
    with measure("덧셈 루프"):
        time.sleep(0.1)
        total = sum(range(100_000))
        print(f"  sum = {total}")
    print()

    print("=== 3) ExitStack (중첩/동적) ===")
    with ExitStack() as stack:
        stack.enter_context(tag("div"))
        stack.enter_context(tag("p"))
        print("  본문 내용")
        print("  ExitStack을 벗어나면 역순으로 정리됩니다")
    print()

    print("=== 4) async with ===")
    asyncio.run(use_async_resource())

    print()
    print("=== 5) contextlib 기타 유틸 ===")
    with contextlib.nullcontext("noop") as n:
        print(f"nullcontext 값: {n}  (아무 작업도 하지 않음)")
    with contextlib.suppress(ZeroDivisionError):
        result = 1 / 0
        print("이 줄은 실행되지 않음")
    print("suppress: 지정한 예외를 조용히 무시합니다")
