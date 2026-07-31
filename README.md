# Basic CS - 프로그래밍 기초 + 중급 강의 자료

Arduino, C, C#, FastAPI, Git & GitHub, Java, MicroPython, Node.js, PHP, Python, React, Rust, TypeScript, VB.NET, Vue.js, WebAssembly, WPF 프로그래밍 언어의 기초와 중급 개념을 학습할 수 있는 예제 모음입니다.

## 구조

```
basic_cs/
├── ARDUINO/     Arduino 기초 + 중급 강의 (40개 챕터)
├── C/           C 기초 + 중급 강의 (40개 챕터)
├── CS/          C# 기초 + 중급 강의 (40개 챕터)
├── FASTAPI/     FastAPI 기초 + 중급 강의 (40개 챕터)
├── GIT/         Git & GitHub 기초 + 중급 강의 (40개 챕터)
├── JAVA/        Java 기초 + 중급 강의 (40개 챕터)
├── MP/          MicroPython 기초 + 중급 (40개 챕터)
├── NODEJS/      Node.js 기초 + 중급 강의 (40개 챕터)
├── PHP/         PHP 기초 + 중급 강의 (40개 챕터)
├── PYTHON/      Python 기초 + 중급 강의 (40개 챕터)
├── REACT/       React 기초 + 중급 강의 (40개 챕터)
├── RUST/        Rust 기초 + 중급 강의 (40개 챕터)
├── TYPESCRIPT/  TypeScript 기초 + 중급 강의 (40개 챕터)
├── VB/          Visual Basic .NET 기초 + 중급 강의 (40개 챕터)
├── VUE/         Vue.js 기초 + 중급 강의 (40개 챕터)
├── WASM/        WebAssembly 기초 + 중급 (40개 챕터)
└── WPF/         WPF 기초 + 중급 강의 (40개 챕터)
```

## C 기초 + 중급 (40개 챕터)

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

## C# 기초 + 중급 (40개 챕터)

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

## FastAPI 기초 + 중급 (40개 챕터)

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

## Git & GitHub 기초 + 중급 (40개 챕터)

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

## Java 기초 + 중급 (40개 챕터)

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

## MicroPython 기초 + 중급 (40개 챕터)

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

## PHP 기초 + 중급 (40개 챕터)

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

## Node.js 기초 + 중급 (40개 챕터)

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

## Python 기초 + 중급 (40개 챕터)

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

## React 기초 + 중급 (40개 챕터)

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

## Rust 기초 + 중급 (40개 챕터)

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

## TypeScript 기초 + 중급 (40개 챕터)

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

## Visual Basic .NET 기초 + 중급 (40개 챕터)

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

## Arduino 기초 + 중급 (40개 챕터)

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

## Vue.js 기초 + 중급 (40개 챕터)

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

## WebAssembly 기초 + 중급 (40개 챕터)

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
## Python 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Advanced OOP | dataclass, __slots__, 추상 클래스, 매직 메서드 |
| 22 | Type Hints | TypedDict, Protocol, NewType, mypy |
| 23 | Context Managers | contextlib, @contextmanager, async context |
| 24 | Threads & GIL | Thread, Lock, Queue, 동기화 |
| 25 | Multiprocessing | Process, Pool, ProcessPoolExecutor, IPC |
| 26 | Asyncio | async/await, Task, gather, asyncio.run |
| 27 | Async I/O | aiohttp/httpx 비동기 클라이언트, 로컬 서버 |
| 28 | Metaclasses | type(), __new__, 커스텀 메타클래스 |
| 29 | Descriptors | __get__/__set__, property 내부 동작 |
| 30 | Functional | itertools, functools.partial, curry |
| 31 | Data Formats | csv, openpyxl, configparser, pickle |
| 32 | Web Scraping | requests, BeautifulSoup, robots.txt |
| 33 | Advanced Regex | 전방탐색, 역참조, 플래그, re 패턴 |
| 34 | Performance | cProfile, timeit, lru_cache, 코드 최적화 |
| 35 | Memory | gc, weakref, __slots__, 순환 참조 |
| 36 | Networking | socket, TCP/UDP 클라이언트-서버 |
| 37 | Database | sqlite3, SQLAlchemy Core/ORM 기초 |
| 38 | GUI | tkinter 기본 위젯, 이벤트 |
| 39 | Advanced Testing | pytest fixture, parametrize, mock |
| 40 | Mini Project | 명령줄 할일 관리 앱 |

