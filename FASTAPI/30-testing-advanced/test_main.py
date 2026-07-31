"""고급 테스팅 데모 테스트

실행:
    pytest -v
    pytest --cov=. --cov-report=term-missing   # coverage 측정
"""
import pytest
import httpx

import main


class MemoryStore:
    """실제 SQLite 대신 테스트용 인메모리 저장소"""

    def __init__(self):
        self.items: dict[int, dict] = {}
        self.seq = 0

    def create(self, title: str) -> dict:
        self.seq += 1
        item = {"id": self.seq, "title": title, "done": False}
        self.items[self.seq] = item
        return item

    def get(self, todo_id: int) -> dict | None:
        return self.items.get(todo_id)

    def list(self) -> list[dict]:
        return list(self.items.values())

    def update(self, todo_id: int, done: bool) -> dict | None:
        item = self.items.get(todo_id)
        if item is None:
            return None
        item["done"] = done
        return item


@pytest.fixture
def fake_store(monkeypatch):
    """monkeypatch로 의존성을 교체해 실제 DB 접근을 차단"""
    store = MemoryStore()
    monkeypatch.setattr(main, "store", store)
    return store


@pytest.fixture
def client(fake_store):
    """httpx.AsyncClient + ASGITransport로 앱을 직접 실행"""
    return httpx.AsyncClient(transport=httpx.ASGITransport(app=main.app), base_url="http://test")


@pytest.mark.asyncio
async def test_create_todo(client):
    resp = await client.post("/todos", json={"title": "숙제"})
    assert resp.status_code == 200
    body = resp.json()
    assert body["title"] == "숙제"
    assert body["done"] is False


@pytest.mark.asyncio
async def test_list_todos_empty(client):
    resp = await client.get("/todos")
    assert resp.status_code == 200
    assert resp.json() == []


@pytest.mark.asyncio
async def test_update_todo_not_found(client):
    resp = await client.patch("/todos/999", json={"done": True})
    assert resp.status_code == 404


@pytest.mark.asyncio
async def test_store_isolation(fake_store):
    """monkeypatch된 저장소가 실제로 사용되는지 검증"""
    fake_store.create("테스트 항목")
    assert main.store is fake_store
    assert main.store.seq == 1


@pytest.mark.asyncio
async def test_invalid_payload(client):
    """검증 실패 시 422 응답"""
    resp = await client.post("/todos", json={"wrong_field": 1})
    assert resp.status_code == 422
