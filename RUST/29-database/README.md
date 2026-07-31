# 29: 데이터베이스 — sqlx/Diesel 개념, 파일 기반 저장

Rust에서 DB를 다루는 주요 크레이트인 `sqlx`, `Diesel`의 개념을 살펴보고, 표준 라이브러리로 파일 기반 저장소를 구현합니다.

## sqlx 개념 (외부 크레이트)

```rust
// Cargo.toml
// sqlx = { version = "0.8", features = ["runtime-tokio", "postgres"] }

let row = sqlx::query("SELECT id, name FROM users WHERE id = ?")
    .bind(1)
    .fetch_one(&pool).await?;
```

- 컴파일 타임 쿼리 검증, 비동기 지원

## Diesel 개념 (외부 크레이트)

- ORM, 마이그레이션, 스키마 코드 생성

## 본 챕터 구현

파일 기반 키-값 저장소 + 인덱스 유지, 간단한 SQL 파서 흉내

## 실행

```bash
cd RUST/29-database
cargo run
```
