# 14: 반복자(Iterator)와 생성자(Generator)

## Iterator (`__iter__` / `__next__`)
`__iter__`는 self를 반환하고 `__next__`는 다음 값을 반환합니다. 더 이상 값이 없으면 `StopIteration` 예외를 발생시킵니다.

```python
class Counter:
    def __iter__(self):
        self.n = 0; return self
    def __next__(self):
        n = self.n; self.n += 1; return n
```

## Generator (`yield`)
`yield` 키워드를 사용하면 함수가 generator가 됩니다. 상태를 기억하며 값을 하나씩 생성합니다. 메모리 효율적입니다.

## Generator Expression
리스트 컴프리헨션과 비슷하지만 `()`로 감싸며, 한 번에 모든 값을 생성하지 않고 필요할 때 하나씩 생성합니다.

## itertools 기초
`itertools` 모듈은 효율적인 반복자 도구를 제공합니다: `count`, `cycle`, `chain`, `islice` 등.
