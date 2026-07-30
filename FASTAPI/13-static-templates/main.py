from pathlib import Path

from fastapi import FastAPI, Request
from fastapi.responses import HTMLResponse
from fastapi.staticfiles import StaticFiles
from fastapi.templating import Jinja2Templates

app = FastAPI()

BASE_DIR = Path(__file__).parent
app.mount("/static", StaticFiles(directory=str(BASE_DIR / "static")), name="static")
templates = Jinja2Templates(directory=str(BASE_DIR / "templates"))


@app.get("/", response_class=HTMLResponse)
def home(request: Request):
    return templates.TemplateResponse(
        "index.html",
        {"request": request, "title": "FastAPI + Jinja2"},
    )


@app.get("/items/{item_id}", response_class=HTMLResponse)
def item_detail(request: Request, item_id: int):
    items = {
        1: {"name": "Laptop", "price": 999.99},
        2: {"name": "Mouse", "price": 29.99},
        3: {"name": "Keyboard", "price": 89.99},
    }
    item = items.get(item_id, {"name": "Unknown", "price": 0})
    return templates.TemplateResponse(
        "item.html",
        {"request": request, "item_id": item_id, "item": item},
    )
