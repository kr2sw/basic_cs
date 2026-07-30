from fastapi import FastAPI

from routers.v1 import items as items_v1
from routers.v2 import items as items_v2

app = FastAPI()

app.include_router(items_v1.router, prefix="/v1")
app.include_router(items_v2.router, prefix="/v2")


@app.get("/")
def root():
    return {
        "message": "API Versioning Demo",
        "v1": "/v1/items",
        "v2": "/v2/items",
    }
