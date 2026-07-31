# 25: 페이지네이션/필터/정렬 — Page/Query 검증

데이터가 많아지면 전체 응답 대신 필요한 만큼만 잘라서 반환해야 합니다. 이번 챕터에서는 **offset 기반 페이지네이션**, **커서 기반 페이지네이션**, **필터**, **정렬**, 그리고 `Query` 검증을 다룹니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

## 주요 개념

### Query 검증으로 잘못된 요청 방지

`Query`의 `ge`, `le`, `min_length`로 값을 검증하면 422 응답이 자동 반환됩니다.

```python
page: int = Query(1, ge=1)
page_size: int = Query(10, ge=1, le=100)
search: str = Query(None, min_length=1)
```

`Literal["asc", "desc"]`로 정렬 방향도 타입 단계에서 제한할 수 있습니다.

### offset/limit 페이지네이션

일반적인 게시판형 페이지네이션입니다. 페이지 수 계산 시 올림 나눗셈을 사용합니다.

```python
start = (page - 1) * page_size
items[start : start + page_size]
pages = (total + page_size - 1) // page_size
```

데이터가 커질수록 `OFFSET`이 느려지는 단점이 있습니다.

### 커서 기반 페이지네이션

SNS 피드처럼 실시간으로 데이터가 늘어나는 경우, `offset`은 중복/누락이 발생할 수 있습니다. **커서**(마지막으로 본 항목의 id)를 받아 그 이후만 조회합니다.

```python
candidates = [p for p in fake_products if p["id"] > cursor]
```

- 새 데이터가 들어와도 안정적입니다.
- 정렬 기준이 고유값(id)이어야 합니다.

### 필터와 정렬

필터는 먼저 적용한 뒤 정렬하고, 정렬 가능한 필드는 화이트리스트로 제한합니다. 사용자 입력을 그대로 정렬 키로 쓰면 열거 폭탄이 될 수 있습니다.

```python
allowed = ("id", "price", "name")
if sort not in allowed:
    raise HTTPException(422, "정렬 불가 필드")
```

### 응답 형식 통일

`Page` 모델로 응답을 통일하면 클라이언트가 한 번만 구현하면 됩니다. `total`, `pages`, `has_next` 등을 함께 내려줍니다.

## 연습

1. `Query` 검증으로 `page_size`의 최댓값을 100으로 제한해 보세요.
2. `list_products_cursor`에서 정렬 기준을 `price`로 바꾸면 어떤 문제가 생기는지 생각해 보세요.
