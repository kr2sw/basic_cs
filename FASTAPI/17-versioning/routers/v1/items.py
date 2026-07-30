from fastapi import APIRouter

router = APIRouter(prefix="/items", tags=["items-v1"])


@router.get("/")
def list_items():
    return [{"id": 1, "name": "Item v1", "price": 10.0}]


@router.post("/")
def create_item(name: str, price: float = 0.0):
    return {"id": 2, "name": name, "price": price, "version": "v1"}