## Java 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Advanced Collections | 고급 컬렉션, Comparator, groupingBy, parallelStream |
| 22 | Optional & Functional | Optional 심화, Supplier/BiFunction, 커링 |
| 23 | Modern Java | record, sealed class, pattern matching, switch 패턴 |
| 24 | Concurrency & Executors | ExecutorService, CompletableFuture, ForkJoin |
| 25 | Virtual Threads | 가상 스레드, VirtualThread, StructuredTaskScope |
| 26 | Memory & GC | JMM, 힙/스택, WeakReference, GC 알고리즘 |
| 27 | Reflection & Proxy | 리플렉션, Method.invoke, 동적 프록시, InvocationHandler |
| 28 | Annotations | 커스텀 어노테이션, Retention/Target, 리플렉션 활용 |
| 29 | JDBC Advanced | 트랜잭션, 배치, DAO/Repository 패턴 |
| 30 | JPA Basics | 엔티티, 영속성 컨텍스트, JPQL, Hibernate 개념 |
| 31 | Spring Core | IoC/DI, Bean, AOP 개념 |
| 32 | Spring Boot | REST 컨트롤러, 계층 구조, 어노테이션 매핑 |
| 33 | Spring Security | JWT 인증, 인증/인가, 필터 체인 |
| 34 | Testing Advanced | Mockito, AssertJ 체이닝, 파라미터 테스트 |
| 35 | Microservices | REST 통신, 서비스 분리, 서킷 브레이커 |
| 36 | Messaging & Kafka | 프로듀서/컨슈머 패턴, 토픽/파티션, JDK 큐 구현 |
| 37 | Performance | JMH 개념, 프로파일링, 컬렉션 최적화 |
| 38 | Build Tools | Maven/Gradle 개념, 디렉터리 구조, 의존성 관리 |
| 39 | Design Patterns | 싱글턴, 팩토리, 전략, 옵저버 구현 |
| 40 | Final Project | 콘솔 기반 할일 관리 앱 (종합 프로젝트) |

## C 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
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

## PHP 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Advanced OOP | 트레이트, 익명 클래스, 매직 메서드, 객체 복사 |
| 22 | Design Patterns | 싱글턴, 팩토리, 전략, 옵저버, 의존성 주입 |
| 23 | PSR & Composer | autoload, PSR-4, 버전 제약, 시맨틱 버저닝 |
| 24 | Advanced PDO | 트랜잭션, prepared statement, Repository 패턴 |
| 25 | Doctrine ORM | 엔티티 매핑, 영속성 컨텍스트, UnitOfWork (시뮬레이션) |
| 26 | Symfony Components | 라우터, Console 컴포넌트 개념 |
| 27 | Laravel Basics | 설치, 라우팅, 컨트롤러, 블레이드 개념 |
| 28 | Eloquent ORM | 모델, 접근자/뮤테이터, 관계(1:N, N:M) |
| 29 | Auth | 세션 인증, Sanctum 토큰, JWT 개념 |
| 30 | REST API | 엔드포인트 설계, 상태 코드, 버저닝 |
| 31 | Testing | PHPUnit 스타일 어설션, 미니 테스트 러너 |
| 32 | Security | password_hash, CSRF, XSS, SQL 인젝션 방어 |
| 33 | Caching | OPcache, 파일 캐시, Redis 개념 |
| 34 | Queues & Jobs | 작업 큐 패턴, 워커, 재시도/백오프 |
| 35 | WebSocket | Ratchet 개념, 핸드셰이크, 채팅 구조 |
| 36 | Performance | 벤치마크, 지연 로딩, 프로파일링 |
| 37 | Deployment | Docker, Nginx + PHP-FPM, 환경 변수 |
| 38 | GraphQL | 스키마, 쿼리/리졸버 개념 |
| 39 | SOLID | 단일 책임, 개방-폐쇄 등 5원칙 |
| 40 | Final Project | CLI 기반 작업 관리 앱 |

