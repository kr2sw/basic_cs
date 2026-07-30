from datetime import datetime
from typing import Optional

from fastapi import FastAPI, status
from pydantic import BaseModel

app = FastAPI()


class ItemIn(BaseModel):
    name: str
    price: float
    tax: Optional[float] = None


class ItemOut(BaseModel):
    name: str
    price: float
    price_with_tax: Optional[float] = None
    created_at: datetime = None


class MessageOut(BaseModel):
    message: str
    status_code: int


@app.post("/items", response_model=ItemOut, status_code=status.HTTP_201_CREATED)
def create_item(item: ItemIn):
    price_with_tax = item.price + item.tax if item.tax else None
    return ItemOut(
        name=item.name,
        price=item.price,
        price_with_tax=price_with_tax,
        created_at=datetime.now(),
    )


@app.get("/items/{item_id}", response_model=ItemOut)
def get_item(item_id: int):
    return ItemOut(
        name=f"Item {item_id}",
        price=99.99,
        price_with_tax=109.99,
        created_at=datetime.now(),
    )


@app.delete("/items/{item_id}", response_model=MessageOut)
def delete_item(item_id: int):
    return MessageOut(
        message=f"Item {item_id} deleted",
        status_code=status.HTTP_200_OK,
    )


@app.get("/items", response_model=list[ItemOut])
def list_items():
    return [
        ItemOut(name="Item A", price=10.0, created_at=datetime.now()),
        ItemOut(name="Item B", price=20.0, created_at=datetime.now()),
    ]
