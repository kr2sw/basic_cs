# 09: Exceptions — 예외 처리, raise, 사용자 정의 예외

## try / except / else / finally
예외 발생 시 프로그램이 중단되지 않도록 처리합니다.

```python
try:
    result = 10 / 0
except ZeroDivisionError:
    print("0으로 나눌 수 없습니다")
else:
    print("성공:", result)
finally:
    print("항상 실행됩니다")
```

## 여러 예외 처리
```python
except (ValueError, TypeError) as e:
    print(f"오류: {e}")
```

## raise
의도적으로 예외를 발생시킵니다.

```python
if age < 0:
    raise ValueError("나이는 음수일 수 없습니다")
```

## 사용자 정의 예외
Exception 클래스를 상속받아 직접 예외를 정의할 수 있습니다.

```python
class MyError(Exception):
    pass
```

## 일반적인 예외 타입
`ValueError`, `TypeError`, `IndexError`, `KeyError`, `FileNotFoundError`, `ZeroDivisionError`, `AttributeError`
