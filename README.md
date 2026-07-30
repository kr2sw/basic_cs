# Basic CS - 프로그래밍 기초 강의 자료

Arduino, C, C#, FastAPI, Git & GitHub, Java, MicroPython, Node.js, PHP, Python, React, Rust, TypeScript, VB.NET, Vue.js, WebAssembly, WPF 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 구조

```
basic_cs/
├── ARDUINO/     Arduino 기초 강의 (20개 챕터)
├── C/           C 기초 강의 (20개 챕터)
├── CS/          C# 기초 강의 (20개 챕터)
├── FASTAPI/     FastAPI 기초 강의 (20개 챕터)
├── GIT/         Git & GitHub 기초 강의 (20개 챕터)
├── JAVA/        Java 기초 강의 (20개 챕터)
├── MP/          MicroPython 기초 (20개 챕터)
├── NODEJS/      Node.js 기초 강의 (20개 챕터)
├── PHP/         PHP 기초 강의 (20개 챕터)
├── PYTHON/      Python 기초 강의 (20개 챕터)
├── REACT/       React 기초 강의 (20개 챕터)
├── RUST/        Rust 기초 강의 (20개 챕터)
├── TYPESCRIPT/  TypeScript 기초 강의 (20개 챕터)
├── VB/          Visual Basic .NET 기초 강의 (20개 챕터)
├── VUE/         Vue.js 기초 강의 (20개 챕터)
├── WASM/        WebAssembly 기초 (20개 챕터)
└── WPF/         WPF 기초 강의 (20개 챕터)
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

## FastAPI 기초 (20개 챕터)

```bash
cd FASTAPI/01-introduction
pip install -r requirements.txt
uvicorn main:app --reload
# http://127.0.0.1:8000
# API 문서: http://127.0.0.1:8000/docs
```

| # | 주제 | 설명 |
|---|------|------|
| 00 | Setup | FastAPI 설치, Uvicorn, 가상 환경 |
| 01 | Introduction | 첫 FastAPI 앱, 자동 문서화 |
| 02 | Path & Query | 경로/쿼리 매개변수, Path/Query 검증 |
| 03 | Request Body | Pydantic 모델, POST 요청 |
| 04 | Response Model | 응답 모델, 상태 코드, 필드 제어 |
| 05 | HTTP Methods | GET/POST/PUT/PATCH/DELETE CRUD |
| 06 | Validation | Field, @field_validator, 커스텀 검증 |
| 07 | Error Handling | HTTPException, 커스텀 예외 처리기 |
| 08 | Dependencies | Depends, 의존성 주입, DB 세션 |
| 09 | Middleware | CORS, 커스텀 미들웨어, 로깅 |
| 10 | Database | SQLAlchemy, ORM, CRUD with DB |
| 11 | Authentication | OAuth2, JWT, passlib 해싱 |
| 12 | File Upload | UploadFile, 다중 파일 업로드 |
| 13 | Static & Templates | Jinja2, StaticFiles, HTML 렌더링 |
| 14 | WebSocket | 실시간 채팅, WebSocket 연결 관리 |
| 15 | Background Tasks | BackgroundTasks, 이메일 발송 |
| 16 | Testing | TestClient, pytest, parametrize |
| 17 | Versioning | APIRouter, prefix, 다중 버전 |
| 18 | Async Advanced | httpx.AsyncClient, asyncio.gather |
| 19 | Caching | ETag, lru_cache, Redis 캐싱 |
| 20 | Deployment | Uvicorn/Gunicorn, Docker, docker-compose |

## Git & GitHub 기초 (20개 챕터)

```bash
git init
git add .
git commit -m "first commit"
```

| 장 | 제목 | 설명 |
|----|------|------|
| 00 | 개발 환경 설정 | Git 설치, GitHub 계정, 기본 설정 |
| 01 | Git 소개 | 저장소, 커밋, 기본 개념 |
| 02 | 기본 명령어 | add, commit, status, diff |
| 03 | 파일 관리 | .gitignore, git rm, git mv |
| 04 | 변경 이력 | log, diff, show, blame |
| 05 | 되돌리기 | reset, restore, revert |
| 06 | 브랜치 | branch, checkout, switch |
| 07 | 병합 | merge, fast-forward, 3-way |
| 08 | 병합 충돌 | 충돌 해결, mergetool |
| 09 | 원격 저장소 | remote, push, pull, fetch |
| 10 | GitHub 기초 | 저장소, Issues, Fork |
| 11 | Pull Request | PR 워크플로, 코드 리뷰 |
| 12 | 태그와 릴리즈 | tag, SemVer, Releases |
| 13 | Stashing | stash 임시 저장 |
| 14 | Rebase | rebase, interactive rebase |
| 15 | Cherry-pick | cherry-pick, revert |
| 16 | GitHub Actions | CI/CD 자동화 |
| 17 | 협업 워크플로우 | GitHub Flow, GitFlow |
| 18 | 고급 기능 | submodule, worktree, bisect |
| 19 | .gitignore와 보안 | 보안, 민감 정보 관리 |
| 20 | 실전 프로젝트 | 전체 워크플로 실습 |

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

## MicroPython 기초 (20개 챕터)

```bash
# Thonny IDE에서 파일 열기 → 실행
# 또는 ampy로 업로드
ampy --port COM3 put main.py
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | MicroPython 소개, REPL, LED 제어 |
| 02 | Buttons | 버튼 입력, 인터럽트, 풀업/풀다운 |
| 03 | LED Display | LED 매트릭스, 7세그먼트, NeoPixel |
| 04 | Joystick | 조이스틱 입력, 아날로그 값 매핑 |
| 05 | Motors | DC 모터, 서보 모터, 스테퍼 모터 |
| 06 | Sensors | 온도/습도/거리 센서, I2C 센서 |
| 07 | NFC | NFC/RFID 리더, MIFARE 카드 |
| 08 | Bluetooth | BLE 통신, 데이터 송수신 |
| 09 | Music | 부저, 멜로디, 사운드 출력 |
| 10 | Games | 간단한 게임 제작, 디스플레이 활용 |
| 11 | Data Logging | CSV 기록, SD 카드, 시계열 데이터 |
| 12 | Robotics | 로봇 팔 제어, 라인 트레이서 |
| 13 | Solar Tracker | 태양광 추적 시스템, 광센서 |
| 14 | Weather Station | 기상 관측소, 센서 융합 |
| 15 | Smart Home | 홈 오토메이션, 릴레이 제어 |
| 16 | Health Monitoring | 심박수/체온 측정, IoT 전송 |
| 17 | Educational Games | 학습용 게임, 퀴즈 프로그램 |
| 18 | Wearables | 웨어러블 디바이스, 저전력 설계 |
| 19 | Internet of Things | MQTT, HTTP 클라이언트, 클라우드 |
| 20 | Artificial Intelligence | TinyML, Edge AI, 센서 데이터 분석 |

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

