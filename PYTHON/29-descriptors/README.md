# 29: 디스크립터 (Descriptors) — `__get__`/`__set__`, property 내부 동작

## 디스크립터란?
`__get__`, `__set__`, `__delete__` 중 하나라도 구현한 객체입니다. 클래스 속성으로 두면 인스턴스 접근 시 자동으로 호출됩니다.

```python
class Positive:
    def __get__(self, obj, objtype=None):
        return self.value
    def __set__(self, obj, value):
        if value < 0:
            raise ValueError("음수 불가")
        self.value = value
```

## `__get__` 시그니처
`__get__(self, instance, owner)` — 인스턴스 접근이면 `instance`, 클래스 접근이면 `None`이 전달됩니다.

## 데이터 vs 비데이터 디스크립터
- `__set__`이 있으면 **데이터 디스크립터**: 인스턴스 `__dict__`보다 우선
- `__set__`이 없으면 **비데이터 디스크립터**: 인스턴스 `__dict__`가 우선 (메서드가 대표적)

## property의 내부
`property`는 `__get__`/`__set__`/`__delete__`를 구현한 디스크립터입니다. 직접 만들어 동작을 재현할 수 있습니다.

## 실행

```bash
python main.py
```
