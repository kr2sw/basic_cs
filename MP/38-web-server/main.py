# 38: 웹 서버 — MicroWebSrv-Style Implementation, REST
# 대상: ESP32 + Wi-Fi (브라우저로 접속해 REST API 테스트)
import network
import socket
import json
import time

SSID = "YOUR_WIFI_SSID"
PASSWORD = "YOUR_WIFI_PASSWORD"
PORT = 80

wlan = network.WLAN(network.STA_IF)

# --- 간단한 상태 저장소 ----------------------------------------------------------
state = {
    "led": False,
    "count": 0,
    "status": "running",
}

def connect_wifi():
    wlan.active(True)
    wlan.connect(SSID, PASSWORD)
    while not wlan.isconnected():
        time.sleep(0.5)
    print("연결 완료:", wlan.ifconfig()[0])


# --- HTTP 응답 헬퍼 ---------------------------------------------------------------
def http_response(body, content_type="application/json", code="200 OK"):
    """HTTP 응답 텍스트 생성 (JSON 기본)"""
    if isinstance(body, (dict, list)):
        body = json.dumps(body)
    elif isinstance(body, str) and content_type.startswith("text"):
        pass
    header = (f"HTTP/1.1 {code}\r\n"
              f"Content-Type: {content_type}\r\n"
              f"Content-Length: {len(body)}\r\n"
              "Connection: close\r\n"
              "\r\n")
    return header + body


# --- 라우팅 (간단한 REST API) --------------------------------------------------------
def handle_request(method, path, body):
    """method+path 기반 라우팅 — MicroWebSrv 스타일"""
    # GET /api/status
    if method == "GET" and path == "/api/status":
        return http_response(state)

    # GET /api/state
    if method == "GET" and path == "/api/state":
        return http_response({"led": state["led"], "count": state["count"]})

    # POST /api/led  body: {"on": true}
    if method == "POST" and path == "/api/led":
        try:
            data = json.loads(body)
            state["led"] = bool(data.get("on", False))
            from machine import Pin
            Pin(2, Pin.OUT).value(state["led"])
            return http_response({"result": "ok", "led": state["led"]})
        except (ValueError, OSError):
            return http_response({"error": "bad body"}, code="400 Bad Request")

    # POST /api/count/increment
    if method == "POST" and path == "/api/count/increment":
        state["count"] += 1
        return http_response({"count": state["count"]})

    # 웹 UI 페이지
    if method == "GET" and path == "/":
        html = """<html><body>
        <h1>ESP32 REST Server</h1>
        <p>LED: <span id="s"></span></p>
        <button onclick="fetch('/api/led',{method:'POST',body:'{\\"on\\":true}'})">LED ON</button>
        <button onclick="fetch('/api/led',{method:'POST',body:'{\\"on\\":false}'})">LED OFF</button>
        </body></html>"""
        return http_response(html, "text/html")

    return http_response({"error": "not found"}, code="404 Not Found")


def parse_request(req):
    """HTTP 요청 텍스트에서 method/path/body 분리"""
    lines = req.split(b"\r\n")
    try:
        method, path, _ = lines[0].decode().split(" ")
    except ValueError:
        return None, None, b""
    body = b""
    idx = lines.index(b"") if b"" in lines else -1
    if idx != -1:
        body = b"\r\n".join(lines[idx + 1:])
    return method, path, body


def run_server():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind(("0.0.0.0", PORT))
    server.listen(4)
    print(f"서버 시작: http://{wlan.ifconfig()[0]}:{PORT}/")

    while True:
        conn, addr = server.accept()
        conn.settimeout(5)
        try:
            request = b""
            while b"\r\n\r\n" not in request:
                chunk = conn.recv(1024)
                if not chunk:
                    break
                request += chunk
            if request:
                method, path, body = parse_request(request)
                if method:
                    print(f"{method} {path} from {addr[0]}")
                    response = handle_request(method, path, body)
                    conn.sendall(response.encode())
        except OSError:
            pass
        finally:
            conn.close()


def main():
    connect_wifi()
    run_server()


if __name__ == "__main__":
    main()
