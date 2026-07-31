# 31: OpenAPI 커스터마이징 — 태그, 메타데이터, 커스텀 문서

FastAPI는 `main:app`에서 자동으로 OpenAPI 스키마와 Swagger/ReDoc 문서를 생성합니다. 이번 챕터에서는 문서의 **품질과 브랜딩**을 높이는 커스터마이징을 다룹니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

- Swagger UI: http://127.0.0.1:8000/docs
- ReDoc: http://127.0.0.1:8000/redoc
- OpenAPI JSON: http://127.0.0.1:8000/openapi.json

## 주요 개념

### 앱 메타데이터

제목/설명/버전/연락처/라이선스를 지정하면 문서 상단과 `info` 필드에 반영됩니다. 설명에 Markdown을 넣으면 Swagger UI에서 렌더링됩니다.

```python
app = FastAPI(
    title="쇼핑 API",
    description="프로덕션급 API 문서입니다.",
    version="2.1.0",
    contact={"name": "API 팀", "email": "api@example.com"},
    license_info={"name": "MIT"},
)
```

### 태그(tags)로 그룹화

라우트에 `tags=["orders"]`를 붙이면 문서에서 그룹으로 분류됩니다. `openapi_tags`로 설명과 정렬 순서를 지정합니다.

```python
openapi_tags=[
    {"name": "orders", "description": "주문 생성과 조회"},
]
```

### get_openapi 오버라이드

`get_openapi()`로 생성된 스키마를 가져와 커스텀 필드를 추가할 수 있습니다. `x-`로 시작하는 **vendor extension**은 비표준 필드로, 내부 메타데이터(연결 서버, 팀 정보 등)를 담는 데 씁니다.

```python
def custom_openapi():
    schema = get_openapi(title=..., routes=app.routes, ...)
    schema["x-api-id"] = "shopping-api-v2"
    app.openapi_schema = schema
    return schema

app.openapi = custom_openapi
```

### Swagger UI 동작 파라미터

`swagger_ui_parameters`로 UI 동작을 조정합니다.

```python
swagger_ui_parameters={
    "persistAuthorization": True,      # Authorize 버튼 상태 유지
    "defaultModelsExpandDepth": -1,    # 모델 스키마 기본 접기
}
```

### 문서 라우트 변경

`docs_url`, `redoc_url`, `openapi_url`을 바꾸면 보안(문서 비공개) 목적에 대응할 수 있습니다. 예: 운영 환경에서 `openapi_url=None`으로 스키마를 숨길 수 있습니다.

## 연습

1. `terms_of_service`와 `contact`를 추가하고 `/docs`에서 어떻게 보이는지 확인해 보세요.
2. `openapi_tags`를 `users`와 `orders` 두 그룹으로 분류해 정렬 순서를 바꿔 보세요.
