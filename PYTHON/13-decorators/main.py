import functools
import time


def timer(func):
    @functools.wraps(func)
    def wrapper(*args, **kwargs):
        start = time.perf_counter()
        result = func(*args, **kwargs)
        elapsed = time.perf_counter() - start
        print(f"[Timer] {func.__name__} took {elapsed:.6f}s")
        return result
    return wrapper


def logger(func):
    @functools.wraps(func)
    def wrapper(*args, **kwargs):
        print(f"[LOG] Calling {func.__name__}({args!r}, {kwargs!r})")
        result = func(*args, **kwargs)
        print(f"[LOG] {func.__name__} returned {result!r}")
        return result
    return wrapper


@timer
@logger
def slow_add(a, b):
    time.sleep(0.1)
    return a + b


class Circle:
    def __init__(self, radius):
        self._radius = radius

    @property
    def radius(self):
        return self._radius

    @radius.setter
    def radius(self, value):
        if value < 0:
            raise ValueError("Radius cannot be negative")
        self._radius = value

    @property
    def area(self):
        return 3.14159 * self._radius ** 2

    @classmethod
    def from_diameter(cls, diameter):
        return cls(diameter / 2)

    @staticmethod
    def description():
        return "A geometric shape defined by a center point and a radius"


if __name__ == "__main__":
    result = slow_add(3, 5)
    print(f"Result: {result}")
    print(f"Function name preserved: {slow_add.__name__}")
    print()

    c = Circle(5)
    print(f"Radius: {c.radius}")
    print(f"Area: {c.area:.2f}")
    c.radius = 10
    print(f"New area: {c.area:.2f}")

    c2 = Circle.from_diameter(20)
    print(f"From diameter: radius={c2.radius}, area={c2.area:.2f}")
    print(c2.description())
