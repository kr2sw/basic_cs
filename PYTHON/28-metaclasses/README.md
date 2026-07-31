# 28: 메타클래스 (Metaclasses) — type(), `__new__`, 커스텀 메타클래스

## 클래스도 객체다
Python에서 클래스는 `type`의 인스턴스입니다. 즉, 클래스 자체를 만들고 수정할 수 있습니다.

```python
MyClass = type("MyClass", (object,), {"value": 42})
```

## `type()`으로 동적 클래스 생성
`type(이름, 베이스튜플, 속성딕셔너리)` 세 인자로 새 클래스를 만들 수 있습니다.

## `__new__`와 `__init__`
- `type.__new__` / `type.__init__`: **클래스**를 만들 때 호출됩니다.
- `object.__new__` / `object.__init__`: **인스턴스**를 만들 때 호출됩니다.

## 커스텀 메타클래스
`class Meta(type):` 로 메타클래스를 정의하고, `class Foo(metaclass=Meta):`로 사용합니다. 메타클래스를 사용해 자동 데코레이션, 클래스 등록, 속성 검증 등을 할 수 있습니다.

## 실행

```bash
python main.py
```