## React 기초 (20개 챕터)

```bash
cd REACT/01-introduction && npm install && npm run dev
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | React 소개, createRoot, JSX 기본 |
| 02 | JSX | JSX 표현식, 조건부 렌더링, Fragments |
| 03 | Components & Props | 함수 컴포넌트, props, children |
| 04 | State & useState | useState, 상태 업데이트, 배열/객체 |
| 05 | Event Handling | onClick, onChange, onSubmit, 합성 이벤트 |
| 06 | Conditional Rendering | &&, 삼항연산자, if/else 조건 분기 |
| 07 | Lists & Keys | map, filter, key prop, 리스트 필터링 |
| 08 | Forms | 제어 컴포넌트, input/select/checkbox, 검증 |
| 09 | useEffect | useEffect, 의존성 배열, cleanup, 데이터 패칭 |
| 10 | useRef & DOM | useRef, forwardRef, DOM 조작 |
| 11 | Context API | createContext, useContext, Provider 패턴 |
| 12 | useReducer | useReducer, dispatch, 복잡한 상태 |
| 13 | Custom Hooks | useLocalStorage, useFetch, Hook 합성 |
| 14 | React Router | BrowserRouter, Routes, useParams |
| 15 | Styling | Inline styles, 동적 className |
| 16 | Error Handling | ErrorBoundary, try/catch, fallback UI |
| 17 | Performance | React.memo, useMemo, useCallback, Suspense |
| 18 | Portals & Fragments | createPortal, Fragment, 모달 예제 |
| 19 | Testing | React Testing Library, jest, fireEvent |
| 20 | Deployment | 빌드, 환경변수, Netlify/Vercel 배포 |

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

## TypeScript 기초 (20개 챕터)

```bash
cd TYPESCRIPT/01-introduction && npx ts-node index.ts
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | TypeScript 소개, tsc, tsconfig, 기본 타입 |
| 02 | Basic Types | number/string/boolean, tuple, enum, any/unknown/never |
| 03 | Interfaces | interface, optional/readonly, extends, index signature |
| 04 | Types | type alias, union, intersection, literal types |
| 05 | Functions | 매개변수/반환 타입, 오버로드, this |
| 06 | Classes | class, implements, abstract, parameter properties |
| 07 | Generics | 제네릭 함수/클래스/제약, infer |
| 08 | Enums & Type Guards | enum, typeof, instanceof, discriminated union |
| 09 | Utility Types | Partial, Required, Pick, Omit, Record, ReturnType |
| 10 | Modules | export/import, namespace, ambient 선언 |
| 11 | Type Manipulation | keyof, typeof, conditional types, mapped types |
| 12 | Template Literal Types | 템플릿 리터럴, intrinsic string types |
| 13 | Decorators | 클래스/메서드/프로퍼티/파라미터 데코레이터 |
| 14 | Declaration Files | .d.ts, declare, module augmentation |
| 15 | Advanced Types | recursive types, branded types, satisfies |
| 16 | Configuration | tsconfig.json, strict mode, paths, references |
| 17 | React with TS | FC, useState/useRef typing, 이벤트 핸들러 |
| 18 | Node.js with TS | Express, Request/Response, 미들웨어 |
| 19 | Testing | jest, ts-jest, typed mock |
| 20 | Real-world Project | Todo API (Express + TypeScript) |