## Node.js 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Express Advanced | 라우터 모듈화, 미들웨어 패턴, 오류 미들웨어 |
| 22 | Event Emitter | EventEmitter, 커스텀 이벤트, once |
| 23 | Design Patterns | 싱글턴, 팩토리, 의존성 주입 컨테이너 |
| 24 | MongoDB & Mongoose | 문서 모델, 스키마, CRUD 개념 |
| 25 | Redis | 캐시, pub/sub 개념 |
| 26 | Advanced REST API | 검증, 에러 응답, 버저닝 패턴 |
| 27 | GraphQL | 스키마, 리졸버 개념 |
| 28 | WebSocket Advanced | Socket.IO 개념, 실시간 채팅 구조 |
| 29 | Advanced Auth | JWT, 리프레시 토큰, OAuth2 개념 |
| 30 | Authorization | RBAC, ACL 권한 관리 |
| 31 | Security | helmet, rate limiting, 입력 검증, crypto |
| 32 | Advanced Testing | node:test, assert 테스트 구조 |
| 33 | Clustering | cluster, worker_threads, 로드 밸런싱 |
| 34 | Observability | 로깅, 요청 추적, 성능 메트릭 |
| 35 | Docker | Dockerfile, compose 개념 |
| 36 | Microservices | 서비스 분리, HTTP 통신, health check |
| 37 | Queues & Jobs | BullMQ 개념, core 기반 큐 구현 |
| 38 | TypeScript + Node | 타입 안전 서버 구조 |
| 39 | Advanced Streams | 파이프라인, backpressure, transform |
| 40 | Final Project | CLI 작업 관리 도구 |

## TypeScript 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Advanced Generics | 타입 추론, 제약, infer 패턴 |
| 22 | Type System Deep | 구조적 타이핑, 공변성/반공변성 |
| 23 | Conditional & Mapped | 분배 법칙, 재귀 타입, 키 재매핑 |
| 24 | Template Literals Deep | 문자열 파싱, CamelCase 변환 |
| 25 | Utility Type Design | Partial, Pick, ReturnType 직접 구현 |
| 26 | Type-safe APIs | zod 스키마, tRPC 개념, 미니 검증기 |
| 27 | Decorators Deep | 메서드 데코레이터, DI 컨테이너, 싱글턴 |
| 28 | Monorepo | 프로젝트 레퍼런스, workspace, 위상 정렬 |
| 29 | Build Tools | tsc vs esbuild/swc, 증분 빌드, paths |
| 30 | FP & Pipeline | Option/Either 패턴, pipe, compose |
| 31 | Express + TS | 타입 안전 라우터, 제네릭 핸들러 |
| 32 | GraphQL + TS | 스키마, 리졸버, 미니 GraphQL 엔진 |
| 33 | Testing TS | 테스트 러너, 타입 테스팅(tsd) |
| 34 | React Generics | 다형성 컴포넌트, 제네릭 훅 |
| 35 | Node + TS Advanced | 워커, 스트림, 배압(backpressure) |
| 36 | Type-safe ORM | Prisma/Drizzle 개념, DTO 변환 |
| 37 | Module Systems | ESM/CJS 상호운용, createRequire |
| 38 | Package Authoring | .d.ts 배포, SemVer, 의존성 범위 |
| 39 | Events & State Machines | 이벤트 맵, 타입 안전 FSM |
| 40 | Final Project | 타입 안전 할일 관리 CLI |

## React 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Advanced Hooks | 커스텀 훅 합성, 훅 규칙, useMemo/useCallback |
| 22 | State Management | Context + useReducer 대형 상태 관리 |
| 23 | Zustand / Redux Toolkit | 외부 상태 라이브러리, Provider 패턴 |
| 24 | Data Fetching | TanStack Query, useQuery/useMutation |
| 25 | Advanced Router | 중첩 라우트, 인증 가드, useNavigate |
| 26 | Forms Advanced | React Hook Form + Zod 검증 |
| 27 | Performance | React.memo, 코드 스플리팅, Profiler |
| 28 | Suspense | lazy, Suspense, useTransition |
| 29 | Advanced Testing | MSW, user-event, e2e 개념 |
| 30 | Next.js Basics | SSR, SSG, ISR 개념 |
| 31 | Next.js App Router | 서버/클라이언트 컴포넌트 |
| 32 | Accessibility | ARIA, 키보드 내비게이션 |
| 33 | Security | XSS 방어, dangerouslySetInnerHTML, CSRF |
| 34 | Animations | CSS transitions, Framer Motion |
| 35 | TypeScript + React | 제네릭 컴포넌트, 이벤트 타입 |
| 36 | State Machines | XState, 유한 상태 머신 |
| 37 | Realtime | WebSocket 채팅 UI, Socket.IO |
| 38 | Design Systems | Storybook, 컴포넌트 API |
| 39 | PWA | Service Worker, offline, manifest |
| 40 | Final Project | 할일 관리 앱 전체 통합 |

