# 16: 테스트 — TestClient, pytest

## 설치

```bash
pip install pytest httpx
```

## 실행

```bash
pytest main.py -v
```

## 주요 개념

- **TestClient**: FastAPI 앱 테스트용 클라이언트
- **httpx**: HTTP 요청 라이브러리 (TestClient 기반)
- **pytest**: Python 테스트 프레임워크
- **@pytest.fixture**: 테스트 설정/정리