## Visual Basic .NET 기초 (20개 챕터)

```bash
cd VB/01-hello-world && dotnet run
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 입출력, Module, Sub Main, 주석 |
| 02 | Variables | 데이터 타입, Dim/Const, 형변환, Option Strict |
| 03 | Control Flow | If/ElseIf/Else, Select Case, For/While/Do/Loop |
| 04 | Arrays & Collections | Array, ReDim, List(Of T), Dictionary(Of K,V) |
| 05 | Functions & Subs | Sub, Function, ByVal/ByRef, Optional, ParamArray |
| 06 | Strings | 문자열 함수, StringBuilder, 보간, Format |
| 07 | Classes & Objects | Class, Property, Constructor(Sub New), Shared |
| 08 | Inheritance | Inherits, Overridable/Overrides, MustInherit |
| 09 | Interfaces | Interface, Implements, 다중 인터페이스 |
| 10 | Exceptions | Try/Catch/Finally, Throw, 사용자 정의 예외 |
| 11 | LINQ | LINQ 쿼리 구문, 메서드 구문, Aggregate |
| 12 | Generics | 제네릭 클래스/메서드, Of T, 제약 조건 |
| 13 | File I/O | StreamReader/Writer, File, My.Computer.FileSystem |
| 14 | Date & Time | Date, TimeSpan, 날짜 연산, 서식 |
| 15 | Delegates & Events | Delegate, Event, RaiseEvent, Handles, AddHandler |
| 16 | Async & Await | Async/Await, Task, 비동기 프로그래밍 |
| 17 | XML & JSON | LINQ to XML, System.Text.Json |
| 18 | Database | ADO.NET, SqlConnection, SqlCommand |
| 19 | Reflection & Attributes | Attribute, Reflection, CallByName |
| 20 | Unit Testing | MSTest, Assert, DataRow, TestInitialize |

## Arduino 기초 (20개 챕터)

```bash
# Arduino IDE에서 .ino 파일 열어서 업로드
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | Arduino IDE, setup/loop, LED blink |
| 02 | Digital I/O | digitalRead, 버튼 입력, 풀업 저항 |
| 03 | Analog Input | analogRead, 가변저항, map() |
| 04 | PWM | analogWrite, LED fade, PWM 핀 |
| 05 | Serial | Serial.begin, Serial.print/read |
| 06 | Conditional | if/else, 디바운싱, 상태 변화 감지 |
| 07 | Loops | for/while, LED 패턴, Knight Rider |
| 08 | Functions | 함수 정의, 파라미터, 리턴값 |
| 09 | Arrays | 핀 배열, LED 시퀀스, 패턴 제어 |
| 10 | LCD Display | I2C LCD, 문자 출력, LiquidCrystal |
| 11 | Servo | Servo 라이브러리, sweep, 위치 제어 |
| 12 | Ultrasonic | HC-SR04, pulseIn, 거리 측정 |
| 13 | DHT Sensor | DHT11 온습도 센서 |
| 14 | IR Remote | IR 리모컨 수신, 코드 매핑 |
| 15 | DC Motor | L298N, 모터 속도/방향 제어 |
| 16 | Interrupts | attachInterrupt, ISR, volatile |
| 17 | EEPROM | EEPROM 읽기/쓰기, 구조체 저장 |
| 18 | Timers | millis(), Blink Without Delay |
| 19 | I2C | Wire 라이브러리, Master/Slave |
| 20 | IoT (ESP) | ESP8266 WiFi, HTTP 요청 |

