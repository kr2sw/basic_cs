# 28: 웹 프레임워크 — Axum/Actix 개념, 미니 라우터 구현

Axum, Actix-web 같은 웹 프레임워크의 개념을 이해하고 표준 라이브러리로 미니 라우터를 만듭니다.

## Axum 개념 (외부 크레이트)

```rust
// Cargo.toml
// axum = "0.7"
// tokio = { version = "1", features = ["full"] }

use axum::{routing::get, Router};
async fn hello() -> &'static str { "Hello, World!" }
let app = Router::new().route("/", get(hello));
```

- **Router**: 경로와 핸들러를 등록
- **핸들러**: 요청을 받아 응답을 반환하는 함수
- **상태 공유**: `State`로 DB 등 공유

## 본 챕터 구현

- `get`, `post`, `put`, `delete` 메서드 라우팅
- 경로 파라미터 `:id`
- 미들웨어(로깅) 개념

## 실행

```bash
cd RUST/28-web-frameworks
cargo run
```

브라우저에서 `http://127.0.0.1:8080/users/42` 확인 가능.
