# 22: 함수 포인터 고급 — 콜백, qsort, 커맨드 테이블, 함수 반환

## 콜백 (Callback)

다른 함수에 함수 포인터를 넘겨 특정 시점에 호출되게 합니다.

```c
void forEach(int arr[], int n, void (*fn)(int));
```

## qsort

C 표준 라이브러리의 정렬 함수. 비교 함수(comparator)를 함수 포인터로 받습니다.

```c
int cmpInt(const void* a, const void* b);
qsort(arr, n, sizeof(int), cmpInt);
```

- `const void*`로 모든 타입을 받는 제네릭 설계
- 문자열 배열, 구조체 배열도 같은 방식으로 정렬 가능

## 커맨드 테이블 (Command Table)

함수 포인터 배열로 메뉴/명령어를 분기 없이 처리합니다.

```c
typedef struct { const char* name; void (*fn)(void); } Command;
Command table[] = {{"help", cmdHelp}, {"quit", cmdQuit}};
```

## 함수를 반환하는 함수 (팩토리)

함수 포인터를 반환해 연산자에 맞는 함수를 골라줍니다.

```c
int (*getOperation(char op))(int, int);
```

## 실행

```bash
gcc main.c -o main && ./main
```