## Vue.js 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Composition Advanced | Composition API 심화, 라이프사이클, ref vs reactive, 함수 분리 |
| 22 | Pinia | 상태 관리, store, actions, getters |
| 23 | Router Advanced | 라우터 심화, 가드, lazy loading, 메타 필드 |
| 24 | Forms Validation | 폼 검증, VeeValidate, 커스텀 규칙 |
| 25 | HTTP Axios | HTTP 통신, Axios 인터셉터, API 레이어 패턴 |
| 26 | TypeScript + Vue | TypeScript, script setup 타입, props 타입 |
| 27 | Reusable Components | 재사용 컴포넌트, v-model 패턴, composable props |
| 28 | Teleport & Suspense | Teleport, Suspense, 모달, 비동기 컴포넌트 |
| 29 | Render Functions | 렌더 함수, h(), VNode, JSX |
| 30 | Custom Directives | 커스텀 디렉티브, v-focus, v-click-outside |
| 31 | Plugins | 플러그인 개발, app.use, provide/inject |
| 32 | Testing | 테스팅, Vitest, Vue Test Utils, e2e 개념 |
| 33 | Performance | 성능 최적화, defineAsyncComponent, memoization, v-memo |
| 34 | Transitions & Animations | 전환과 애니메이션, Transition, TransitionGroup |
| 35 | SSR & Nuxt | SSR 개념, hydration, Nuxt 시작 |
| 36 | Nuxt Advanced | Nuxt 심화, data fetching, middleware, layouts |
| 37 | Composables Deep | 컴포저블 심화, useMouse, useFetch 구현, 패턴 |
| 38 | Accessibility | 접근성, ARIA, 포커스 관리, 키보드 |
| 39 | Security | 보안, XSS, v-html 위험, CSP, 인증 가드 |
| 40 | Final Project | 종합 프로젝트, 대시보드 앱 (전체 통합) |

## FastAPI 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Pydantic v2 | computed_field, model_validator, 재사용 모델 |
| 22 | SQLAlchemy 고급 | relationship, 조인, 비동기 엔진 |
| 23 | Alembic | 마이그레이션 개념, revision, upgrade |
| 24 | 고급 보안 | RBAC, 리프레시 토큰, 토큰 저장 |
| 25 | 페이지네이션 | Page/Query 검증, 커서 기반 페이징 |
| 26 | 파일 처리 | 청크 업로드, 스트리밍, 검증 |
| 27 | 백그라운드 작업 | Celery/ARQ 개념, InProcess 큐 |
| 28 | 속도 제한 | RateLimiter 미들웨어 구현 |
| 29 | 멀티테넌시 | 테넌트 분리, 헤더 기반 라우팅 |
| 30 | 고급 테스팅 | pytest-asyncio, monkeypatch, coverage |
| 31 | OpenAPI 커스터마이징 | 태그, 메타데이터, 커스텀 문서 |
| 32 | 모니터링 | structlog, Prometheus 메트릭 개념 |
| 33 | Docker 고급 | 멀티스테이지 빌드, docker-compose |
| 34 | 배포 | Gunicorn/Uvicorn 워커, Nginx, systemd |
| 35 | GraphQL | Strawberry 기초, 스키마, 리졸버 |
| 36 | WebSocket 고급 | 룸, 인증, 재연결 |
| 37 | SSE | 이벤트 스트리밍, 실시간 푸시 |
| 38 | 마이크로서비스 | httpx 호출 패턴, 디스커버리 |
| 39 | 대용량 스트리밍 | StreamingResponse, 청크 전송 |
| 40 | 프로덕션급 API | 종합 프로젝트 |

