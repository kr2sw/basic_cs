# 10: 데이터베이스 — SQLAlchemy + FastAPI

## 설치

```bash
pip install sqlalchemy databases
```

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **SQLAlchemy ORM**: Python ORM을 통한 데이터베이스 연동
- **세션 관리**: 의존성 주입으로 DB 세션 관리
- **모델 정의**: SQLAlchemy Model + Pydantic Schema
- **마이그레이션**: Alembic을 통한 스키마 관리
