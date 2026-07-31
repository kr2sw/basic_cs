"""
30: 함수형 프로그래밍 — itertools, functools.partial, 커링, reduce
"""
import functools
import itertools
from operator import mul


# 1) itertools 기본
def itertools_demo():
    print("=== 1) itertools ===")
    print("chain('AB', [1,2]):", list(itertools.chain("AB", [1, 2])))
    print("product([1,2], 'ab'):", list(itertools.product([1, 2], "ab")))
    print("permutations('ABC', 2):", list(itertools.permutations("ABC", 2)))
    print("combinations('ABC', 2):", list(itertools.combinations("ABC", 2)))
    print("islice(range(100), 5):", list(itertools.islice(range(100), 5)))
    print("takewhile(lambda x: x<5, [1,3,4,7,2]):",
          list(itertools.takewhile(lambda x: x < 5, [1, 3, 4, 7, 2])))
    print()


# 2) groupby: 정렬된 데이터를 키로 묶기
def groupby_demo():
    print("=== 2) groupby (연속된 값 묶기) ===")
    words = sorted(["banana", "apple", "apricot", "cherry", "blueberry"])
    for key, group in itertools.groupby(words, key=lambda w: w[0]):
        print(f"  {key}: {list(group)}")
    print()


# 3) functools.partial
def power(base, exponent):
    return base ** exponent


def partial_demo():
    print("=== 3) functools.partial ===")
    square = functools.partial(power, exponent=2)
    cube = functools.partial(power, exponent=3)
    print("square(5):", square(5))
    print("cube(3):", cube(3))
    print()


# 4) 커링 구현
def curry(func):
    """f(a, b, c) -> f(a)(b)(c) 형태로 바꾸는 커링 데코레이터"""
    def curried(*args, **kwargs):
        if len(args) + len(kwargs) >= func.__code__.co_argcount:
            return func(*args, **kwargs)
        return lambda *more: curried(*(args + more), **kwargs)
    return curried


@curry
def add3(a, b, c):
    return a + b + c


def curry_demo():
    print("=== 4) 커링 ===")
    print("add3(1)(2)(3):", add3(1)(2)(3))
    add_to_5 = add3(0)(5)  # 첫 두 인자 고정
    print("add3(0)(5)(10):", add_to_5(10))
    print()


# 5) reduce + partial 조합
def reduce_demo():
    print("=== 5) reduce ===")
    total = functools.reduce(mul, range(1, 6), 1)  # 1*2*3*4*5
    print("reduce(mul, 1..5):", total)
    add = functools.partial(functools.reduce, lambda a, b: a + b)
    print("reduce 합계 [1..5]:", add(range(1, 6)))
    print()


# 6) 파이프라인: 지연 이터레이터 체이닝
def pipeline_demo():
    print("=== 6) 지연 파이프라인 ===")
    numbers = range(1, 101)
    pipeline = itertools.islice(
        itertools.takewhile(
            lambda n: n < 100,
            (x ** 2 for x in numbers if x % 3 == 0),
        ),
        6,
    )
    print("3의 배수의 제곱 중 100 미만 (처음 6개):", list(pipeline))
    print()


if __name__ == "__main__":
    itertools_demo()
    groupby_demo()
    partial_demo()
    curry_demo()
    reduce_demo()
    pipeline_demo()
