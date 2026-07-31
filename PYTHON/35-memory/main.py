"""
35: 메모리 관리 — 참조 카운트, gc, weakref, __slots__, 순환 참조
"""
import gc
import sys
import weakref

# 1) 참조 카운트 확인
def refcount_demo():
    print("=== 1) sys.getrefcount ===")
    obj = []
    print(f"  생성 직후 refcount: {sys.getrefcount(obj) - 1}")
    ref1 = obj
    ref2 = obj
    print(f"  ref1/ref2 추가 후: {sys.getrefcount(obj) - 1}")
    del ref1, ref2
    print(f"  삭제 후: {sys.getrefcount(obj) - 1}")
    print()


# 2) 순환 참조와 gc
class Node:
    def __init__(self, name):
        self.name = name
        self.partner = None

    def __del__(self):
        print(f"  [삭제] {self.name}")


def circular_demo():
    print("=== 2) 순환 참조와 gc ===")
    a = Node("A")
    b = Node("B")
    a.partner = b
    b.partner = a  # 서로를 참조 -> 순환
    print(f"  gc가 추적하는 객체 수: {len(gc.get_objects())}")
    del a, b
    print("  a, b 참조 제거 (레퍼런스 카운트는 0이 안 됨)")
    print(f"  gc.collect() 수거 객체: {gc.collect()}개")
    print()


# 3) weakref
def weakref_demo():
    print("=== 3) weakref ===")
    class Cache:
        def __init__(self):
            self.data = {}

        def __del__(self):
            print("  [캐시 삭제됨]")

    cache = Cache()
    ref = weakref.ref(cache)
    print(f"  weakref 살아있음: {ref() is not None}")
    del cache
    print(f"  대상 삭제 후 weakref: {ref()}")

    # WeakValueDictionary: 값이 살아있는 동안만 캐시 유지
    data = {"a": 1}
    wdict = weakref.WeakValueDictionary()
    wdict["key"] = data
    print(f"  WeakValueDictionary 값 존재: {'key' in wdict}")
    del data
    print(f"  값 삭제 후: {'key' in wdict}")
    print()


# 4) __slots__ 메모리 절약 비교
class Slotted:
    __slots__ = ("x", "y")

    def __init__(self, x, y):
        self.x = x
        self.y = y


class Regular:
    def __init__(self, x, y):
        self.x = x
        self.y = y


def slots_demo():
    print("=== 4) __slots__ 메모리 비교 ===")
    import sys
    regular = Regular(1, 2)
    slotted = Slotted(1, 2)
    r_size = sys.getsizeof(regular) + sys.getsizeof(regular.__dict__)
    s_size = sys.getsizeof(slotted)
    print(f"  Regular 인스턴스 크기: {r_size} bytes (__dict__ 포함)")
    print(f"  Slotted 인스턴스 크기: {s_size} bytes")
    print(f"  절약: {r_size - s_size} bytes/인스턴스")
    print()


# 5) gc로 관리되는 순환 참조를 발견하기
def gc_demo():
    print("=== 5) gc 설정과 수동 수거 ===")
    gc.set_debug(gc.DEBUG_STATS)
    gc.collect()
    gc.set_debug(0)
    print("  DEBUG_STATS로 수거 로그를 볼 수 있습니다 (지금은 해제)")
    print()


if __name__ == "__main__":
    refcount_demo()
    circular_demo()
    weakref_demo()
    slots_demo()
    gc_demo()
