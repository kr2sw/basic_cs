# Basic CS - 프로그래밍 기초 강의 자료

C#과 Node.js 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 구조

```
basic_cs/
├── CS/          C# 기초 강의 (11개 챕터)
└── NODEJS/      Node.js 기초 강의 (20개 챕터)
```

## C# 기초

| 챕터 | 주제 | 설명 |
|------|------|------|
| 01 | Hello World | 기본 입출력, 첫 C# 프로그램 |
| 02 | Variables | 변수, 데이터 타입, 형변환, nullable |
| 03 | Control Flow | 조건문 (if, switch), 반복문 (for, foreach, while) |
| 04 | Arrays & Collections | 배열, List, Dictionary, HashSet, Queue, Stack |
| 05 | Methods | 메서드 정의, ref/out/params, 오버로딩, 로컬 함수 |
| 06 | Classes & Objects | 클래스, 생성자, 속성, record, struct, 확장 메서드 |
| 07 | Inheritance | 상속, 다형성, virtual/override, abstract, sealed |
| 08 | Interfaces | 인터페이스, 다중 구현, DI 예제, default interface method |
| 09 | Exceptions | 예외 처리, try-catch-finally, 사용자 정의 예외 |
| 10 | LINQ | LINQ 쿼리/메서드 구문, GroupBy, 집계, SelectMany |
| 11 | Delegates & Events | 델리게이트, Func/Action/Predicate, 이벤트, 클로저 |

```bash
dotnet build CS
dotnet run --project CS/01_hello_world
```

## Node.js 기초

| 챕터 | 주제 | 설명 |
|------|------|------|
| 01 | Introduction | Node.js 소개, REPL, 첫 스크립트 |
| 02 | Module System | CommonJS, ES Modules |
| 03 | npm | 패키지 관리, package.json, 스크립트 |
| 04 | File System | fs 모듈 (readFile, writeFile, promises) |
| 05 | Path | path 모듈 (join, resolve, parse) |
| 06 | HTTP | 기본 HTTP 서버 |
| 07 | Express | Express 프레임워크 |
| 08 | Routing & Middleware | 라우터, 미들웨어 (morgan, cors) |
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

```bash
cd NODEJS/01-introduction
node index.js
```
