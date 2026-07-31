# 35: GraphQL — Strawberry 기초, 스키마, 리졸버

REST는 여러 개의 엔드포인트로 리소스를 노출하지만, GraphQL은 **하나의 엔드포인트**에서 클라이언트가 원하는 필드를 정확히 골라 받을 수 있습니다. 이번 챕터에서는 Python GraphQL 라이브러리인 **Strawberry**를 FastAPI와 함께 사용합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

GraphQL 플레이그라운드: http://127.0.0.1:8000/graphql

```graphql
query {
  books { id title price }
}
```

## 주요 개념

### 스키마와 타입

`@strawberry.type`로 객체 타입, `@strawberry.input`으로 입력 타입을 정의합니다.

```python
@strawberry.type
class Book:
    id: int
    title: str
    author: str
    price: int
```

### Query — 읽기

`Query` 타입의 필드가 곧 조회 가능한 항목입니다. `@strawberry.field` 데코레이터를 붙인 메서드(리졸버)를 작성합니다. 인자에 기본값이 있는 경우 선택 인자로 노출됩니다.

```python
@strawberry.type
class Query:
    @strawberry.field
    def books(self, author: Optional[str] = None) -> list[Book]:
        return [Book(**b) for b in fake_books if not author or b["author"] == author]
```

### Mutation — 쓰기

변경 작업은 `Mutation` 타입에 정의합니다. 인자 대신 `@strawberry.input` 타입을 쓰면 여러 값을 한 번에 전달할 수 있습니다.

```python
@strawberry.mutation
def add_book(self, data: BookInput) -> Book:
    ...
```

### 스키마 구성과 라우팅

```python
schema = strawberry.Schema(query=Query, mutation=Mutation)
app.include_router(GraphQLRouter(schema), prefix="/graphql")
```

### REST vs GraphQL

| 구분 | REST | GraphQL |
|------|------|---------|
| 엔드포인트 | 리소스별 다수 | 단일 `/graphql` |
| 응답 크기 | 전체(또는 필드 제한 불가) | 클라이언트 선택 필드만 |
| 오버페칭/언더페칭 | 발생 가능 | 최소화 |
| 캐싱/인증 | HTTP 표준 활용 | 별도 설계 필요 |

## 연습

1. `price` 필터(`max_price`)를 `books` 리졸버에 추가해 보세요.
2. 한 쿼리에서 `books`와 `book(id:1)`을 동시에 요청해 보세요.
