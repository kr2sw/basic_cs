# 26: 네트워킹 — TCP Socket Server/Client, DNS
# 대상: ESP32 + Wi-Fi (네트워크 사용 가능)
import network
import socket
import time

SSID = "YOUR_WIFI_SSID"
PASSWORD = "YOUR_WIFI_PASSWORD"
PORT = 8080

wlan = network.WLAN(network.STA_IF)


def connect_wifi():
    wlan.active(True)
    wlan.connect(SSID, PASSWORD)
    print("Wi-Fi 연결 중...")
    while not wlan.isconnected():
        time.sleep(0.5)
    print("연결 완료:", wlan.ifconfig()[0])


# --- 1) DNS 조회 ------------------------------------------------------
def dns_lookup(host):
    """호스트 이름을 IP 주소로 변환"""
    try:
        infos = socket.getaddrinfo(host, 80)
        ip = infos[0][4][0]
        print(f"DNS: {host} → {ip}")
        return ip
    except OSError as e:
        print(f"DNS 실패: {e}")
        return None


# --- 2) TCP 클라이언트 --------------------------------------------------
def tcp_client(host, port):
    """원격 서버에 HTTP GET 요청을 보내 응답을 받는 클라이언트"""
    addr = socket.getaddrinfo(host, port)[0][-1]
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.settimeout(5)
    try:
        s.connect(addr)
        print(f"연결됨: {addr}")
        request = b"GET / HTTP/1.0\r\nHost: " + host.encode() + b"\r\n\r\n"
        s.send(request)
        data = b""
        while True:
            chunk = s.recv(1024)
            if not chunk:
                break
            data += chunk
        print(f"응답 {len(data)} 바이트 수신")
        print(data[:200].decode(errors="replace"))
    except OSError as e:
        print("클라이언트 오류:", e)
    finally:
        s.close()


# --- 3) TCP 서버 ---------------------------------------------------------
def tcp_server(port):
    """클라이언트 연결을 기다리며 받은 텍스트를 그대로 되돌려주는 에코 서버"""
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind(("0.0.0.0", port))
    server.listen(2)
    print(f"TCP 에코 서버 대기 중: 0.0.0.0:{port}")

    while True:
        conn, addr = server.accept()
        print(f"클라이언트 연결: {addr}")
        try:
            data = conn.recv(1024)
            if data:
                print(f"수신: {data.decode().strip()}")
                conn.sendall(b"echo: " + data)   # 에코 응답
        except OSError:
            pass
        finally:
            conn.close()
            print("연결 종료")


def main():
    connect_wifi()

    print("\n=== DNS 조회 ===")
    dns_lookup("google.com")
    dns_lookup("nonexistent-domain-12345.com")  # 실패 예시

    print("\n=== TCP 클라이언트 (HTTP GET) ===")
    tcp_client("example.com", 80)

    print("\n=== TCP 서버 시작 ===")
    print("다른 장치에서 접속: telnet <ESP32_IP> 8080")
    tcp_server(PORT)


if __name__ == "__main__":
    main()
