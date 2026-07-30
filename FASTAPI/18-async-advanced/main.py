import asyncio
import time

import httpx
from fastapi import FastAPI

app = FastAPI()


async def fetch_url(client: httpx.AsyncClient, url: str) -> dict:
    response = await client.get(url)
    return {"url": url, "status": response.status_code, "time": time.strftime("%H:%M:%S")}


@app.get("/sequential")
async def sequential():
    urls = [
        "https://httpbin.org/delay/1",
        "https://httpbin.org/delay/2",
        "https://httpbin.org/delay/1",
    ]
    start = time.time()
    results = []
    async with httpx.AsyncClient() as client:
        for url in urls:
            result = await fetch_url(client, url)
            results.append(result)
    return {"method": "sequential", "time": time.time() - start, "results": results}


@app.get("/parallel")
async def parallel():
    urls = [
        "https://httpbin.org/delay/1",
        "https://httpbin.org/delay/2",
        "https://httpbin.org/delay/1",
    ]
    start = time.time()
    async with httpx.AsyncClient() as client:
        tasks = [fetch_url(client, url) for url in urls]
        results = await asyncio.gather(*tasks)
    return {"method": "parallel", "time": time.time() - start, "results": results}


@app.get("/sync")
def sync_endpoint():
    import requests
    start = time.time()
    urls = [
        "https://httpbin.org/delay/1",
        "https://httpbin.org/delay/2",
        "https://httpbin.org/delay/1",
    ]
    results = []
    for url in urls:
        response = requests.get(url)
        results.append({"url": url, "status": response.status_code})
    return {"method": "sync", "time": time.time() - start, "results": results}
