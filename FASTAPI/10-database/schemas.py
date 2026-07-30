from typing import Optional

from pydantic import BaseModel


class ItemBase(BaseModel):
    name: str
    price: float
    is_offer: Optional[bool] = False
    description: Optional[str] = None


class ItemCreate(ItemBase):
    pass


class ItemUpdate(BaseModel):
    name: Optional[str] = None
    price: Optional[float] = None
    is_offer: Optional[bool] = None
    description: Optional[str] = None


class Item(ItemBase):
    id: int

    model_config = {"from_attributes": True}
