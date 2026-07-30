from typing import Optional

from fastapi import FastAPI, Depends, HTTPException, Query, Header

app = FastAPI()


def pagination(
    skip: int = Query(0, ge=0),
    limit: int = Query(10, ge=1, le=100),
):
    return {"skip": skip, "limit": limit}


def verify_token(authorization: Optional[str] = Header(None)):
    if not authorization:
        raise HTTPException(status_code=401, detail="No authorization header")
    if not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Invalid token format")
    token = authorization.replace("Bearer ", "")
    if token != "valid-token":
        raise HTTPException(status_code=401, detail="Invalid token")
    return {"username": "test_user", "role": "admin"}


class DatabaseSession:
    def __init__(self):
        self.connected = True

    def query(self, sql: str):
        return f"Executing: {sql}"

    def close(self):
        self.connected = False


async def get_db():
    db = DatabaseSession()
    try:
        yield db
    finally:
        db.close()


@app.get("/items")
def list_items(
    pagination: dict = Depends(pagination),
    user: dict = Depends(verify_token),
    db: DatabaseSession = Depends(get_db),
):
    result = db.query(f"SELECT * FROM items LIMIT {pagination['limit']} OFFSET {pagination['skip']}")
    return {
        "user": user["username"],
        "page": pagination,
        "result": result,
    }


@app.get("/users")
def list_users(
    pagination: dict = Depends(pagination),
    user: dict = Depends(verify_token),
):
    return {
        "user": user["username"],
        "page": pagination,
    }
