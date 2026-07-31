# C 기초 강의 (20개 챕터)

C 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

C 언어는 1972년 Dennis Ritchie가 AT&T 벨 연구소에서 UNIX 운영체제를 개발하기 위해 설계했습니다. 전신인 B 언어(Ken Thompson)에서 발전하여 구조적 프로그래밍과 하드웨어 제어 기능을 갖췄습니다. 1978년 Kernighan과 Ritchie의 "The C Programming Language" (K&R) 책이 표준 역할을 했고, 1989년 ANSI C(C89), 1999년 C99, 2011년 C11, 2018년 C18로 표준화가 이어졌습니다. C는 UNIX, Linux, Windows 커널의 핵심 언어로, 현대 프로그래밍 언어(C++, Java, C#, Go, Rust)에 막대한 영향을 미쳤습니다.

## 특징

- **이식성 높은 저수준 언어**: 어셈블리어 수준의 하드웨어 제어 가능하면서도 다양한 플랫폼으로 이식 가능
- **효율성**: 런타임 오버헤드가 거의 없어 임베디드 시스템, OS 커널에 최적
- **포인터**: 메모리 주소를 직접 조작하는 강력한 기능
- **구조적 프로그래밍**: 함수 기반의 모듈화, 순차/선택/반복 제어 구조
- **최소한의 런타임**: 가비지 컬렉션 등 오버헤드가 없음
- **방대한 레거시**: 수십 년간 축적된 라이브러리와 시스템

## 실행

```bash
cd C/01-hello-world && gcc main.c -o main && ./main
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 출력, printf, scanf, 주석, 컴파일 과정 |
| 02 | Variables | 변수, 기본 자료형, 형변환, 상수, sizeof |
| 03 | Control Flow | if/else, switch, for/while/do-while, break/continue |
| 04 | Arrays | 1차원/2차원 배열, sizeof 배열, 배열과 포인터 |
| 05 | Functions | 함수 정의, 파라미터, return, 프로토타입, 재귀 |
| 06 | Pointers | 포인터 기초, 역참조, 포인터 연산, 배열과 포인터 |
| 07 | Strings | 문자 배열, 문자열 함수, str系列 |
| 08 | Structs | 구조체, typedef, 중첩 구조체, 구조체 배열 |
| 09 | File I/O | fopen/fclose, fprintf/fscanf, fread/fwrite |
| 10 | Dynamic Memory | malloc/calloc/realloc/free, 메모리 누수 방지 |
| 11 | Preprocessor | #define, #include, 매크로, 조건부 컴파일 |
| 12 | Multi-file | 헤더 파일, extern, static, makefile |
| 13 | Bit Manipulation | 비트 연산자, 비트 플래그, shift, 마스킹 |
| 14 | Recursion | 재귀 함수, 팩토리얼, 피보나치, 하노이 탑 |
| 15 | Linked List | 단일 연결 리스트, 삽입/삭제/탐색 |
| 16 | Stack & Queue | 스택, 큐 (배열/연결 리스트 기반) |
| 17 | Sorting | 버블 정렬, 선택 정렬, 삽입 정렬, 퀵 정렬 |
| 18 | Search | 선형 탐색, 이진 탐색 |
| 19 | Advanced Pointers | 이중 포인터, 함수 포인터, void 포인터 |
| 20 | OOP Simulation | 구조체 + 함수 포인터로 OOP 흉내내기 |
| 21 | Advanced Structs | 비트 필드, union, 열거형 심화, flexible array member |
| 22 | Function Pointers | 콜백, qsort, 커맨드 테이블, 함수 반환 |
| 23 | Variadic Functions | stdarg.h, va_list, 안전한 가변 인자 패턴 |
| 24 | Advanced File I/O | 바이너리, 랜덤 접근(fseek), 버퍼링 |
| 25 | Trees | 이진 탐색 트리, 순회, AVL 균형 |
| 26 | Graphs | 인접 리스트/행렬, DFS, BFS, 최단경로 |
| 27 | Hash Table | 해시 함수, 체이닝, 오픈 어드레싱 |
| 28 | Dynamic Strings | 문자열 빌더, 토큰화, 정규화 |
| 29 | Memory Optimization | 메모리 풀, 커스텀 할당자, 캐시 친화적 코드 |
| 30 | Signals & Errors | errno, strerror, assert, abort |
| 31 | Processes | fork/exec 개념, exit, 환경 변수 (Windows 참고) |
| 32 | Threads | pthread 개념, 동기화 (Windows 스레드 참고) |
| 33 | Sockets | TCP/UDP 클라이언트-서버 (POSIX, 개념 중심) |
| 34 | Build Systems | Makefile, 컴파일 단계, 정적/동적 라이브러리 |
| 35 | Embedded C | 레지스터 접근, volatile, ISR, 비트 마스킹 |
| 36 | Crypto Basics | XOR, 해시 구현, HMAC 개념, 난수 |
| 37 | Parsers | 토크나이저, 표현식 계산기, 재귀 하강 파서 |
| 38 | Interop | C ABI, extern "C", Python ctypes 호출 |
| 39 | Design Patterns in C | 상태 머신, 옵저버, 리소스 풀 |
| 40 | Final Project | 미니 메모리 기반 DB (파일 저장) |
