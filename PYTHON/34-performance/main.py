"""
34: 성능 최적화 — timeit, cProfile, lru_cache, 코드 최적화 기법
"""
import cProfile
import functools
import timeit

# 1) timeit: 짧은 코드 조각의 실행 시간 측정
def timeit_demo():
    print("=== 1) timeit (1만 회 반복) ===")
    t1 = timeit.timeit("sum(range(1000))", number=10_000)
    t2 = timeit.timeit("[x for x in range(1000) if x % 2 == 0]", number=10_000)
    print(f"  sum(range(1000)):          {t1:.4f}s")
    print(f"  list comprehension:        {t2:.4f}s")
    print()


# 2) lru_cache: 피보나치 비교 (캐시 유/무)
def fib_plain(n):
    return n if n < 2 else fib_plain(n - 1) + fib_plain(n - 2)


@functools.lru_cache(maxsize=None)
def fib_cached(n):
    return n if n < 2 else fib_cached(n - 1) + fib_cached(n - 2)


def lru_demo():
    print("=== 2) lru_cache (피보나치 n=30) ===")
    plain_time = timeit.timeit("fib_plain(30)", globals=globals(), number=3)
    cached_time = timeit.timeit("fib_cached(30)", globals=globals(), number=3)
    print(f"  캐시 없음: {plain_time:.3f}s")
    print(f"  lru_cache: {cached_time:.5f}s  (약 {plain_time/max(cached_time,1e-9):.0f}배 빠름)")
    print(f"  캐시 정보: {fib_cached.cache_info()}")
    print()


# 3) 최적화 기법 비교
def membership_demo():
    print("=== 3) 멤버십 검사: 리스트 vs 세트 ===")
    data_list = list(range(10_000))
    data_set = set(data_list)
    targets = [i * 2 for i in range(500)]

    t_list = timeit.timeit(
        lambda: sum(1 for t in targets if t in data_list), number=100)
    t_set = timeit.timeit(
        lambda: sum(1 for t in targets if t in data_set), number=100)
    print(f"  리스트 in: {t_list:.4f}s")
    print(f"  세트 in:   {t_set:.4f}s")
    print()


def local_var_demo():
    print("=== 4) 전역 vs 지역 변수 ===")
    def use_global():
        return sum(range(1000))

    def use_local():
        rng = range
        return sum(rng(1000))

    t_global = timeit.timeit(use_global, number=100_000)
    t_local = timeit.timeit(use_local, number=100_000)
    print(f"  전역 참조: {t_global:.3f}s")
    print(f"  지역 바인딩: {t_local:.3f}s")
    print()


# 4) cProfile: 함수 단위 병목 분석
def process():
    total = 0
    for i in range(1000):
        total += sum(range(i))
    for s in ["a", "b", "c"]:
        total += len(s) * 10
    return total


def profile_demo():
    print("=== 5) cProfile (호출 프로파일) ===")
    profiler = cProfile.Profile()
    profiler.enable()
    process()
    profiler.disable()
    import io
    import pstats
    stream = io.StringIO()
    stats = pstats.Stats(profiler, stream=stream).sort_stats("cumulative")
    stats.print_stats(5)
    print(stream.getvalue())
    print()


if __name__ == "__main__":
    timeit_demo()
    lru_demo()
    membership_demo()
    local_var_demo()
    profile_demo()
