# 30: 고급 테스팅 — pytest-asyncio, monkeypatch, coverage

기초 챕터 16에서 `TestClient`와 pytest를 다뤘습니다. 이번에는 async 테스트, 의존성 교체(`monkeypatch`), 그리고 커버리지 측정까지 다룹니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

테스트 실행:

```bash
pytest -v
pytest --cov=. --cov-report=term-missing
```

## 주요 개념

### pytest-asyncio — async 테스트

`async` 엔드포인트를 테스트하려면 `pytest-asyncio`가 필요합니다. `@pytest.mark.asyncio`를 붙이면 해당 테스트를 async 실행합니다.

```python
@pytest.mark.asyncio
async def test_create_todo(client):
    resp = await client.post("/todos", json={"title": "숙제"})
    assert resp.status_code == 200
```

`pytest.ini`에 `asyncio_mode = auto`를 쓰면 데코레이터 없이 async 테스트를 자동 인식합니다.

### httpx.AsyncClient + ASGITransport

라이브 서버 없이 in-process로 앱을 테스트합니다.

```python
client = httpx.AsyncClient(
    transport=httpx.ASGITransport(app=main.app),
    base_url="http://test",
)
```

### monkeypatch — 의존성 교체

`monkeypatch.setattr`으로 실제 DB나 외부 API를 **가짜 구현**으로 바꿉니다. 실제 SQLite 파일이 만들어지지 않아 테스트가 빠르고 격리됩니다.

```python
@pytest.fixture
def fake_store(monkeypatch):
    store = MemoryStore()
    monkeypatch.setattr(main, "store", store)
    return store
```

`tmp_path` 픽스처를 쓰면 파일 생성 테스트도 격리할 수 있습니다. `mocker`(pytest-mock)는 `monkeypatch`의 래퍼로 `mocker.patch(...)` 형태를 제공합니다.

### coverage — 커버리지 측정

`pytest --cov=. --cov-report=term-missing`으로 **실행된 코드 비율**과 **미커버 라인**을 확인합니다.

```
Name        Stmts   Miss  Cover   Missing
main.py        45      5    89%    30-35
```

- 브랜치 커버리지(`--cov-branch`)는 if/else 분기까지 측정합니다.
- 100% 달성보다 **핵심 로직(인증, 검증, 비즈니스 규칙)**에 집중합니다.
- CI에서 `--cov-fail-under=80`로 하한을 강제할 수 있습니다.

## 연습

1. `test_update_todo_not_found`에 정상 업데이트 케이스를 추가해 보세요.
2. `pytest --cov=. --cov-report=term-missing`로 커버리지를 확인하고 누락 라인을 메워 보세요.
