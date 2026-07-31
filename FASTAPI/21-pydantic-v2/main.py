from typing import Literal

from fastapi import FastAPI, HTTPException, Query
from pydantic import (
    BaseModel,
    ConfigDict,
    TypeAdapter,
    computed_field,
    field_validator,
    model_validator,
)

app = FastAPI(title="Pydantic v2 심화", description="computed_field, model_validator, 재사용 모델")


# 1. computed_field: 저장된 필드가 아니라 계산으로 만들어지는 파생 필드
#    -> response_model에 자동으로 포함되어 응답된다.
class OrderItem(BaseModel):
    name: str
    price: int  # 단가(원)
    quantity: int = 1

    @computed_field
    @property
    def total(self) -> int:
        """수량 x 단가를 자동 계산"""
        return self.price * self.quantity


class Order(BaseModel):
    model_config = ConfigDict(validate_assignment=True)  # 값 할당 시점에도 재검증

    items: list[OrderItem]

    @computed_field
    @property
    def grand_total(self) -> int:
        """모든 항목의 합계"""
        return sum(item.total for item in self.items)

    @field_validator("items")
    @classmethod
    def items_not_empty(cls, v: list[OrderItem]) -> list[OrderItem]:
        if not v:
            raise ValueError("주문에는 최소 1개의 항목이 필요합니다")
        return v

    @model_validator(mode="after")
    def limit_total(self) -> "Order":
        """after 모드 검증: 모델 전체가 구성된 뒤 추가 검증"""
        if self.grand_total > 1_000_000:
            raise ValueError("1회 주문 금액은 100만원을 초과할 수 없습니다")
        return self


# 2. 재사용 모델: 공통 필드는 부모 모델로 분리해 상속으로 재사용
class Timestamped(BaseModel):
    created_by: str = "system"
    created_at: str | None = None


class Product(Timestamped):
    id: int
    name: str
    price: int

    @computed_field
    @property
    def display_price(self) -> str:
        """1,200,000원 형식으로 표시"""
        return f"{self.price:,}원"


# 3. from_attributes: ORM 객체/일반 클래스를 그대로 Pydantic 모델로 변환
class Row(BaseModel):
    model_config = ConfigDict(from_attributes=True)

    id: int
    name: str


class DBRow:
    """ORM 객체를 흉내 낸 클래스 (실제로는 SQLAlchemy 모델)"""

    def __init__(self, id: int, name: str):
        self.id = id
        self.name = name


@app.post("/orders", response_model=Order)
def create_order(order: Order):
    """요청 본문이 검증된 뒤 그대로 반환 (데모)"""
    return order


@app.get("/orders/example", response_model=Order)
def example_order():
    """computed_field가 응답에 자동 포함되는 예시"""
    return Order(items=[OrderItem(name="커피", price=4500, quantity=2)])


@app.get("/products", response_model=list[Product])
def list_products(q: str = Query("", description="이름 검색어")):
    data = [
        Product(id=1, name="노트북", price=1_200_000),
        Product(id=2, name="마우스", price=29_000),
    ]
    return [p for p in data if q in p.name]


@app.get("/rows", response_model=Row)
def row_to_model():
    """model_validate: 일반 객체를 Pydantic 모델로 변환"""
    db_row = DBRow(1, "A")
    return Row.model_validate(db_row)


# 4. TypeAdapter: BaseModel 밖의 단일 타입(예: list[str])도 검증 가능
_tag_adapter = TypeAdapter(list[str])


@app.get("/tags")
def parse_tags(raw: str = Query("a,b,c", description="쉼표로 구분된 태그")):
    try:
        tags = _tag_adapter.validate_python(raw.split(","))
    except Exception:
        raise HTTPException(status_code=422, detail="태그 형식이 올바르지 않습니다")
    return {"tags": tags}