## Arduino 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | 고급 센서 | BMP280 기압, MPU6050 가속도/자이로 |
| 22 | SPI | SPI 핀, 시프트 레지스터, SD 카드 |
| 23 | SD 카드 | 파일 로깅, CSV 저장, 타임스탬프 |
| 24 | 블루투스 | HC-05/HM-10 AT 명령, 데이터 송수신 |
| 25 | WiFi | ESP8266/ESP32 HTTP 클라이언트/서버 |
| 26 | MQTT | pub/sub, 브로커 연결, 토픽 관리 |
| 27 | 실시간 시계 | DS3231, 시간 동기화, 알람 |
| 28 | 스테퍼 모터 | A4988 드라이버, 스텝 제어 |
| 29 | 오디오 | DFPlayer, 멜로디 생성, 볼륨 제어 |
| 30 | OLED/TFT | I2C OLED, 그래픽, 메뉴 UI |
| 31 | GPS | NMEA 파싱, 위치/속도/시간 |
| 32 | 전원 관리 | 딥 슬립, 인터럽트 웨이크, 배터리 |
| 33 | PID | 온도/모터 PID 루프 |
| 34 | 상태 머신 | enum 기반 FSM, 시나리오 설계 |
| 35 | 멀티태스킹 | millis 스케줄러, 비블로킹 패턴 |
| 36 | 데이터 수집 | 시리얼 플로터, CSV, 샘플링 |
| 37 | 보안 | XOR 암호화, 키 저장, 인증 토큰 |
| 38 | ESP32 심화 | 웹 서버, OTA, FreeRTOS 개념 |
| 39 | IoT 클라우드 | HTTP POST, 대시보드 연동 개념 |
| 40 | 종합 프로젝트 | 기상 관측소 / 로봇 제어 |

## MicroPython 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | I2C Advanced | 고급 I2C, 다중 디바이스, 스캔, 레지스터 직접 접근 |
| 22 | SPI | SPI 통신, SPI 기기, 비트뱅킹, OLED |
| 23 | BLE Advanced | GATT 서비스, 커스텀 캐릭터리스틱, 노티피케이션 |
| 24 | MQTT Advanced | QoS, retained, TLS, last will |
| 25 | File Systems | LittleFS, 데이터 영속화, 파일 구조 |
| 26 | Networking | TCP 소켓 서버/클라이언트, DNS |
| 27 | uasyncio | 비동기 태스크, 이벤트 루프 |
| 28 | State Machine | 이벤트 기반 설계, HSM |
| 29 | Sensor Fusion | 이동 평균, 칼만 필터 기초 |
| 30 | PWM Advanced | 서보, 사운드, LED 밝기 곡선 |
| 31 | Display Graphics | framebuffer, 도형, 폰트 |
| 32 | Power Management | 딥 슬립, 주기 웨이크, 전류 최적화 |
| 33 | LoRa | LoRa 모듈, 게이트웨이, 장거리 통신 |
| 34 | Edge AI | TensorFlow Lite Micro 개념, 센서 분류 |
| 35 | Security | 암호화, 키 관리, 보안 부팅 개념 |
| 36 | RTOS Concepts | 태스크, 큐, 세마포어 (uasyncio 관점) |
| 37 | Multi-Board | ESP32 + Pico 통신, 프레임워크 |
| 38 | Web Server | MicroWebSrv 유사 구현, REST |
| 39 | OTA | 원격 펌웨어 업데이트 개념 |
| 40 | Final Project | IoT 환경 모니터링 시스템 |

## Git & GitHub 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | 브랜치 전략 | trunk-based, GitFlow, feature branch |
| 22 | Actions 워크플로우 | 워크플로우, 이벤트, 잡 구조 |
| 23 | Actions 고급 | 매트릭스 빌드, 재사용 워크플로우 |
| 24 | Actions 배포 | environments, secrets, 승인 |
| 25 | Git 훅 | pre-commit, commit-msg, 커스텀 훅 |
| 26 | Git 내부 | objects, refs, HEAD, packfiles |
| 27 | 고급 리베이스 | rerere, autosquash, fixup |
| 28 | bisect 디버깅 | 이진 탐색, 자동 실행 |
| 29 | 서브모듈 | submodule, subtree |
| 30 | 워크트리 | 병렬 작업, 컨텍스트 전환 |
| 31 | 히스토리 편집 | filter-branch, filter-repo |
| 32 | CI/CD 설계 | 파이프라인, 게이트, 아티팩트 |
| 33 | GitHub API | gh CLI, REST API, 자동화 |
| 34 | 팀 워크플로 | 보호된 브랜치, 코드 리뷰 |
| 35 | 릴리즈 엔지니어링 | SemVer, changelog, 태그 |
| 36 | Git 보안 | 서명 커밋, 시크릿 스캔 |
| 37 | 모노레포 | 전략, 툴, 경로 제한 |
| 38 | Git 성능 | LFS, shallow, partial clone |
| 39 | 멀티레포 | 저장소 간 자동화 |
| 40 | 실전 프로젝트 | CI/CD 전체 파이프라인 |

