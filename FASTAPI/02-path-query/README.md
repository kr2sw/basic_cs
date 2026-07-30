# 02: 경로와 쿼리 매개변수

## 실행

```bash
uvicorn main:app --reload
```

경로: http://127.0.0.1:8000/users/42
쿼리: http://127.0.0.1:8000/items?skip=0&limit=10
검증: http://127.0.0.1:8000/users/abc

## 주요 개념

- **경로 매개변수**: URL 경로의 일부로 전달 (`/users/{user_id}`)
- **쿼리 매개변수**: URL 뒤 `?key=value` 형태로 전달
- **타입 힌트**: FastAPI가 타입에 따라 자동 검증
- **기본값**: 기본값이 있는 매개변수는 자동으로 쿼리 파라미터
- **Path / Query**: 추가 검증 옵션 제공
