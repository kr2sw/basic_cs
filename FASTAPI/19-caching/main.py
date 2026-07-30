import hashlib
import json
import time
from functools import lru_cache

from fastapi import FastAPI, HTTPException, Request, Response
from fastapi.responses import JSONResponse

app = FastAPI()

fake_db = {
    1: {"name": "Laptop", "price": 999.99, "stock": 5},
    2: {"name": "Mouse", "price": 29.99, "stock": 10},
    3: {"name": "Keyboard", "price": 89.99, "stock": 3},
}


@lru_cache(maxsize=32)
def get_cached_item(item_id: int) -> dict | None:
    time.sleep(0.5)
    return fake_db.get(item_id)


@app.get("/items/{item_id}")
def get_item(item_id: int, request: Request):
    data = get_cached_item(item_id)
    if data is None:
        raise HTTPException(status_code=404, detail="Item not found")

    body = json.dumps({"id": item_id, **data}, ensure_ascii=False)
    etag = hashlib.md5(body.encode()).hexdigest()

    if request.headers.get("if-none-match") == etag:
        return Response(status_code=304)

    return JSONResponse(content=json.loads(body), headers={"ETag": etag})


@app.get("/cache-info")
def cache_info():
    return {
        "cache_size": get_cached_item.cache_info().currsize,
        "hits": get_cached_item.cache_info().hits,
        "misses": get_cached_item.cache_info().misses,
    }


@app.post("/clear-cache")
def clear_cache():
    get_cached_item.cache_clear()
    return {"message": "Cache cleared"}
