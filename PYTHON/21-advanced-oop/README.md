# 21: 고급 OOP (Advanced OOP) — dataclass, `__slots__`, 추상 클래스, 매직 메서드

## `@dataclass`
데이터를 담는 클래스에 `__init__`, `__repr__`, `__eq__` 등을 자동으로 생성해 줍니다. `field(default_factory=...)`로 가변 기본값도 처리할 수 있습니다.

```python
@dataclass
class Point:
    x: float
    y: float = 0.0
```

## `__slots__`
인스턴스마다 `__dict__`를 만들지 않고 고정된 속성만 허용합니다. 메모리를 절약하고 속성 접근이 빨라지지만, 선언한 속성만 사용할 수 있습니다.

## 추상 클래스 (ABC)
`abc.ABC`를 상속하고 `@abstractmethod`로 표시한 메서드는 자식 클래스가 반드시 구현해야 합니다. 공통 인터페이스를 강제할 때 사용합니다.

## 매직 메서드
`__repr__`, `__eq__`, `__len__`, `__getitem__`, `__call__`, `__add__` 등을 구현하면 내장 연산자/함수와 자연스럽게 통합됩니다.

## 실행

```bash
python main.py
```
