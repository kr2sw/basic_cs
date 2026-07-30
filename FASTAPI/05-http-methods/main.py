from typing import Optional

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

app = FastAPI()


class Item(BaseModel):
    name: str
    price: float
    description: Optional[str] = None


class ItemUpdate(BaseModel):
    name: Optional[str] = None
    price: Optional[float] = None
    description: Optional[str] = None


db: dict[int, Item] = {}
next_id = 1


@app.post("/items")
def create_item(item: Item):
    global next_id
    db[next_id] = item
    next_id += 1
    return {"id": next_id - 1, **item.model_dump()}


@app.get("/items")
def list_items():
    return [{"id": i, **item.model_dump()} for i, item in db.items()]


@app.get("/items/{item_id}")
def get_item(item_id: int):
    if item_id not in db:
        raise HTTPException(status_code=404, detail="Item not found")
    return {"id": item_id, **db[item_id].model_dump()}


@app.put("/items/{item_id}")
def update_item(item_id: int, item: Item):
    if item_id not in db:
        raise HTTPException(status_code=404, detail="Item not found")
    db[item_id] = item
    return {"id": item_id, **item.model_dump()}


@app.patch("/items/{item_id}")
def patch_item(item_id: int, item: ItemUpdate):
    if item_id not in db:
        raise HTTPException(status_code=404, detail="Item not found")
    existing = db[item_id].model_dump()
    update_data = item.model_dump(exclude_unset=True)
    existing.update(update_data)
    db[item_id] = Item(**existing)
    return {"id": item_id, **existing}


@app.delete("/items/{item_id}")
def delete_item(item_id: int):
    if item_id not in db:
        raise HTTPException(status_code=404, detail="Item not found")
    del db[item_id]
    return {"message": "Item deleted"}
