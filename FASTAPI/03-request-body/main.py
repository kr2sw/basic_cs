from datetime import date
from typing import Optional

from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()


class Item(BaseModel):
    name: str
    price: float
    is_offer: Optional[bool] = None
    description: Optional[str] = None


class Order(BaseModel):
    item_id: int
    quantity: int = 1
    shipping_date: Optional[date] = None


@app.post("/items")
def create_item(item: Item):
    return {
        "message": f"Item '{item.name}' created",
        "price": item.price,
        "is_offer": item.is_offer,
    }


@app.post("/orders")
def create_order(order: Order):
    total = order.quantity * 100
    return {
        "order_id": 12345,
        "item_id": order.item_id,
        "quantity": order.quantity,
        "total": total,
        "shipping": str(order.shipping_date or "ASAP"),
    }


@app.put("/items/{item_id}")
def update_item(item_id: int, item: Item):
    return {"item_id": item_id, **item.model_dump()}
