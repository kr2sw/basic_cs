# 04: 응답 모델과 상태 코드

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **response_model**: 응답 데이터의 스키마 지정
- **status_code**: HTTP 상태 코드 커스터마이징
- **response_model_exclude_unset**: 기본값 필드 제외
- **response_model_include / exclude**: 필드 선택적 포함/제외
