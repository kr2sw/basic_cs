# 17: API 버전 관리 — APIRouter

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **APIRouter**: 라우터 모듈화
- **prefix**: 경로 접두사 (/v1, /v2)
- **tags**: Swagger UI에서 그룹화
- **다중 버전**: 같은 API의 여러 버전 동시 운영
