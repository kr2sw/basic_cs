# Basic CS - 프로그래밍 기초 강의 자료

C#, Node.js, Rust 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 구조

```
basic_cs/
├── CS/          C# 기초 강의 (20개 챕터)
├── NODEJS/      Node.js 기초 강의 (20개 챕터)
└── RUST/        Rust 기초 강의 (20개 챕터)
```

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
