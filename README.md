# Basic CS - 프로그래밍 기초 강의 자료

C, C#, Java, Node.js, PHP, Python, Rust 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 구조

```
basic_cs/
├── C/           C 기초 강의 (20개 챕터)
├── CS/          C# 기초 강의 (20개 챕터)
├── JAVA/        Java 기초 강의 (20개 챕터)
├── NODEJS/      Node.js 기초 강의 (20개 챕터)
├── PHP/         PHP 기초 강의 (20개 챕터)
├── PYTHON/      Python 기초 강의 (20개 챕터)
└── RUST/        Rust 기초 강의 (20개 챕터)
```

## C 기초 (20개 챕터)

```bash
cd C/01-hello-world && gcc main.c -o main && ./main
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 출력, printf/scanf, 주석, 컴파일 과정 |
| 02 | Variables | 변수, 기본 자료형, 형변환, 상수, sizeof |
| 03 | Control Flow | if/else, switch, for/while/do-while, break/continue |
| 04 | Arrays | 1차원/2차원 배열, VLA, 배열과 포인터 |
| 05 | Functions | 함수 정의, 프로토타입, 재귀, static 변수 |
| 06 | Pointers | 포인터 기초, 역참조, 포인터 연산, Call by reference |
| 07 | Strings | 문자 배열, string.h 함수, strtok, sprintf |
| 08 | Structs | 구조체, typedef, 중첩 구조체, 구조체 배열 |
| 09 | File I/O | fopen/fclose, fprintf/fscanf, fread/fwrite |
| 10 | Dynamic Memory | malloc/calloc/realloc/free, 메모리 관리 |
| 11 | Preprocessor | #define, #include, 매크로, 조건부 컴파일 |
| 12 | Multi-file | 헤더 파일, extern, static, 분할 컴파일 |
| 13 | Bit Manipulation | 비트 연산자, 비트 플래그, 시프트, 마스킹 |
| 14 | Recursion | 재귀 함수, 팩토리얼, 피보나치, 하노이 탑 |
| 15 | Linked List | 단일 연결 리스트, 삽입/삭제/탐색/역순 |
| 16 | Stack & Queue | 스택, 큐 (배열/연결 리스트 기반) |
| 17 | Sorting | 버블/선택/삽입/퀵/병합 정렬, 성능 비교 |
| 18 | Search | 선형 탐색, 이진 탐색 (재귀/반복) |
| 19 | Advanced Pointers | 이중 포인터, 함수 포인터, void 포인터, const |
| 20 | OOP Simulation | 구조체 + 함수 포인터로 OOP 흉내내기 |

## C# 기초 (20개 챕터)

```bash
dotnet build CS
dotnet run --project CS/01_hello_world
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 입출력, 첫 C# 프로그램 |
| 02 | Variables | 변수, 데이터 타입, 형변환, nullable |
| 03 | Control Flow | 조건문 (if, switch), 반복문 (for, foreach, while) |
| 04 | Arrays & Collections | 배열, List, Dictionary, HashSet, Queue, Stack |
| 05 | Methods | 메서드 정의, ref/out/params, 오버로딩, 로컬 함수 |
| 06 | Classes & Objects | 클래스, 생성자, 속성, record, struct, 확장 메서드 |
| 07 | Inheritance | 상속, 다형성, virtual/override, abstract, sealed |
| 08 | Interfaces | 인터페이스, 다중 구현, DI 예제 |
| 09 | Exceptions | 예외 처리, try-catch-finally, 사용자 정의 예외 |
| 10 | LINQ | LINQ 쿼리/메서드 구문, GroupBy, 집계 |
| 11 | Delegates & Events | 델리게이트, Func/Action, 이벤트, 클로저 |
| 12 | Generics | 제네릭 클래스/메서드, 제약 조건 |
| 13 | Async & Await | 비동기 프로그래밍, Task, CancellationToken |
| 14 | Strings | String, StringBuilder, 문자열 처리 |
| 15 | DateTime | DateTime, TimeSpan, DateOnly, TimeZoneInfo |
| 16 | File I/O & Streams | 파일 읽기/쓰기, StreamReader/Writer, BinaryReader/Writer |
| 17 | Serialization | JSON (System.Text.Json), XML 직렬화 |
| 18 | Reflection & Attributes | 리플렉션, 커스텀 어트리뷰트 |
| 19 | Networking | HttpClient, TCP 클라이언트/서버 |
| 20 | Unit Testing | 단위 테스트 (Calculator 예제) |

