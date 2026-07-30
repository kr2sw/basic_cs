# 12: Multi-file — 다중 파일 프로그래밍

## 헤더 파일 (.h)

- 함수 프로토타입, 매크로, typedef, extern 변수 선언
- `#include "헤더.h"`로 포함
- 중복 포함 방지: `#pragma once` 또는 `#ifndef ... #define ... #endif`

## 소스 파일 (.c)

- 함수 구현, 전역 변수 정의
- 각 .c 파일은 독립적으로 컴파일

## extern

다른 파일에서 정의된 전역 변수를 참조할 때 사용합니다.

## static

- **파일 내부**: 해당 파일에서만 접근 가능 (내부 연결)
- **함수 내부**: 호출 간 값 유지

## Makefile

```makefile
CC = gcc
CFLAGS = -Wall -Wextra
OBJS = main.o math_utils.o

app: $(OBJS)
	$(CC) -o app $(OBJS)

%.o: %.c
	$(CC) $(CFLAGS) -c $<

clean:
	rm -f *.o app
```
