# 37: 데이터베이스 (Database) — sqlite3, SQLAlchemy Core/ORM 기초

## sqlite3 (표준 라이브러리)
파일 하나로 동작하는 경량 DB입니다. `:memory:`를 사용하면 인메모리로 테스트할 수 있습니다.

```python
import sqlite3
conn = sqlite3.connect("app.db")
cur = conn.cursor()
```

## 기본 SQL
- `CREATE TABLE`, `INSERT INTO`, `SELECT`, `WHERE`, `ORDER BY`
- `commit()`으로 저장, `execute()`로 SQL 실행

## 트랜잭션
여러 작업을 하나로 묶어 원자성을 보장합니다. 실패 시 `rollback()`.

## SQLAlchemy
ORM/Core를 제공하는 서드파티 라이브러리입니다 (`pip install sqlalchemy`). 주석으로 예시를 제공합니다.

## 실행

```bash
python main.py
```
