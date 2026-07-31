"""
29: 디스크립터 — __get__/__set__/__delete__, property 내부 동작
"""
import weakref


# 1) 검증 디스크립터: 데이터 디스크립터 예시
class Positive:
    """0 이상의 값만 허용하는 디스크립터"""
    def __init__(self, default=0):
        self.default = default
        self.values = weakref.WeakKeyDictionary()  # 인스턴스별 저장

    def __get__(self, instance, owner):
        if instance is None:
            return self
        return self.values.get(instance, self.default)

    def __set__(self, instance, value):
        if value < 0:
            raise ValueError(f"음수는 허용되지 않습니다: {value}")
        self.values[instance] = value

    def __delete__(self, instance):
        del self.values[instance]


class Item:
    price = Positive(100)

    def __init__(self, name, price):
        self.name = name
        self.price = price


# 2) 비데이터 디스크립터: 인스턴스 __dict__가 우선
class LazyProp:
    """접근 시 계산하고 결과를 인스턴스에 캐시하는 디스크립터"""
    def __init__(self, func):
        self.func = func

    def __get__(self, instance, owner):
        if instance is None:
            return self
        result = self.func(instance)
        setattr(instance, self.func.__name__, result)  # 인스턴스 __dict__에 저장
        return result


class Rectangle:
    def __init__(self, w, h):
        self.w = w
        self.h = h

    @LazyProp
    def area(self):
        print("  (area 계산 실행)")
        return self.w * self.h


# 3) property 내부 동작 재현
class CachedProperty:
    """property처럼 __get__/__set__을 구현한 디스크립터"""
    def __init__(self, getter, setter=None):
        self.getter = getter
        self.setter = setter
        self._cache = weakref.WeakKeyDictionary()

    def __get__(self, instance, owner):
        if instance is None:
            return self
        if instance not in self._cache:
            self._cache[instance] = self.getter(instance)
        return self._cache[instance]

    def __set__(self, instance, value):
        if self.setter is None:
            raise AttributeError("read-only 속성")
        self.setter(instance, value)
        self._cache[instance] = value


class Circle:
    def __init__(self, radius):
        self._radius = radius

    @CachedProperty
    def area(self):
        print("  (원 넓이 계산)")
        return 3.14159 * self._radius ** 2


if __name__ == "__main__":
    print("=== 1) 검증 디스크립터 ===")
    a = Item("apple", 1500)
    b = Item("banana", 800)
    print(f"a.price = {a.price}, b.price = {b.price}")
    print(f"기본값(생성자 미지정): {Item.price.__get__(None, Item)}")
    try:
        a.price = -5
    except ValueError as e:
        print("음수 설정 시도 ->", e)
    print()

    print("=== 2) 비데이터 디스크립터 (lazy) ===")
    r = Rectangle(3, 4)
    print(f"area: {r.area}")
    print(f"area (캐시됨): {r.area}")
    print(f"인스턴스 __dict__에 area 저장됨: {'area' in r.__dict__}")
    print()

    print("=== 3) property 내부 동작 재현 ===")
    c = Circle(5)
    print(f"면적: {c.area}")
    print(f"면적 (캐시됨): {c.area}")
    try:
        c.area = 999  # setter 없음 -> 읽기 전용
    except AttributeError as e:
        print("쓰기 시도 ->", e)

    print()
    print("=== 프로퍼티 접근 순서 확인 ===")
    print("데이터 디스크립터(Item.price)는 인스턴스 __dict__보다 우선합니다")
    print("비데이터 디스크립터는 인스턴스 __dict__가 우선합니다")
