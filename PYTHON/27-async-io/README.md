# 27: 비동기 네트워크 (Async I/O) — aiohttp / httpx 비동기 클라이언트

## aiohttp / httpx
`aiohttp`와 `httpx`는 비동기 HTTP 클라이언트를 제공합니다. 동시에 여러 API를 호출해도 스레드를 차지하지 않습니다.

```python
import httpx
async with httpx.AsyncClient() as client:
    r = await client.get("https://api.github.com/repos/python/cpython")
    print(r.status_code, r.json()["stargazers_count"])
```

## aiohttp 사용 예 (서드파티 설치 필요)
```python
# pip install aiohttp
import aiohttp
async with aiohttp.ClientSession() as session:
    async with session.get("https://httpbin.org/json") as resp:
        print(await resp.json())
```

> 본 파일은 외부 서버 없이도 실행되도록 로컬 asyncio 서버를 띄워 실제 네트워크 I/O를 시뮬레이션합니다. 위 코드는 주석으로만 제공됩니다.

## 로컬 폴백 예제
`asyncio.start_server`로 로컬 서버를 띄우고, 비동기 클라이언트가 여러 요청을 동시에 보냅니다.

## 실행

```bash
python main.py
```
