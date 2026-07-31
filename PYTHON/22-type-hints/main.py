"""
22: 타입 힌트 — TypedDict, Protocol, NewType, Literal
mypy로 검사하려면: mypy main.py  (pip install mypy)
"""
from typing import List, Literal, NewType, Optional, Protocol, TypedDict


# 1) 기본 타입 힌트
def add(a: int, b: int) -> int:
    return a + b


def greet(name: str, age: Optional[int] = None) -> str:
    if age is None:
        return f"안녕하세요, {name}!"
    return f"안녕하세요, {name}! 나이는 {age}살이네요."


def total_length(values: List[str]) -> int:
    return sum(len(v) for v in values)


# 2) TypedDict: 딕셔너리 구조를 타입으로 정의
class Movie(TypedDict):
    title: str
    year: int
    rating: Optional[float]


# 3) NewType: 기본 타입과 구분되는 새 타입
UserId = NewType("UserId", int)
ProductId = NewType("ProductId", int)


def find_user(uid: UserId) -> str:
    return f"사용자 #{uid} 조회 완료"


# 4) Protocol: 구조적 타이핑 (상속 없이 덕 타이핑 검사)
class SupportsQuack(Protocol):
    def quack(self) -> str:
        ...


class Duck:
    def quack(self) -> str:
        return "꽥꽥!"


class Robot:
    def quack(self) -> str:
        return "삐익-꽥!"


def make_noise(obj: SupportsQuack) -> str:
    return obj.quack()


# 5) Literal: 정해진 값 중 하나만 허용
def set_mode(mode: Literal["auto", "manual", "off"]) -> str:
    return f"모드가 {mode}(으)로 설정되었습니다."


if __name__ == "__main__":
    print("=== 1) 기본 타입 힌트 ===")
    print(f"add(2, 3) = {add(2, 3)}")
    print(greet("영희"))
    print(greet("철수", 25))
    print(f"total_length(['py', 'thon']) = {total_length(['py', 'thon'])}")
    print()

    print("=== 2) TypedDict ===")
    movie: Movie = {"title": "기생충", "year": 2019, "rating": 8.6}
    print(movie, "-> 타입: Movie")
    print()

    print("=== 3) NewType ===")
    uid = UserId(42)
    pid = ProductId(99)
    print(find_user(uid))
    print(f"UserId는 int의 서브타입인가? {isinstance(uid, int)}")
    print(f"uid + pid = {uid + pid}  # int처럼 연산은 되지만 타입상 구분됨")
    print()

    print("=== 4) Protocol ===")
    # Duck과 Robot 모두 상속 관계가 없지만 quack()을 가지므로 Protocol 만족
    print("Duck:", make_noise(Duck()))
    print("Robot:", make_noise(Robot()))
    print()

    print("=== 5) Literal ===")
    for m in ("auto", "manual"):
        print(set_mode(m))
    # set_mode("turbo")  # mypy는 이 호출을 오류로 표시합니다 (Literal 위반)
