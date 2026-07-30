# 01: 소개 — 첫 FastAPI 앱

FastAPI는 현대적인 Python 웹 프레임워크로, Starlette 위에 구축되었으며 Pydantic을 사용한 자동 데이터 검증을 제공합니다.

## 실행

```bash
uvicorn main:app --reload
# http://127.0.0.1:8000
# API 문서: http://127.0.0.1:8000/docs
# 대체 문서: http://127.0.0.1:8000/redoc
```

## 주요 개념

- **FastAPI()**: 앱 인스턴스 생성
- **@app.get()**: 경로 데코레이터 (HTTP 메서드 + 경로)
- **자동 문서화**: OpenAPI/Swagger UI 자동 생성
- **uvicorn --reload**: 개발 모드 (코드 변경 시 자동 재시작)
