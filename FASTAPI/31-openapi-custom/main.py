from fastapi import APIRouter, FastAPI
from fastapi.openapi.utils import get_openapi

# --- 메타데이터: 제목, 설명, 버전, 연락처, 라이선스 ---
app = FastAPI(
    title="쇼핑 API",
    description="""
쇼핑몰을 위한 프로덕션급 API 문서입니다.

## 주요 기능
- 상품 조회 / 주문 생성 / 사용자 관리
- JWT 인증 기반의 엔드포인트 보호
""",
    version="2.1.0",
    terms_of_service="https://example.com/terms",
    contact={"name": "API 팀", "url": "https://example.com/contact", "email": "api@example.com"},
    license_info={"name": "MIT", "url": "https://opensource.org/licenses/MIT"},
    # 태그 정의: 그룹별 설명과 정렬 순서 지정
    openapi_tags=[
        {"name": "users", "description": "사용자 계정 관리"},
        {"name": "products", "description": "상품 조회/검색"},
        {"name": "orders", "description": "주문 생성과 조회"},
    ],
    # Swagger UI 동작 옵션
    swagger_ui_parameters={"persistAuthorization": True, "defaultModelsExpandDepth": -1},
)

router = APIRouter()


@router.get("/users", tags=["users"], summary="사용자 목록")
def list_users():
    return [{"id": 1, "name": "alice"}, {"id": 2, "name": "bob"}]


@router.get("/products", tags=["products"], summary="상품 목록")
def list_products():
    return [{"id": 1, "name": "노트북", "price": 1200000}]


@router.post("/orders", tags=["orders"], summary="주문 생성", status_code=201)
def create_order():
    return {"order_id": 100, "status": "created"}


app.include_router(router)


# --- OpenAPI 커스터마이징: 기본 스키마에 확장 필드 추가 ---
def custom_openapi():
    """get_openapi 오버라이드: 기본 생성 스키마를 가져와 확장"""
    if app.openapi_schema:
        return app.openapi_schema
    schema = get_openapi(
        title=app.title,
        version=app.version,
        description=app.description,
        routes=app.routes,
        tags=app.openapi_tags,
        terms_of_service=app.terms_of_service,
        contact=app.contact,
        license_info=app.license_info,
    )
    # vendor extension(x-*)은 OpenAPI 표준에 없는 커스텀 필드
    schema["x-api-id"] = "shopping-api-v2"
    schema["info"]["x-contact-team"] = "platform@example.com"
    app.openapi_schema = schema
    return schema


app.openapi = custom_openapi
