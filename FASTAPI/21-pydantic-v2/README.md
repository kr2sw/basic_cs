# 21: Pydantic v2 심화 — computed_field, model_validator, 재사용 모델

Pydantic v1 -> v2로 바뀌며 검증 엔진이 Rust 기반인 **Pydantic Core**로 교체되어 약 5~50배 빨라졌습니다. 기초 챕터에서 다룬 `Field`, `field_validator`에 이어 v2에서 새로 추가되거나 달라진 핵심 기능을 살펴봅니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

## 주요 개념

### computed_field — 계산 필드

저장하지 않고 계산만 하는 필드를 응답에 자동 포함시킵니다. `@property` 위에 `@computed_field`를 붙입니다.

```python
class OrderItem(BaseModel):
    name: str
    price: int
    quantity: int = 1

    @computed_field
    @property
    def total(self) -> int:
        return self.price * self.quantity
```

- `response_model`로 지정하면 클라이언트에 자동 노출됩니다.
- 직렬화 시에만 존재하므로 `model_dump()` 기본값으로는 제외됩니다 (`by_alias` 등 옵션 참고).

### field_validator vs model_validator

- `field_validator`: **단일 필드** 검증 (v1의 `@validator` 대체).
- `model_validator(mode="after")`: 모델 전체가 만들어진 뒤 **필드 간 관계** 검증.
- `model_validator(mode="before")`: 원본 데이터가 파싱되기 전에 가공.

```python
@model_validator(mode="after")
def limit_total(self):
    if self.grand_total > 1_000_000:
        raise ValueError("1회 주문 금액은 100만원을 초과할 수 없습니다")
    return self
```

### ConfigDict — v1 `class Config` 대체

```python
model_config = ConfigDict(
    from_attributes=True,   # ORM 객체 변환 허용
    validate_assignment=True,  # 값 재할당 시 재검증
    frozen=True,            # 불변 모델
    extra="forbid",         # 정의되지 않은 필드 거부
)
```

### 재사용 모델 — 상속과 합성

공통 필드를 부모 모델로 빼서 재사용합니다. 응답용/요청용 스키마를 분리할 때도 유용합니다.

```python
class Timestamped(BaseModel):
    created_by: str = "system"
    created_at: str | None = None

class Product(Timestamped):
    id: int
    name: str
    price: int
```

### from_attributes — ORM 객체 변환

`from_attributes=True`를 설정하면 `Row.model_validate(db_row)` 형태로 ORM 객체를 바로 검증/변환할 수 있습니다. SQLAlchemy와 함께 쓸 때 가장 많이 활용됩니다.

### TypeAdapter — 모델 밖의 타입 검증

BaseModel이 아니라 `list[str]` 같은 단일 타입도 검증할 수 있습니다. `Tag` 모델을 따로 만들기 아까운 쿼리 파싱에 유용합니다.

```python
_tag_adapter = TypeAdapter(list[str])
tags = _tag_adapter.validate_python(raw.split(","))
```

## 연습

1. `Order`에 배송비 필드를 추가하고, `grand_total >= 50000`이면 배송비 무료가 되도록 `computed_field`로 표현해 보세요.
2. `frozen=True` 모델에서 값을 바꾸면 어떤 예외가 발생하는지 확인해 보세요.
