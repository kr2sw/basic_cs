from pydantic import BaseModel

from fastapi import APIRouter

router = APIRouter(prefix="/items", tags=["items-v2"])


class ItemCreate(BaseModel):
    name: str
    price: float
    description: str | None = None


class Item(ItemCreate):
    id: int


@router.get("/", response_model=list[Item])
def list_items():
    return [Item(id=1, name="Item v2", price=20.0, description="New version")]


@router.post("/", response_model=Item)
def create_item(item: ItemCreate):
    return Item(id=2, **item.model_dump())
