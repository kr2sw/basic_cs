# 05: Dicts & Sets — 딕셔너리, 세트, 연산과 메서드

## 딕셔너리 (Dictionary)
키(key)-값(value) 쌍으로 데이터를 저장합니다. `{}`로 생성하며, 키는 불변 타입이어야 합니다.

```python
person = {"name": "Alice", "age": 25}
print(person["name"])
person["age"] = 26
```

## 딕셔너리 메서드
- `keys()`, `values()`, `items()`
- `get(key, default)`: 안전하게 값 조회
- `pop(key)`: 키로 항목 제거 후 반환
- `update(dict)`: 여러 키-값 한 번에 갱신

## 세트 (Set)
중복을 허용하지 않는 컬렉션입니다. `{}`로 생성하되 빈 세트는 `set()`을 사용합니다.

```python
a = {1, 2, 3}
b = {3, 4, 5}
```

## 세트 연산
- 합집합: `a | b` 또는 `a.union(b)`
- 교집합: `a & b` 또는 `a.intersection(b)`
- 차집합: `a - b` 또는 `a.difference(b)`
- 대칭 차집합: `a ^ b`

## 세트 메서드
`add()`, `remove()`, `discard()`, `clear()`, `copy()`
