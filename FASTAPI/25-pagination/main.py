import time
from typing import Literal

from fastapi import FastAPI, HTTPException, Query
from pydantic import BaseModel

app = FastAPI(title="페이지네이션 / 필터 / 정렬")

# 데모용 상품 데이터
fake_products: list[dict] = [
    {"id": i, "name": f"상품{i}", "price": (i * 137) % 100000, "category": "카테고리" + str(i % 5)}
    for i in range(1, 101)
]


class Page(BaseModel):
    """일반적인 페이지네이션 응답 형식"""
    items: list[dict]
    total: int
    page: int
    page_size: int
    pages: int
    has_next: bool


# ---- 쿼리 매개변수 검증 (Query) ----
def parse_sort(order_by: str, order: Literal["asc", "desc"], allowed: tuple[str, ...]):
    if order_by not in allowed:
        raise HTTPException(status_code=422, detail=f"정렬 불가 필드: {order_by}")
    return order_by, order


@app.get("/products", response_model=Page)
def list_products(
    page: int = Query(1, ge=1, description="페이지 번호"),
    page_size: int = Query(10, ge=1, le=100, description="페이지당 개수"),
    category: str | None = Query(None, description="카테고리 필터"),
    search: str = Query(None, min_length=1, description="이름 검색"),
    sort: str = Query("id", description="정렬 기준"),
    order: Literal["asc", "desc"] = Query("asc", description="정렬 방향"),
):
    """offset/limit 기반 페이지네이션 + 필터 + 정렬"""
    sort, order = parse_sort(sort, order, allowed=("id", "price", "name"))

    items = fake_products
    if category:
        items = [p for p in items if p["category"] == category]
    if search:
        items = [p for p in items if search in p["name"]]

    items = sorted(items, key=lambda p: p[sort], reverse=(order == "desc"))

    total = len(items)
    pages = (total + page_size - 1) // page_size
    start = (page - 1) * page_size
    return Page(
        items=items[start : start + page_size],
        total=total,
        page=page,
        page_size=page_size,
        pages=pages,
        has_next=page < pages,
    )


class CursorPage(BaseModel):
    """커서 기반 페이지네이션: 마지막 항목의 값이 다음 커서"""
    items: list[dict]
    next_cursor: int | None
    has_more: bool


@app.get("/products/cursor", response_model=CursorPage)
def list_products_cursor(
    cursor: int = Query(0, ge=0, description="이전 응답의 next_cursor"),
    limit: int = Query(10, ge=1, le=100),
):
    """커서 기반: offset이 아니라 '이전에 본 마지막 id'부터 조회"""
    candidates = sorted((p for p in fake_products if p["id"] > cursor), key=lambda p: p["id"])
    page_items = candidates[:limit]
    next_cursor = page_items[-1]["id"] if len(page_items) == limit else None
    return CursorPage(items=page_items, next_cursor=next_cursor, has_more=next_cursor is not None)


class PriceStats(BaseModel):
    count: int
    total: int
    min_price: int
    max_price: int
    avg_price: float


@app.get("/products/stats", response_model=PriceStats)
def price_stats(category: str | None = None):
    """필터된 결과에 대한 집계"""
    items = fake_products
    if category:
        items = [p for p in items if p["category"] == category]
    prices = [p["price"] for p in items]
    return PriceStats(
        count=len(prices),
        total=sum(prices),
        min_price=min(prices),
        max_price=max(prices),
        avg_price=round(sum(prices) / len(prices), 2),
    )
