# 15: 컴프리헨션 (Comprehensions)

## List Comprehension
`[표현식 for 변수 in 반복가능객체 if 조건]`
기존 리스트를 변환하거나 필터링할 때 간결하게 작성합니다.

## Dict Comprehension
`{키: 값 for 변수 in 반복가능객체 if 조건}`
딕셔너리를 생성할 때 사용합니다.

## Set Comprehension
`{표현식 for 변수 in 반복가능객체 if 조건}`
중복이 없는 집합(set)을 생성할 때 사용합니다.

## 중첩 컴프리헨션
컴프리헨션 안에 컴프리헨션을 중첩할 수 있습니다. 이중 for 루프와 동일합니다.

```python
matrix = [[1,2],[3,4]]
flat = [x for row in matrix for x in row]  # [1,2,3,4]
```

## 조건부 컴프리헨션
`if` 조건으로 요소를 필터링할 수 있습니다. `if-else`는 표현식 앞에 위치합니다.
