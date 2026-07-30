# 19: Advanced Pointers — 고급 포인터

## 이중 포인터 (Double Pointer)

포인터의 포인터. 2차원 배열이나 함수 내에서 포인터 수정에 사용합니다.

```c
int x = 42;
int* ptr = &x;
int** dptr = &ptr;  // 이중 포인터
```

## 함수 포인터 (Function Pointer)

함수의 주소를 저장하는 포인터입니다.

```c
int (*funcPtr)(int, int) = &add;
int result = funcPtr(3, 5);
```

## void 포인터 (Generic Pointer)

타입이 없는 포인터. 모든 포인터를 저장할 수 있습니다.
사용 시 적절한 타입으로 형변환이 필요합니다.
