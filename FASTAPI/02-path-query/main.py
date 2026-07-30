from fastapi import FastAPI, Path, Query

app = FastAPI()


@app.get("/users/{user_id}")
def get_user(
    user_id: int = Path(..., ge=1, description="사용자 ID (1 이상)"),
):
    return {"user_id": user_id}


@app.get("/items")
def list_items(
    skip: int = Query(0, ge=0, description="건너뛸 개수"),
    limit: int = Query(10, ge=1, le=100, description="가져올 개수"),
    category: str = Query(None, max_length=20, description="카테고리 필터"),
):
    result = {"skip": skip, "limit": limit}
    if category:
        result["category"] = category
    return result


@app.get("/products/{product_id}")
def get_product(
    product_id: int = Path(..., alias="id"),
    detail: bool = Query(False, description="상세 정보 포함 여부"),
):
    product = {"id": product_id, "name": f"Product {product_id}", "price": 99.99}
    if detail:
        product["description"] = f"This is product {product_id}"
        product["in_stock"] = True
    return product
