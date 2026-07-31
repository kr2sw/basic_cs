# 38: 웹 서버 — MicroWebSrv-Style Implementation, REST

## 개요

ESP32 하나로 브라우저에서 접속할 수 있는 **웹 서버**를 만들 수 있습니다. 이번 레슨에서는 `socket`으로 HTTP 서버를 직접 구현하고, **REST API** 스타일의 라우팅을 설계합니다. MicroWebSrv 같은 라이브러리는 이 구조를 더 풍부하게 만든 것입니다.

## HTTP 요청의 구조

브라우저가 서버에 보내는 요청 텍스트입니다.

```http
GET /api/status HTTP/1.1
Host: 192.168.0.10
Connection: close

```

- 첫 줄: **메서드(GET/POST) + 경로(path) + 버전**
- 헤더 줄들, 빈 줄, 그 뒤에 선택적 **body** (POST 데이터)

## REST API 설계

자원(resource)을 경로로, 동작(action)을 메서드로 표현합니다.

| 메서드 | 경로 | 동작 |
|--------|------|------|
| GET | /api/status | 상태 조회 |
| GET | /api/state | LED/카운트 조회 |
| POST | /api/led | LED 켜기/끄기 |
| POST | /api/count/increment | 카운트 증가 |

```python
def handle_request(method, path, body):
    if method == "GET" and path == "/api/status":
        return http_response({"status": "running"})
    if method == "POST" and path == "/api/led":
        data = json.loads(body)
        ...
```

## JSON 응답

데이터는 JSON으로 직렬화해 반환합니다. Content-Type과 길이(Content-Length)를 정확히 지정해야 브라우저가 파싱할 수 있습니다.

```python
def http_response(body):
    body = json.dumps(body)
    header = f"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\n"
    header += f"Content-Length: {len(body)}\r\nConnection: close\r\n\r\n"
    return header + body
```

## 요청 파싱

`parse_request()`가 첫 줄에서 메서드/경로를, 빈 줄 뒤에서 body를 분리합니다. 여러 연결을 처리하려면 `uasyncio`와 결합하면 됩니다.

## 실행/업로드 방법

1. **Thonny IDE**: Wi-Fi 정보를 바꾸고 `MP/38-web-server/main.py` 실행(F5).
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/38-web-server/main.py
   ampy --port COM3 run MP/38-web-server/main.py
   ```
3. 시리얼에 출력된 `http://<IP>:80/`을 브라우저로 접속합니다. 버튼으로 LED를 켜고 끄세요.
4. REST 테스트: `curl http://<IP>/api/status` 도 사용 가능합니다.

## 핵심 개념 요약

- HTTP는 소켓 위의 텍스트 프로토콜 (메서드 + 경로 + 헤더 + body)
- REST = 자원(경로) + 동작(메서드) 설계 방식
- JSON 직렬화 + Content-Length로 표준 응답
- MicroWebSrv는 이 구조를 프레임워크화한 것
