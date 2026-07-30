# 09: 미들웨어 — CORS, 요청/응답 처리

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **@app.middleware("http")**: HTTP 요청/응답 미들웨어
- **CORSMiddleware**: Cross-Origin Resource Sharing 설정
- **TrustedHostMiddleware**: 허용된 호스트 검증
- **요청/응답 로깅**: 실행 시간 측정 등