## Vue.js 기초 (20개 챕터)

```bash
# Vite 개발 서버 실행 (cd VUE && npm install && npx vite serve .)
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | Vue.js 소개, createApp, v-bind |
| 02 | Template Syntax | 보간법, v-once, v-html |
| 03 | Data & Event Binding | data, methods, computed, v-on, v-bind |
| 04 | Computed & Watch | computed getter/setter, watch, deep |
| 05 | Class & Style Binding | :class 객체/배열, :style 객체/배열 |
| 06 | Conditional Rendering | v-if/v-else-if/v-else, v-show |
| 07 | List Rendering | v-for 배열/객체/범위, 필터링, 정렬 |
| 08 | Event Handling | 이벤트 수식어, 키 수식어, 마우스 수식어 |
| 09 | Form Input Binding | v-model 모든 타입, .lazy/.number/.trim |
| 10 | Components | 컴포넌트 등록, props, emit, slot |
| 11 | Component Props | props 타입 검증, Props Drilling |
| 12 | Component Emits | $emit, 커스텀 v-model, emit payload |
| 13 | Slots | 기본 slot, named slot, scoped slot |
| 14 | Lifecycle Hooks | onMounted, onUnmounted, KeepAlive |
| 15 | Composition API | ref, reactive, setup, onMounted |
| 16 | ref & reactive | toRefs, shallowRef, reactive 주의사항 |
| 17 | Computed & Watch (CA) | watch 다중 값, watchEffect |
| 18 | provide & inject | 의존성 주입 (테마 시스템) |
| 19 | Router | vue-router, createWebHistory, $route.params |
| 20 | Composition Patterns | useCounter, useToggle, useLocalStorage |

## WebAssembly 기초 (20개 챕터)

```bash
cd WASM/01-introduction
wat2wasm add.wat -o add.wasm
npx http-server .
# http://localhost:8080 접속
```

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | WASM 소개, 모듈 구조, 첫 WASM 예제 |
| 02 | WAT Basics | S-Expression 문법, 섹션 구조 |
| 03 | Types & Operators | i32/i64/f32/f64, 산술/비트/비교 연산 |
| 04 | Variables | local 변수, global 변수, mut |
| 05 | Memory | 선형 메모리, load/store, data 세그먼트 |
| 06 | Functions | 함수 정의, 매개변수, 반환값, 재귀 |
| 07 | Control Flow | block/loop/if/else/br/br_if/br_table |
| 08 | Stack | 스택 머신 동작, drop/select/tee |
| 09 | Import & Export | import/export 섹션, 모듈 간 인터페이스 |
| 10 | JS Interop | JS ↔ WASM 함수 호출, 메모리 공유 |
| 11 | Call JS from WASM | import로 JS 함수 호출, 콜백 패턴 |
| 12 | Memory Management | memory.grow/copy/fill, 동적 메모리 |
| 13 | WABT Tooling | wat2wasm, wasm2wat, wasm-objdump, wasm-interp |
| 14 | Emscripten (C) | C → WASM, EMSCRIPTEN_KEEPALIVE |
| 15 | Emscripten (C++) | C++ → WASM, Embind, 클래스 바인딩 |
| 16 | Rust + WASM | wasm-pack, wasm-bindgen, cargo |
| 17 | AssemblyScript | TypeScript → WASM, AS 컴파일러 |
| 18 | Debugging | Chrome DevTools, wasm-objdump, 소스맵 |
| 19 | WASI | WebAssembly System Interface, Wasmtime |
| 20 | Real-world Project | 이미지 필터 (grayscale/invert/threshold) |

## WPF 완벽 강좌 (20개 챕터)

```bash
# .NET 8 SDK 확인
dotnet --version

