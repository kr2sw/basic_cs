# 06: 데이터 검증 — 필드 검증과 커스텀 검증

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **Field**: Pydantic의 필드 검증 (min_length, max_length, ge, le, pattern)
- **@field_validator**: 특정 필드의 커스텀 검증 함수
- **@model_validator**: 모델 전체 검증
- **EmailStr / HttpUrl**: Pydantic의 특수 타입
- **SecretStr**: 민감 정보 마스킹
