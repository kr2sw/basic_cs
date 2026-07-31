# 30: 함수형 프로그래밍 (Functional) — itertools, functools.partial, curry

## 함수형 스타일
부수 효과 없이 데이터를 변환하는 함수들을 조합하는 방식입니다. `map`, `filter`, `reduce`와 더불어 `itertools`, `functools`가 핵심입니다.

## `itertools`
- `chain`: 여러 이터러블 연결
- `product`, `permutations`, `combinations`: 조합/순열
- `groupby`: 연속된 키로 그룹핑
- `islice`, `takewhile`: 지연 생성 슬라이싱

## `functools.partial`
함수에 인자를 미리 고정해 새 함수를 만듭니다.

```python
from functools import partial
double = partial(mul, 2)
```

## 커링 (Currying)
여러 인자를 받는 함수를 인자 하나씩 받는 함수들의 체인으로 변환합니다.

## 실행

```bash
python main.py
```
