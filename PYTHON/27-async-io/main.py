"""
27: 비동기 네트워크 — 로컬 asyncio 서버 + 비동기 클라이언트
외부 API 대신 로컬 서버를 띄워 실제 소켓 I/O를 비동기로 처리하는 예제입니다.

실제 외부 API를 사용하려면 (주석 참고):
    # pip install httpx
    import httpx
    async with httpx.AsyncClient() as client:
        r = await client.get("https://api.github.com")
        print(r.status_code, r.json())

    # pip install aiohttp
    import aiohttp
    async with aiohttp.ClientSession() as session:
        async with session.get("https://httpbin.org/json") as resp:
            print(await resp.json())
"""
import asyncio
import time

USERS = {
    "1": {"id": 1, "name": "홍길동"},
    "2": {"id": 2, "name": "김철수"},
}


async def handle_connection(reader, writer):
    """클라이언트 요청을 받아 JSON 응답을 돌려주는 비동기 서버 핸들러"""
    addr = writer.get_extra_info("peername")
    print(f"[서버] {addr} 연결됨")
    try:
        data = await reader.readline()
        request = data.decode().strip()
        print(f"[서버] 요청 수신: {request!r}")
        # 간단한 라인 프로토콜: GET /users/{id}
        user_id = request.split("/")[-1]
        user = USERS.get(user_id, {"error": "not found"})
        response = str(user).encode() + b"\n"
        writer.write(response)
        await writer.drain()
    finally:
        writer.close()
        await writer.wait_closed()


async def client_request(name, user_id, delay):
    """비동기 클라이언트: 서버에 요청을 보내고 응답을 기다립니다."""
    print(f"[클라이언트] {name} 요청 시작: users/{user_id}")
    await asyncio.sleep(delay)  # 서버 부하 흉내
    reader, writer = await asyncio.open_connection("127.0.0.1", 8888)
    writer.write(f"GET /users/{user_id}\n".encode())
    await writer.drain()
    response = await reader.readline()
    writer.close()
    await writer.wait_closed()
    print(f"[클라이언트] {name} 응답 수신: {response.decode().strip()}")
    return response


async def main():
    server = await asyncio.start_server(handle_connection, "127.0.0.1", 8888)
    print("[메인] 로컬 비동기 서버 시작 (127.0.0.1:8888)")

    # 세 요청을 동시에 보냅니다 (gather). 총 소요는 가장 늦은 요청 기준.
    start = time.perf_counter()
    results = await asyncio.gather(
        client_request("A", 1, 0.3),
        client_request("B", 2, 0.5),
        client_request("C", 99, 0.7),  # 존재하지 않는 사용자
    )
    elapsed = time.perf_counter() - start
    print(f"\n[메인] 응답 {len(results)}건, 총 소요 {elapsed:.2f}s")

    server.close()
    await server.wait_closed()
    print("[메인] 서버 종료")


if __name__ == "__main__":
    asyncio.run(main())
