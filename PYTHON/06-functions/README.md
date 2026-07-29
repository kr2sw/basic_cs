# 06: Functions — 함수 정의, 매개변수, lambda, docstring

## 함수 정의 (def)
`def` 키워드로 함수를 정의합니다.

```python
def greet(name):
    """인사말을 출력합니다."""
    print(f"안녕하세요, {name}님!")
```

## return
함수는 값을 반환할 수 있습니다. `return`이 없으면 `None`을 반환합니다.

## 매개변수 (Parameters)
- 기본값 매개변수: `def func(a, b=10)`
- 위치 인자: `func(1, 2)`
- 키워드 인자: `func(a=1, b=2)`

## *args / **kwargs
- `*args`: 가변 개수의 위치 인자를 튜플로 받음
- `**kwargs`: 가변 개수의 키워드 인자를 딕셔너리로 받음

```python
def sum_all(*args):
    return sum(args)
```

## lambda
익명 함수를 한 줄로 정의합니다.

```python
square = lambda x: x ** 2
```

## docstring
함수 첫 줄에 `""" """`으로 문서화 문자열을 작성합니다.
