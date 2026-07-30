from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

app = FastAPI()


class Item(BaseModel):
    name: str
    price: float


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


@app.delete("/items/{item_id}")
def delete_item(item_id: int):
    if item_id not in db:
        raise HTTPException(status_code=404, detail="Item not found")
    del db[item_id]
    return {"message": "Deleted"}
