# 11: 객체지향 프로그래밍 (OOP) — class, `__init__`, self, 메서드, `__str__`, `__repr__`

## 클래스와 인스턴스
`class` 키워드로 클래스를 정의하고, 생성자 `__init__`에서 인스턴스 변수를 초기화합니다. 첫 번째 인자는 항상 `self`(인스턴스 자신)입니다.

```python
class Dog:
    def __init__(self, name):
        self.name = name
```

## 인스턴스 메서드 vs 클래스 메서드 vs 정적 메서드
- **인스턴스 메서드**: `self`를 받아 인스턴스 상태에 접근
- **클래스 메서드**: `@classmethod` + `cls`를 받아 클래스 레벨에서 동작
- **정적 메서드**: `@staticmethod` — `self`/`cls` 없이 유틸리티 함수처럼 사용

## `__str__` vs `__repr__`
- `__str__`: `print()`나 `str()` 호출 시 사용자 friendly 문자열
- `__repr__`: `repr()` 호출 시 개발자 friendly 문자열 (디버깅 용도)

```python
class Point:
    def __init__(self, x, y):
        self.x, self.y = x, y
    def __str__(self):
        return f"({self.x}, {self.y})"
    def __repr__(self):
        return f"Point({self.x}, {self.y})"
```
