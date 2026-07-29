# 12: 상속 (Inheritance) — `super()`, 메서드 오버라이딩, 다중 상속, MRO

## 기본 상속
자식 클래스는 부모 클래스의 모든 속성과 메서드를 물려받습니다.

```python
class Animal:
    def speak(self):
        return "..."
class Cat(Animal):
    def speak(self):
        return "Meow"
```

## `super()`와 메서드 오버라이딩
`super()`로 부모 클래스의 메서드를 호출할 수 있습니다. 자식 클래스에서 부모의 메서드를 재정의하는 것을 오버라이딩이라고 합니다.

## 다중 상속과 MRO
파이썬은 다중 상속을 지원하며, 메서드 탐색 순서(MRO, Method Resolution Order)는 `__mro__` 속성이나 `mro()` 메서드로 확인할 수 있습니다. C3 선형화 알고리즘을 따릅니다.

## `isinstance` / `issubclass`
- `isinstance(obj, cls)`: 객체가 특정 클래스의 인스턴스인지 확인
- `issubclass(cls1, cls2)`: 클래스가 다른 클래스의 서브클래스인지 확인
