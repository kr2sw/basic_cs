# 21: 고급 구조체 — 비트 필드, union, 열거형 심화, flexible array member

## 비트 필드 (Bit Field)

구조체 멤버에 비트 단위 크기를 지정해 메모리를 절약합니다.

```c
typedef struct {
    unsigned int red   : 3;  // 0~7 (3비트)
    unsigned int green : 3;
    unsigned int blue  : 3;
} RGB3;
```

- 비트 플래그, 하드웨어 레지스터 표현에 유용
- 비트 수는 타입이 담을 수 있는 크기를 넘을 수 없음
- 바이트 경계 정렬 방식은 컴파일러마다 달라질 수 있음

## union

같은 메모리 공간을 여러 타입으로 해석합니다. `sizeof(union)`은 가장 큰 멤버의 크기입니다.

```c
typedef union {
    int i;
    float f;
    unsigned char bytes[4];
} Value;
```

- 한 번에 한 멤버만 의미 있게 사용해야 함 (active member 규칙)
- 엔디언, 메모리 재해석(bit casting)에 자주 사용

## 열거형 심화 (enum)

기본값이 0부터 자동 증가하지만, 값을 명시할 수 있습니다. `1 << n` 형태로 비트 플래그에도 사용합니다.

```c
typedef enum { READ = 1 << 0, WRITE = 1 << 1, EXEC = 1 << 2 } Permission;
```

## Flexible Array Member (C99+)

구조체의 마지막에 크기를 지정하지 않은 배열을 둡니다. 할당 시 필요한 만큼 붙여서 만듭니다.

```c
typedef struct {
    int len;
    int data[];  // sizeof(IntVec)에는 미포함
} IntVec;

IntVec* v = malloc(sizeof(IntVec) + n * sizeof(int));
```

- 구조체 헤더 + 가변 데이터의 관용적 패턴 (네트워크 패킷 등)
- C11 표준은 마지막 멤버에만, 한 개만 허용

## 실행

```bash
gcc main.c -o main && ./main
```
