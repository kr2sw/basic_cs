# 03: 요청 본문 — Pydantic 모델

## 실행

```bash
uvicorn main:app --reload
```

POST http://127.0.0.1:8000/items
```json
{"name": "Laptop", "price": 999.99, "is_offer": true}
```

## 주요 개념

- **Pydantic BaseModel**: 데이터 검증 모델 정의
- **자동 JSON 파싱**: 요청 본문을 자동으로 Pydantic 모델로 변환
- **필드 타입 검증**: 타입 힌트에 따른 자동 검증
- **선택적 필드**: `Optional[T]` 또는 기본값으로 설정
