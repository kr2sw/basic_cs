from fastapi.testclient import TestClient
import pytest

from main import app

client = TestClient(app)


def test_create_item():
    response = client.post("/items", json={"name": "Laptop", "price": 999.99})
    assert response.status_code == 200
    data = response.json()
    assert data["name"] == "Laptop"
    assert data["price"] == 999.99
    assert "id" in data


def test_list_items():
    response = client.get("/items")
    assert response.status_code == 200
    assert isinstance(response.json(), list)


def test_get_item_not_found():
    response = client.get("/items/999")
    assert response.status_code == 404
    assert response.json()["detail"] == "Item not found"


def test_get_item():
    client.post("/items", json={"name": "Mouse", "price": 29.99})
    response = client.get("/items/1")
    assert response.status_code == 200
    assert response.json()["name"] == "Laptop"


def test_delete_item():
    response = client.delete("/items/1")
    assert response.status_code == 200


@pytest.mark.parametrize("payload,expected_status", [
    ({"name": "A", "price": 10.0}, 200),
    ({"name": ""}, 422),
    ({"price": 10.0}, 422),
    ({}, 422),
])
def test_create_item_validation(payload, expected_status):
    response = client.post("/items", json=payload)
    assert response.status_code == expected_status
