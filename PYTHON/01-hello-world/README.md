# 01: Hello World — print, input, f-strings, 주석, 기본 자료형

## print()
`print()`는 값을 콘솔에 출력합니다. 여러 값을 쉼표로 구분하면 자동으로 띄어쓰기되어 출력됩니다.

## input()
`input()`은 사용자로부터 문자열을 입력받습니다. 반환값은 항상 문자열(string)입니다.

## f-strings
파이썬 3.6+부터 지원하는 문자열 포맷팅 방식입니다. `f"..."` 안에 `{변수}`를 넣어 값을 삽입할 수 있습니다.

```python
name = "홍길동"
print(f"안녕하세요, {name}님!")
```

## 주석 (Comments)
- 한 줄 주석: `#` 으로 시작
- 여러 줄 주석: `""" """` 또는 `''' '''` (docstring으로도 사용)

## 기본 자료형 (Basic Data Types)
- `int`: 정수 (예: `42`)
- `float`: 실수 (예: `3.14`)
- `str`: 문자열 (예: `"hello"`)
- `bool`: 불리언 (예: `True`, `False`)
- `NoneType`: `None` (값 없음)

```python
print(type(42))        # <class 'int'>
print(type(3.14))      # <class 'float'>
print(type("안녕"))    # <class 'str'>
print(type(True))      # <class 'bool'>
```