# 새 WPF 프로젝트 생성
dotnet new wpf -n MyWpfApp

# 실행
cd MyWpfApp
dotnet run
```

| 장 | 제목 | 설명 |
|----|------|------|
| 00 | 개발 환경 설정 | Visual Studio 설치, .NET SDK, WPF 워크로드 |
| 01 | Hello, WPF! | 첫 번째 WPF 애플리케이션 만들기 |
| 02 | 레이아웃 | Grid, StackPanel, WrapPanel, DockPanel |
| 03 | 컨트롤 | Button, TextBox, Slider, ProgressBar 등 |
| 04 | 이벤트 | 라우티드 이벤트, 버블링, 터널링 |
| 05 | 데이터 바인딩 | {Binding}, DataContext, INotifyPropertyChanged |
| 06 | 커맨드 | ICommand, RelayCommand, CommandBinding |
| 07 | 스타일 | Style, Setter, TargetType, BasedOn |
| 08 | 템플릿 | ControlTemplate, DataTemplate |
| 09 | 트리거 | PropertyTrigger, EventTrigger, MultiTrigger |
| 10 | 리소스 | Resources, ResourceDictionary |
| 11 | MVVM 패턴 | Model-View-ViewModel 아키텍처 |
| 12 | 컬렉션 | ObservableCollection, ListBox, ListView |
| 13 | 데이터 그리드 | DataGrid, 열 템플릿 |
| 14 | 대화상자 | MessageBox, OpenFileDialog, 사용자 정의 대화상자 |
| 15 | 내비게이션 | Frame, Page, NavigationService |
| 16 | 멀티스레딩 | Dispatcher, async/await, Task |
| 17 | 사용자 정의 컨트롤 | UserControl, DependencyProperty |
| 18 | 애니메이션 | Storyboard, DoubleAnimation, ColorAnimation |
| 19 | 스타일 및 테마 | ResourceDictionary, 테마 전환 |
| 20 | 배포 | ClickOnce, 단일 파일 게시 |