## Java 기초 (20개 챕터)

```bash
cd JAVA/01-hello-world && javac Main.java && java Main
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 입출력, 첫 Java 프로그램, 주석 |
| 02 | Variables | 변수, 기본형/참조형, 형변환, 상수 |
| 03 | Control Flow | 조건문 (if/else, switch), 반복문 (for, while, do-while) |
| 04 | Arrays | 배열, 다차원 배열, Arrays 클래스 |
| 05 | Methods | 메서드 정의, 오버로딩, 가변인자, 재귀 |
| 06 | OOP | 클래스, 객체, 생성자, this, 접근 제어자 |
| 07 | Inheritance | 상속, super, 오버라이딩, Object 클래스 |
| 08 | Interface & Abstract | 인터페이스, 추상 클래스, 다형성 |
| 09 | Packages | 패키지, import, import static, classpath |
| 10 | Exceptions | 예외 처리, try-catch-finally, 사용자 정의 예외 |
| 11 | Wrapper & String | Wrapper 클래스, String, StringBuilder |
| 12 | Collections | List, Set, Map, Iterator, Comparable |
| 13 | Generics | 제네릭 클래스/메서드, 와일드카드 |
| 14 | Lambda & Stream | 람다 표현식, Stream API, Optional |
| 15 | I/O | File, Byte/Char Stream, NIO |
| 16 | Threads | Thread, Runnable, ExecutorService, 동기화 |
| 17 | JDBC | JDBC, Connection, PreparedStatement, 트랜잭션 |
| 18 | Networking | Socket, ServerSocket, InetAddress, URL |
| 19 | Date & Time | LocalDate, LocalTime, DateTimeFormatter |
| 20 | Testing & Annotations | JUnit, 커스텀 어노테이션, Reflection |

## PHP 기초 (20개 챕터)

```bash
cd PHP/01-hello-world && php index.php
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 출력, 변수, 주석, PHP 기본 문법 |
| 02 | Variables | 변수, 데이터 타입, 형변환, 상수, null |
| 03 | Control Flow | if/else, switch, for/while/foreach, match |
| 04 | Arrays | 인덱스 배열, 연관 배열, 다차원 배열, 배열 함수 |
| 05 | Functions | 함수 정의, 파라미터, return, 가변인자, 화살표 함수 |
| 06 | Strings | 문자열 함수, 포맷팅, 정규표현식, heredoc |
| 07 | OOP | 클래스, 객체, 생성자, 접근 제어자, static |
| 08 | Inheritance | 상속, parent, 오버라이딩, final |
| 09 | Interface & Abstract | 인터페이스, 추상 클래스, trait |
| 10 | Superglobals | $_GET, $_POST, $_SESSION, $_COOKIE, $_SERVER |
| 11 | Forms & Validation | 폼 처리, 필터링, 유효성 검사 |
| 12 | File Handling | 파일 읽기/쓰기, 디렉토리, glob |
| 13 | Error Handling | 예외 처리, try-catch, 사용자 정의 예외 |
| 14 | Sessions & Cookies | 세션 관리, 쿠키 설정/읽기/삭제 |
| 15 | Database (PDO) | PDO 연결, prepared statements, CRUD |
| 16 | JSON & APIs | json_encode/decode, cURL, REST API |
| 17 | Date & Time | date(), DateTime, DateInterval, 시간대 |
| 18 | File Upload | 파일 업로드, MIME 검사, 보안 |
| 19 | Namespaces | 네임스페이스, use, autoload, Composer |
| 20 | MVC Pattern | MVC 패턴, 라우팅, 컨트롤러, 뷰 |

## Node.js 기초 (20개 챕터)

