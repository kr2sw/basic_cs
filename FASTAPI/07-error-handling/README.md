# 07: 에러 처리 — HTTPException과 커스텀 핸들러

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **HTTPException**: HTTP 에러 응답 발생
- **status_code**: HTTP 상태 코드 상수 (400, 404, 500 등)
- **@app.exception_handler**: 전역 예외 처리기
- **RequestValidationError**: Pydantic 검증 에러 커스터마이징
- **detail**: 에러 상세 메시지
