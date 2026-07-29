class Animal:
    def __init__(self, name):
        self.name = name

    def speak(self):
        return "..."

    def __str__(self):
        return self.name


class Cat(Animal):
    def speak(self):
        return "Meow"


class Dog(Animal):
    def speak(self):
        return "Woof"


class Robot:
    def __init__(self, model):
        self.model = model

    def charge(self):
        return f"{self.model} charging..."


class RoboDog(Dog, Robot):
    def __init__(self, name, model):
        Dog.__init__(self, name)
        Robot.__init__(self, model)

    def speak(self):
        return super().speak() + " (battery 100%)"


class A:
    def method(self):
        return "A"


class B(A):
    def method(self):
        return "B"


class C(A):
    def method(self):
        return "C"


class D(B, C):
    pass


if __name__ == "__main__":
    cat = Cat("Luna")
    dog = Dog("Rex")
    robodog = RoboDog("X10", "RD-2000")

    print(f"{cat}: {cat.speak()}")
    print(f"{dog}: {dog.speak()}")
    print(f"{robodog}: {robodog.speak()}")
    print(f"{robodog.charge()}")

    print(f"isinstance(dog, Animal): {isinstance(dog, Animal)}")
    print(f"issubclass(RoboDog, Dog): {issubclass(RoboDog, Dog)}")
    print(f"issubclass(RoboDog, Robot): {issubclass(RoboDog, Robot)}")

    d = D()
    print(f"D MRO: {[c.__name__ for c in D.__mro__]}")
    print(f"d.method(): {d.method()}")