```bash
cd NODEJS/01-introduction && node index.js
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | Node.js 소개, REPL |
| 02 | Module System | CommonJS, ES Modules |
| 03 | npm | 패키지 관리, package.json |
| 04 | File System | fs 모듈 (readFile, writeFile) |
| 05 | Path | path 모듈 (join, resolve, parse) |
| 06 | HTTP | 기본 HTTP 서버 |
| 07 | Express | Express 프레임워크 |
| 08 | Routing & Middleware | 라우터, 미들웨어 |
| 09 | Template Engines | EJS 템플릿 엔진 |
| 10 | REST API | RESTful API 설계, CRUD |
| 11 | Database | SQLite (better-sqlite3) |
| 12 | Authentication | JWT, bcrypt 인증 |
| 13 | File Uploads | multer 파일 업로드 |
| 14 | WebSocket | ws 라이브러리, 채팅 |
| 15 | Error Handling | 에러 처리, winston 로깅 |
| 16 | Environment | dotenv, 환경 변수 |
| 17 | Async Patterns | Callback, Promise, async/await |
| 18 | Streams | 스트림, pipeline, zlib |
| 19 | Testing | Jest, supertest |
| 20 | Deployment | PM2, Docker 배포 |

## Python 기초 (20개 챕터)

```bash
cd PYTHON/01-hello-world && python main.py
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | print(), input(), f-strings, 자료형 |
| 02 | Variables | 변수, 동적 타이핑, None, 다중 할당 |
| 03 | Control Flow | if/elif/else, for/while, range(), break/continue |
| 04 | Lists & Tuples | 리스트, 튜플, 인덱싱, 슬라이싱, 메서드 |
| 05 | Dicts & Sets | 딕셔너리, 세트, 집합 연산 |
| 06 | Functions | def, return, *args/**kwargs, lambda, docstring |
| 07 | Strings | 문자열 메서드, 포맷팅, 슬라이싱 |
| 08 | File I/O | open/with, read/write, pathlib |
| 09 | Exceptions | try/except/else/finally, raise, 사용자 정의 예외 |
| 10 | Modules & Packages | import, pip, __name__, os/sys |
| 11 | OOP | class, __init__, self, classmethod, staticmethod |
| 12 | Inheritance | super(), 다중 상속, MRO, isinstance |
| 13 | Decorators | @decorator, functools.wraps, @property |
| 14 | Iterators & Generators | __iter__/__next__, yield, itertools |
| 15 | Comprehensions | list/dict/set 컴프리헨션, 조건부 컴프리헨션 |
| 16 | Lambda & Map/Filter | lambda, map(), filter(), reduce(), sorted() |
| 17 | DateTime | datetime, date, timedelta, strftime |
| 18 | JSON & APIs | json 모듈, urllib.request, GET/POST |
| 19 | venv & pip | 가상환경, requirements.txt, pip |
| 20 | Testing | unittest, pytest, assert, mock |

## Rust 기초 (20개 챕터)

```bash
cd RUST/01-hello-world && cargo run
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 출력, Cargo 프로젝트 |
| 02 | Variables | 변수, 가변성, 데이터 타입, Shadowing |
| 03 | Control Flow | 조건문, 반복문 (loop, while, for) |
| 04 | Ownership | 소유권, 참조, 대여, 슬라이스 |
| 05 | Structs | 구조체, 메서드, 연관 함수 |
| 06 | Enums & Pattern Matching | 열거형, match, if let |
| 07 | Collections | Vec, HashMap, HashSet |
| 08 | Error Handling | Result, Option, unwrap, ?, panic! |
| 09 | Generics | 제네릭 함수, 구조체, Trait 제약 |
| 10 | Traits | 트레잇 정의, 구현, Derive |
| 11 | Lifetime | 라이프타임 명시, 생략 규칙 |
| 12 | Closures & Iterators | 클로저, 반복자 어댑터 |
| 13 | Modules & Crates | 모듈 시스템, 외부 크레이트 |
| 14 | File I/O | 파일 읽기/쓰기, BufReader |
| 15 | Smart Pointers | Box, Rc, RefCell, Arc, Mutex |
| 16 | Concurrency | 스레드, 채널 (mpsc), Mutex |
| 17 | Unsafe Rust | 원시 포인터, unsafe 블록 |
| 18 | Macros | 선언적 매크로 (macro_rules!), 속성 매크로 |
| 19 | Testing | 단위 테스트, 통합 테스트, doc 테스트 |
| 20 | Web Server | 간단한 HTTP 서버 (TcpListener) |
