# 34: 빌드 시스템 — Makefile, 컴파일 단계, 정적/동적 라이브러리

## 컴파일 단계

소스 → 실행 파일까지 4단계입니다.

```
전처리(.i) → 컴파일(.s) → 어셈블리(.o) → 링크(실행 파일)
```

```bash
gcc -E main.c      # 전처리 결과만
gcc -S main.c      # 어셈블리어로
gcc -c main.c      # 오브젝트 파일(.o) 생성
gcc main.o -o main # 링크
```

## Makefile 기초

```make
CC = gcc
CFLAGS = -Wall -O2

main: main.o util.o
	$(CC) $(CFLAGS) -o main main.o util.o

main.o: main.c util.h
	$(CC) $(CFLAGS) -c main.c

clean:
	rm -f *.o main
```

- `target: prerequisites` + 탭(tab)으로 시작하는 명령어
- 의존 파일이 바뀌었을 때만 재컴파일

## 정적/동적 라이브러리

```bash
# 정적 라이브러리 (.a)
gcc -c util.c && ar rcs libutil.a util.o
gcc main.c -L. -lutil -o main

# 동적(공유) 라이브러리 (.so / .dll)
gcc -shared -fPIC -o libutil.so util.c
gcc main.c -L. -lutil -o main
```

| 구분 | 확장자(Linux) | 특징 |
|------|--------------|------|
| 정적 | `.a` | 실행 파일에 복사됨, 크기 증가, 이식성 높음 |
| 동적 | `.so` | 실행 시 로드, 메모리 절약, 버전 관리 가능 |

## 실행

```bash
gcc main.c -o main && ./main
```
