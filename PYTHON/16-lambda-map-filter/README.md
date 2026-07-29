# 16: Lambda, map, filter, reduce, sorted

## Lambda 함수
익명 함수를 한 줄로 정의합니다. `lambda 인자: 표현식` 형태로 사용합니다.

```python
square = lambda x: x ** 2
add = lambda a, b: a + b
```

## map()
시퀀스의 각 요소에 함수를 적용한 iterator를 반환합니다.

## filter()
시퀀스의 요소 중 조건이 참인 것만 필터링한 iterator를 반환합니다.

## reduce() (functools)
시퀀스의 요소를 누적하여 단일 값으로 줄입니다. `functools.reduce`에서 제공합니다.

## sorted() with key
`sorted(iterable, key=func)` — `key` 인자에 함수를 전달하여 정렬 기준을 지정합니다. `list.sort()`도 동일한 `key`를 지원합니다.
