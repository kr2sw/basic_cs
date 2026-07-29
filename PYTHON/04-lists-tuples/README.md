# 04: Lists & Tuples — 리스트, 튜플, 인덱싱, 슬라이싱, 메서드

## 리스트 (List)
가변(mutable) 시퀀스입니다. `[]`로 생성하며, 다양한 메서드를 제공합니다.

```python
nums = [1, 2, 3, 4, 5]
nums.append(6)
nums.remove(3)
```

## 인덱싱 (Indexing)
- 0부터 시작: `list[0]` → 첫 번째 요소
- 음수 인덱스: `list[-1]` → 마지막 요소

## 슬라이싱 (Slicing)
`list[start:stop:step]` 형태로 부분 리스트를 추출합니다.

## 리스트 메서드
- `append(x)`: 끝에 추가
- `insert(i, x)`: i 위치에 삽입
- `remove(x)`: 첫 번째 x 제거
- `pop(i)`: i 위치 요소 제거 후 반환 (i 생략 시 마지막)
- `sort()`, `reverse()`, `index()`, `count()`

## 튜플 (Tuple)
불변(immutable) 시퀀스입니다. `()`로 생성하며, 한 번 생성하면 수정할 수 없습니다.

```python
point = (3, 4)
x, y = point  # 언패킹
```
