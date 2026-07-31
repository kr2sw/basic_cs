import sqlite3
from contextlib import asynccontextmanager

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

DB_PATH = "todos.db"


class TodoStore:
    """SQLite 기반 저장소 (테스트에서 메모리 구현으로 교체)"""

    def __init__(self, db_path: str):
        self.conn = sqlite3.connect(db_path)
        self.conn.execute("CREATE TABLE IF NOT EXISTS todos (id INTEGER PRIMARY KEY, title TEXT, done INTEGER)")

    def create(self, title: str) -> dict:
        cur = self.conn.execute("INSERT INTO todos (title, done) VALUES (?, 0)", (title,))
        self.conn.commit()
        return self.get(cur.lastrowid)

    def get(self, todo_id: int) -> dict | None:
        row = self.conn.execute("SELECT id, title, done FROM todos WHERE id = ?", (todo_id,)).fetchone()
        return {"id": row[0], "title": row[1], "done": bool(row[2])} if row else None

    def list(self) -> list[dict]:
        rows = self.conn.execute("SELECT id, title, done FROM todos").fetchall()
        return [{"id": r[0], "title": r[1], "done": bool(r[2])} for r in rows]

    def update(self, todo_id: int, done: bool) -> dict | None:
        cur = self.conn.execute("UPDATE todos SET done = ? WHERE id = ?", (int(done), todo_id))
        self.conn.commit()
        return self.get(todo_id) if cur.rowcount else None


store = TodoStore(DB_PATH)


class TodoCreate(BaseModel):
    title: str


class TodoUpdate(BaseModel):
    done: bool


class TodoOut(BaseModel):
    id: int
    title: str
    done: bool


app = FastAPI(title="고급 테스팅 데모")


def get_store():
    return store


@app.post("/todos", response_model=TodoOut)
def create_todo(data: TodoCreate, store: TodoStore = Depends(get_store)):
    return store.create(data.title)


@app.get("/todos", response_model=list[TodoOut])
def list_todos(store: TodoStore = Depends(get_store)):
    return store.list()


@app.patch("/todos/{todo_id}", response_model=TodoOut)
def update_todo(todo_id: int, data: TodoUpdate, store: TodoStore = Depends(get_store)):
    todo = store.update(todo_id, data.done)
    if todo is None:
        raise HTTPException(status_code=404, detail="할 일을 찾을 수 없습니다")
    return todo


@app.get("/todos/{todo_id}", response_model=TodoOut)
def get_todo(todo_id: int, store: TodoStore = Depends(get_store)):
    todo = store.get(todo_id)
    if todo is None:
        raise HTTPException(status_code=404, detail="할 일을 찾을 수 없습니다")
    return todo
