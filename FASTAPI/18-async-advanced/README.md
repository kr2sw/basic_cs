# 18: 고급 비동기 — async/await 심화

## 설치

```bash
pip install httpx aiofiles
```

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **async def**: 코루틴 기반 핸들러 (비동기 DB/HTTP 호출에 필수)
- **httpx.AsyncClient**: 비동기 HTTP 클라이언트
- **asyncio.gather**: 여러 비동기 작업 병렬 실행
- **async DB 드라이버**: databases, asyncpg, aiomysql
