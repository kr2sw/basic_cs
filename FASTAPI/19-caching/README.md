# 19: 캐싱 — Redis 캐싱, ETag

## 설치

```bash
pip install redis
```

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **Redis 캐싱**: 자주 조회되는 데이터 캐싱
- **ETag / If-None-Match**: HTTP 조건부 요청
- **Cache-Control**: 응답 캐시 헤더
- **메모이제이션**: 함수 결과 캐싱 (functools.lru_cache)
