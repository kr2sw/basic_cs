class Dog:
    species = "Canis familiaris"

    def __init__(self, name, age):
        self.name = name
        self.age = age

    def bark(self):
        return f"{self.name} says Woof!"

    @classmethod
    def from_birth_year(cls, name, birth_year):
        age = 2026 - birth_year
        return cls(name, age)

    @staticmethod
    def is_domestic():
        return True

    def __str__(self):
        return f"{self.name} ({self.age}살)"

    def __repr__(self):
        return f"Dog({self.name!r}, {self.age})"


class Point:
    def __init__(self, x, y):
        self.x, self.y = x, y

    def __add__(self, other):
        return Point(self.x + other.x, self.y + other.y)

    def __str__(self):
        return f"({self.x}, {self.y})"

    def __repr__(self):
        return f"Point({self.x}, {self.y})"


if __name__ == "__main__":
    dog1 = Dog("Buddy", 3)
    dog2 = Dog.from_birth_year("Max", 2020)

    print(dog1.bark())
    print(dog1)
    print(repr(dog2))
    print(f"Species: {Dog.species}")
    print(f"Domestic: {Dog.is_domestic()}")

    p1 = Point(1, 2)
    p2 = Point(3, 4)
    p3 = p1 + p2
    print(f"{p1} + {p2} = {p3}")
