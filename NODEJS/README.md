# Node.js 기초 (20개 챕터)

Node.js는 Chrome V8 JavaScript 엔진 기반의 서버사이드 JavaScript 런타임입니다.

## 역사

Node.js는 2009년 Ryan Dahl이 처음 발표했습니다. 기존 웹 서버(Apache HTTPD)의 스레드 기반 방식 대신, 싱글 스레드 이벤트 루프 기반的非블로킹 I/O 모델을 제안하여 혁신을 일으켰습니다. 2010년 npm(Node Package Manager)이 도입되면서 JavaScript 생태계가 폭발적으로 성장했습니다. 2011년 Microsoft와 협력하여 Windows 지원이 추가되었고, 2015년 Node.js Foundation이 설립되었습니다. 2018년에는 Node.js 10 LTS, 2023년 Node.js 20 LTS가 출시되었습니다. 현재는 서버, CLI 도구, 빌드 도구 등 웹 개발 전반의 핵심 플랫폼으로 자리잡았습니다.

## 특징

- **비동기 I/O**: 이벤트 루프 기반 논블로킹 I/O로 높은 동시성 처리
- **싱글 스레드**: 하나의 스레드로 수천 개의 연결 처리 가능
- **npm 생태계**: 세계 최대의 오픈소스 패키지 레지스트리 (200만+ 패키지)
- **실시간 애플리케이션**: WebSocket, Socket.IO를 활용한 실시간 통신에 최적
- **마이크로서비스**: 경량 HTTP 서버로 MSA 구축에 적합
- **풀 스택 JavaScript**: 클라이언트와 서버가 같은 언어 사용
- **빠른 실행**: V8 엔진의 JIT 컴파일로 높은 성능

## 실행

```bash
cd NODEJS/01-introduction && node index.js
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | Node.js 소개, 전역 객체, module, CommonJS |
| 02 | Module System | exports, require, module caching, 순환 참조 |
| 03 | npm | npm install, package.json, scripts, semver |
| 04 | File System | fs 모듈, readFile/writeFile, watch, stream |
| 05 | Path | path 모듈, join/resolve, 상대/절대 경로 |
| 06 | HTTP | http 모듈, 간단한 웹 서버, request/response |
| 07 | Express | Express 설치, 라우팅, 미들웨어, 정적 파일 |
| 08 | Routing & Middleware | Router, 미들웨어 체인, 에러 처리 |
| 09 | Template Engines | EJS, Pug, Handlebars, 템플릿 렌더링 |
| 10 | REST API | RESTful 설계, CRUD, JSON 응답 |
| 11 | Database | MongoDB(Mongoose), MySQL(Sequelize) |
| 12 | Authentication | JWT, bcrypt, Passport.js, 세션 인증 |
| 13 | File Uploads | multer, 파일 업로드, 이미지 리사이징 |
| 14 | WebSocket | ws, Socket.IO, 실시간 채팅 |
| 15 | Error Handling | 에러 미들웨어, async wrap, uncaughtException |
| 16 | Environment | dotenv, 환경변수, NODE_ENV, config |
| 17 | Async Patterns | Promise, async/await, Callback, EventEmitter |
| 18 | Streams | Readable/Writable/Transform, pipe, backpressure |
| 19 | Testing | Jest, Supertest, 단위/통합/E2E 테스트 |
| 20 | Deployment | PM2, Docker, 환경별 배포, CI/CD |
