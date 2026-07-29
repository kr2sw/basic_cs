# 13: 데코레이터 (Decorators) — `@decorator`, `functools.wraps`, `@property`

## 데코레이터 기본
데코레이터는 함수를 인자로 받아 새로운 함수를 반환하는 호출 가능 객체입니다. `@` 문법으로 사용합니다.

```python
def my_decorator(func):
    def wrapper(*args, **kwargs):
        print("before")
        result = func(*args, **kwargs)
        print("after")
        return result
    return wrapper
```

## `functools.wraps`
데코레이터를 작성할 때 `@functools.wraps`를 사용하면 원본 함수의 메타데이터(`__name__`, `__doc__` 등)를 보존합니다.

## 내장 데코레이터
- `@classmethod`: 클래스 메서드로 변환
- `@staticmethod`: 정적 메서드로 변환
- `@property`: 메서드를 속성처럼 접근 가능하게 함 (getter/setter)

## 실용 예제
- **Timer**: 함수 실행 시간 측정
- **Logging**: 함수 호출 로깅