## WebAssembly 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Tables & Indirect | table, elem, call_indirect, 함수 포인터 |
| 22 | Bulk Memory | memory.copy/fill, 수동 데이터 세그먼트 |
| 23 | Reference Types | externref, funcref, ref.null |
| 24 | SIMD | v128, 128비트 벡터 연산, 정수/부동소수 |
| 25 | Multi-value | 다중값 반환, 다중 메모리 |
| 26 | Exception Handling | try/catch, throw, tag |
| 27 | Threads | shared memory, atomic 연산, worker |
| 28 | Advanced JS Interop | 공유 메모리, 객체 변환, 성능 패턴 |
| 29 | Emscripten FS | FS, IDBFS, MEMFS |
| 30 | Emscripten Advanced | pthreads, Optimize 플래그 |
| 31 | Rust + WASM Advanced | wasm-bindgen, 파서 성능 |
| 32 | AssemblyScript Advanced | 메모리 관리, 라이브러리 |
| 33 | Wasmtime/WASI Advanced | CLI, 리소스 제한 |
| 34 | Component Model | wit, 인터페이스, 합성 |
| 35 | Edge Runtime | Cloudflare Workers, 모듈 연동 |
| 36 | Plugins & Sandbox | Extism, Wasmer, 보안 격리 |
| 37 | Performance | 벤치마킹, 크기 최적화, 메모리 튜닝 |
| 38 | Advanced Debugging | DWARF, 소스맵, Chrome DevTools |
| 39 | Security | 검증, CSP, 메모리 안전 |
| 40 | Final Project | 이미지 필터/계산기 앱 |

## Rust 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | 고급 트레잇 | 연관 타입, 제네릭 트레잇, 상속 트레잇, 트레잇 객체 |
| 22 | 고급 패턴 매칭 | 매치 가드, @바인딩, 구조 분해 심화 |
| 23 | 반복자 심화 | 어댑터 체인, Iterator 직접 구현 |
| 24 | 제네릭 심화 | const generics, 고정 길이 배열 |
| 25 | 에러 처리 패턴 | 커스텀 Error, Result 체이닝, context |
| 26 | 비동기 | async/await, Future 개념, 폴링 이벤트 루프 |
| 27 | 네트워킹 | TcpListener/TcpStream, HTTP 요청 개념 |
| 28 | 웹 프레임워크 | Axum/Actix 개념, 미니 라우터 구현 |
| 29 | 데이터베이스 | sqlx/Diesel 개념, 파일 기반 저장 |
| 30 | 직렬화 | serde 개념, 자체 직렬화 구현 |
| 31 | FFI | extern "C", unsafe, C 바인딩 개념 |
| 32 | 프로시저 매크로 | derive 매크로 개념, 매크로로 구현 재현 |
| 33 | Rust + WASM | wasm-bindgen 개념, 내보낼 함수 작성 |
| 34 | 고급 동시성 | Arc, mpsc channel, AtomicU64 |
| 35 | 성능 최적화 | 벤치마킹, SIMD 개념, 최적화 |
| 36 | 임베디드 Rust | no_std, cortex-m 개념, 가상 MCU 시뮬레이션 |
| 37 | 고급 테스팅 | 속성 테스트(proptest 개념), 벤치마크 |
| 38 | CLI 애플리케이션 | clap 개념, 인자 파싱 직접 구현 |
| 39 | 디자인 패턴 | 전략, 옵저버, 빌더 구현 |
| 40 | 종합 프로젝트 | CLI 할일 관리 앱 (파일 저장) |

## C# 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Advanced LINQ | Expression 트리, 커스텀 연산자, GroupJoin |
| 22 | Dependency Injection | 수동 DI 컨테이너, 생성자 주입, 생명주기 |
| 23 | Entity Framework | ORM 개념, 엔티티, 인메모리 리포지토리 |
| 24 | Advanced Async | ValueTask, IAsyncEnumerable, CancellationToken |
| 25 | Records & Pattern Matching | positional record, with, property pattern |
| 26 | Source Generators | partial, Incremental Generator 개념 |
| 27 | Span & Memory | 저할당 코드, 슬라이싱, Memory<T> |
| 28 | Performance | Stopwatch 벤치마크, 컬렉션 선택 |
| 29 | Minimal API | ASP.NET Core 라우팅, 필터 개념 |
| 30 | gRPC | 프로토콜, proto 파일, 스트리밍 |
| 31 | SignalR | 허브 개념, 그룹, 실시간 통신 |
| 32 | Caching | IMemoryCache 개념, LRU 구현 |
| 33 | Logging | Serilog 개념, 커스텀 로거 |
| 34 | Background Services | IHostedService, Channel 큐 |
| 35 | Microservices | HTTP 통신, 서비스 레지스트리 패턴 |
| 36 | Memory Management | IDisposable, using, GC 개념 |
| 37 | Advanced Collections | 불변 컬렉션, Channel, PriorityQueue |
| 38 | Functional C# | Option/Either, 파이프라인 |
| 39 | Native Interop | P/Invoke, DllImport, 마샬링 |
| 40 | Final Project | 콘솔 할일 관리 앱 (파일 저장) |

