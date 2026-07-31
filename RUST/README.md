# Rust 강의 (기초 20개 + 중급 20개 챕터)

Rust 프로그래밍 언어의 기초부터 중급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

Rust는 2006년 Mozilla의 직원 Graydon Hoare가 개인 프로젝트로 시작했습니다. 2009년 Mozilla가 공식 후원을 시작했고, 2010년 첫 공개 발표가 있었습니다. Rust의 주된 목표는 메모리 안전성을 보장하면서도 C/C++ 수준의 성능을 제공하는 안전한 시스템 프로그래밍 언어를 만드는 것이었습니다. 2015년 Rust 1.0이 안정화되었고, 2020년부터 Rust Foundation이 Mozilla로부터 관리권을 이어받았습니다. 2022년 Linux 커널이 Rust 도입을 공식화했고, 2023년 Android, Windows 등 주요 OS에서 Rust 채택이 확대되었습니다. 2023년에는 Stack Overflow 설문조사에서 "가장 사랑받는 언어" 8년 연속 1위를 기록했습니다.

## 특징

- **메모리 안전성**: 컴파일 타임에 소유권(Ownership), 대여(Borrowing), 수명(Lifetime)을 검증하여 메모리 오류 원천 차단
- **제로 코스트 추상화**: 고수준 추상화가 런타임 오버헤드를 발생시키지 않음
- **데이터 레이스 방지**: 컴파일러가 동시성 접근을 엄격히 검사하여 데이터 레이스를 원천 차단
- **강력한 타입 시스템**: 대수적 데이터 타입(enum), 패턴 매칭, 제네릭, 트레이트
- **높은 성능**: C/C++에 필적하는 실행 속도와 메모리 효율
- **뛰어난 도구**: cargo(빌드/패키지 관리), rustfmt(포매터), clippy(린터), rust-analyzer
- **안전한 FFI**: C 언어와의 상호 운용성을 위한 안전한 외부 함수 인터페이스

## 실행

```bash
cd RUST/01-hello-world && cargo run
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | cargo new, main 함수, println!, cargo run |
| 02 | Variables | 변수/가변성, 상수, shadowing, 데이터 타입 |
| 03 | Control Flow | if/else, loop/while/for, match |
| 04 | Ownership | 소유권 규칙, 이동(move), 복제(clone), 복사(copy) |
| 05 | Structs | 구조체 정의, 튜플 구조체, 메서드, impl |
| 06 | Enums | 열거형, Option, Result, 패턴 매칭 |
| 07 | Collections | Vec, String, HashMap, HashSet |
| 08 | Error Handling | panic!, Result, ?, unwrap/expect, anyhow/thiserror |
| 09 | Generics | 제네릭 함수/구조체/열거형, 모노모피제이션 |
| 10 | Traits | 트레이트 정의/구현, 트레이트 바운드, derive |
| 11 | Lifetimes | 수명 표기법, 생략 규칙, 'static |
| 12 | Closures & Iterators | 클로저, Iterator 트레이트, 반복자 어댑터 |
| 13 | Modules & Crates | mod, use, pub, Cargo.toml, 크레이트 배포 |
| 14 | File I/O | std::fs, std::io, Read/Write 트레이트, BufReader |
| 15 | Smart Pointers | Box, Rc, Arc, RefCell, Cow, Deref 트레이트 |
| 16 | Concurrency | 스레드(spawn), Message Passing(mpsc), Mutex, Arc |
| 17 | Unsafe Rust | raw 포인터, unsafe 함수/트레이트, FFI |
| 18 | Macros | macro_rules!, 선언적 매크로, 절차적 매크로 |
| 19 | Testing | #[test], assert!, 테스트 모듈, doc 테스트 |
| 20 | Web Server | 단순 HTTP 서버, TCP 연결, 라우팅, 비동기(async/await) |
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
