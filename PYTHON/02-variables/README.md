# 02: Variables — 변수, 네이밍 규칙, type(), 동적 타이핑, 여러 할당, None

## 변수 (Variables)
변수는 데이터를 저장하는 공간입니다. `=` 연산자로 값을 할당합니다.

```python
x = 10
name = "Python"
```

## 네이밍 규칙 (Naming Rules)
- 문자, 숫자, 밑줄(`_`)로 구성
- 숫자로 시작할 수 없음
- 예약어(`if`, `for`, `class` 등)는 사용 불가
- 대소문자 구분 (`myVar` ≠ `myvar`)
- 관례: `snake_case` 사용

## type() 함수
변수의 자료형을 확인합니다.

## 동적 타이핑 (Dynamic Typing)
파이썬은 변수에 할당되는 값에 따라 타입이 자동 결정됩니다. 같은 변수에 다른 타입도 재할당 가능합니다.

```python
x = 10       # int
x = "hello"  # str (재할당 가능)
```

## 여러 할당 (Multiple Assignment)
```python
a, b, c = 1, 2, 3
a = b = c = 0
```

## None
값이 없음을 나타내는 특별한 상수입니다. `NoneType` 타입이며, `None`은 `False`와 다릅니다.
