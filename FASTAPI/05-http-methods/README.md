# 05: HTTP 메서드 — CRUD

## 실행

```bash
uvicorn main:app --reload
```

## 테스트

```bash
# Create
curl -X POST http://localhost:8000/items -H "Content-Type: application/json" -d '{"name":"Book","price":15.99}'
# Read all
curl http://localhost:8000/items
# Read one
curl http://localhost:8000/items/1
# Update
curl -X PUT http://localhost:8000/items/1 -H "Content-Type: application/json" -d '{"name":"Book","price":19.99}'
# Delete
curl -X DELETE http://localhost:8000/items/1
```

## 주요 개념

- **@app.get()**: Read (조회)
- **@app.post()**: Create (생성)
- **@app.put()**: Update (전체 수정)
- **@app.patch()**: Partial Update (일부 수정)
- **@app.delete()**: Delete (삭제)