## Visual Basic .NET 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Advanced OOP | 제네릭 심화, 중첩 타입, 이벤트 상속 |
| 22 | Advanced LINQ | GroupJoin, Aggregate, Expression 트리 |
| 23 | Advanced Async | ValueTask, IAsyncEnumerable, Progress(Of T) |
| 24 | ADO.NET Advanced | DataAdapter, DataSet, 트랜잭션 |
| 25 | Entity Framework | DbContext, Code First, 마이그레이션 |
| 26 | WPF + MVVM | MVVM 패턴, INotifyPropertyChanged |
| 27 | WinForms Advanced | 사용자 컨트롤, 데이터 바인딩 |
| 28 | Design Patterns | 싱글턴, 팩토리, 전략, 옵저버 |
| 29 | COM Interop | COM 인터페이스, Marshal, P/Invoke |
| 30 | XML/JSON Advanced | LINQ to XML, System.Text.Json |
| 31 | Networking | TcpClient/Listener, HttpClient |
| 32 | Parallel | Parallel.For, PLINQ, 동기화 |
| 33 | Generics Advanced | 제약 조건, 공변성/반공변성 |
| 34 | Extension & Partial | 확장 메서드, Partial 클래스 |
| 35 | Reflection Advanced | Type, Activator, 동적 호출 |
| 36 | Performance | StringBuilder, 컬렉션 튜닝, GC |
| 37 | Testing Advanced | xUnit/NUnit 개념, mock, 파라미터 테스트 |
| 38 | Localization | 리소스 파일, CultureInfo, 다국어 |
| 39 | Windows Services | ServiceBase 개념, 설치 |
| 40 | Final Project | 콘솔 할일 관리 앱 (파일 저장) |

## WPF 중급 (20개 챕터)

| # | 주제 | 설명 |
|---|------|------|
| 21 | MVVM 심화 | Messenger, Mediator 패턴 |
| 22 | 고급 데이터 바인딩 | MultiBinding, PriorityBinding, UpdateSourceTrigger |
| 23 | 커스텀 컨트롤 | ControlTemplate, parts, ThemeInfo |
| 24 | 첨부 속성 | AttachedProperty, 컨테이너별 값 |
| 25 | 비헤이비어 | Behavior/TriggerAction 개념 |
| 26 | 비동기 커맨드 | AsyncRelayCommand, 취소 |
| 27 | 유효성 검사 | IDataErrorInfo, INotifyDataErrorInfo, ValidationRule |
| 28 | 컨버터 | IValueConverter, MultiValueConverter, Parameter |
| 29 | DataGrid 고급 | 편집, 그룹핑, 열 템플릿 |
| 30 | 가상화와 성능 | VirtualizingStackPanel, 병렬 디자인 |
| 31 | 동적 리소스와 테마 | ResourceDictionary, 다크/라이트 테마 |
| 32 | 대화상자 프레임워크 | 커스텀 다이얼로그, MVVM 친화적 |
| 33 | 고급 내비게이션 | Frame/Page, MVVM 내비게이션 |
| 34 | 멀티스레딩 심화 | TPL Dataflow, async/await UI |
| 35 | 의존성 주입 | DI 컨테이너, MVVM 결합 |
| 36 | 지역화 | 리소스, CultureInfo, 다국어 |
| 37 | 커스텀 패널 | MeasureOverride/ArrangeOverride |
| 38 | 미디어와 그래픽 | DrawingVisual, RenderTargetBitmap, 효과 |
| 39 | WPF 테스팅 | MVVM 단위 테스트, UI 자동화 개념 |
| 40 | 종합 프로젝트 | MVVM + DI + 테마의 완성된 앱 |

