"""
21: 고급 OOP — dataclass, __slots__, 추상 클래스, 매직 메서드
"""
from abc import ABC, abstractmethod
from dataclasses import dataclass, field


# 1) dataclass: 반복적인 __init__/__repr__/__eq__ 자동 생성
@dataclass
class Point:
    x: float
    y: float = 0.0
    tags: list = field(default_factory=list)

    # 매직 메서드도 직접 정의 가능
    def __add__(self, other):
        return Point(self.x + other.x, self.y + other.y)

    def __len__(self):
        return 2


# 2) __slots__: 인스턴스 __dict__ 대신 고정 슬롯만 사용 -> 메모리 절약
class SlotPoint:
    __slots__ = ("x", "y")

    def __init__(self, x, y):
        self.x = x
        self.y = y


# 3) 추상 클래스: 자식이 반드시 구현해야 하는 인터페이스 강제
class Shape(ABC):
    @abstractmethod
    def area(self):
        pass

    def describe(self):
        return f"{type(self).__name__}의 넓이 = {self.area():.2f}"


class Circle(Shape):
    def __init__(self, radius):
        self.radius = radius

    def area(self):
        return 3.14159 * self.radius ** 2


# 4) 매직 메서드 조합: 은행 계좌를 파이썬 객체답게 만들기
class BankAccount:
    def __init__(self, owner, balance=0):
        self.owner = owner
        self.balance = balance
        self._history = []

    def deposit(self, amount):
        self.balance += amount
        self._history.append(("입금", amount))
        return self  # 메서드 체이닝을 위해 self 반환

    def withdraw(self, amount):
        if amount > self.balance:
            raise ValueError("잔액 부족")
        self.balance -= amount
        self._history.append(("출금", amount))
        return self

    def __repr__(self):
        return f"BankAccount(owner={self.owner!r}, balance={self.balance})"

    def __eq__(self, other):
        return isinstance(other, BankAccount) and self.owner == other.owner

    def __len__(self):
        return len(self._history)

    def __getitem__(self, index):
        return self._history[index]

    def __call__(self):
        return f"{self.owner}님의 잔액은 {self.balance:,}원입니다."


if __name__ == "__main__":
    print("=== 1) dataclass ===")
    p1 = Point(1, 2)
    p2 = Point(3, 4)
    p1.tags.append("a")
    print("p1:", p1)
    print("p1 + p2:", p1 + p2)
    print("len(p1):", len(p1))
    print("p1 == p2:", p1 == p2)
    print()

    print("=== 2) __slots__ ===")
    sp = SlotPoint(10, 20)
    print(f"SlotPoint(10,20) -> x={sp.x}, y={sp.y}")
    print(f"__slots__ 존재 여부: {'__slots__' in dir(sp)}")
    print(f"__dict__ 존재 여부: {'__dict__' in dir(sp)}")
    print()

    print("=== 3) 추상 클래스 ===")
    c = Circle(5)
    print(c.describe())
    print("isinstance(c, Shape):", isinstance(c, Shape))
    try:
        class MissingArea(Shape):  # area 미구현 -> 인스턴스 생성 불가
            pass
        MissingArea()
    except TypeError as e:
        print("area 미구현 클래스는 생성 불가:", e)
    print()

    print("=== 4) 매직 메서드 ===")
    acc = BankAccount("홍길동", 1000)
    acc.deposit(5000).withdraw(2000).deposit(1000)
    print("repr(acc):", repr(acc))
    print("acc():", acc())
    print("거래 횟수 len(acc):", len(acc))
    for i, (kind, amount) in enumerate(acc):
        print(f"  [{i}] {kind}: {amount:,}원")
    print("acc == BankAccount('홍길동', 0):", acc == BankAccount("홍길동", 0))
