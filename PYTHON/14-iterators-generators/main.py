import itertools


class CountDown:
    def __init__(self, start):
        self.current = start

    def __iter__(self):
        return self

    def __next__(self):
        if self.current < 0:
            raise StopIteration
        value = self.current
        self.current -= 1
        return value


def fibonacci(limit):
    a, b = 0, 1
    for _ in range(limit):
        yield a
        a, b = b, a + b


def read_lines(filepath):
    with open(filepath, "r", encoding="utf-8") as f:
        for line in f:
            yield line.strip()


if __name__ == "__main__":
    print("=== CountDown iterator ===")
    for n in CountDown(5):
        print(n, end=" ")
    print()

    print("\n=== Fibonacci generator ===")
    for n in fibonacci(10):
        print(n, end=" ")
    print()

    print("\n=== Generator expression ===")
    squares = (x ** 2 for x in range(10))
    print(f"Type: {type(squares).__name__}")
    for s in squares:
        print(s, end=" ")
    print()

    print("\n=== itertools basics ===")
    print("First 5 even squares:")
    evens = (x for x in range(100) if x % 2 == 0)
    for n in itertools.islice(evens, 5):
        print(n, end=" ")
    print()

    print("chain('abc', [1,2,3]):")
    for item in itertools.chain("abc", [1, 2, 3]):
        print(item, end=" ")
    print()

    print("First 5 of cycle('AB'):")
    for i, item in enumerate(itertools.cycle("AB")):
        if i >= 5:
            break
        print(item, end=" ")
    print()

    print("\n=== Read lines generator (from this file) ===")
    for i, line in enumerate(read_lines(__file__)):
        if i >= 3:
            break
        print(f"  {line}")
