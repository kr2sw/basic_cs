# 03: Control Flow — 조건문, 반복문, break/continue, pass

## if / elif / else
조건에 따라 코드를 실행합니다.

```python
if score >= 90:
    print("A")
elif score >= 80:
    print("B")
else:
    print("C")
```

## for 문
시퀀스(리스트, 문자열 등)를 순회합니다.

```python
for i in range(5):
    print(i)
```

## while 문
조건이 참인 동안 반복합니다.

```python
while count < 5:
    count += 1
```

## range()
`range(start, stop, step)` 형태로 숫자 시퀀스를 생성합니다.

## break / continue
- `break`: 반복문 즉시 종료
- `continue`: 다음 반복으로 건너뜀

## pass
아무 것도 하지 않고 넘어갑니다. 코드 구조를 미리 잡을 때 사용합니다.
