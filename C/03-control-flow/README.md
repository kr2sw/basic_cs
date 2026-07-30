# 03: Control Flow — 조건문과 반복문

## 조건문

### if / else if / else
```c
if (조건) {
    // true
} else if (다른 조건) {
    // 다른 조건 true
} else {
    // false
}
```

### switch
```c
switch (값) {
    case 1: ... break;
    case 2: ... break;
    default: ...
}
```

## 반복문

- **for**: `for (초기화; 조건; 증감)`
- **while**: `while (조건)`
- **do-while**: `do { ... } while (조건);` (최소 1회 실행)

## break / continue

- `break`: 반복문 즉시 종료
- `continue`: 다음 반복으로 이동
