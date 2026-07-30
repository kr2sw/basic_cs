from fastapi import FastAPI, HTTPException, Request, status
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse

app = FastAPI()


class NotFoundError(HTTPException):
    def __init__(self, item_id: int):
        super().__init__(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=f"Item {item_id} not found",
        )


class InsufficientStockError(HTTPException):
    def __init__(self, item_id: int, requested: int, available: int):
        super().__init__(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail={
                "error": "Insufficient stock",
                "item_id": item_id,
                "requested": requested,
                "available": available,
            },
        )


@app.exception_handler(RequestValidationError)
async def validation_exception_handler(request: Request, exc: RequestValidationError):
    return JSONResponse(
        status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        content={
            "detail": exc.errors(),
            "body": exc.body,
            "message": "입력 데이터가 올바르지 않습니다.",
        },
    )


db = {1: {"name": "Laptop", "stock": 5}, 2: {"name": "Mouse", "stock": 10}}


@app.get("/items/{item_id}")
def get_item(item_id: int):
    if item_id not in db:
        raise NotFoundError(item_id)
    return db[item_id]


@app.post("/items/{item_id}/order")
def order_item(item_id: int, quantity: int = 1):
    if item_id not in db:
        raise NotFoundError(item_id)

    item = db[item_id]
    if item["stock"] < quantity:
        raise InsufficientStockError(item_id, quantity, item["stock"])

    item["stock"] -= quantity
    return {"message": "Order success", "remaining": item["stock"]}


@app.get("/risky")
def risky_operation():
    raise HTTPException(
        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        detail="Unexpected server error",
    )
