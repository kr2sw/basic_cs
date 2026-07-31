"""
36: 소켓 프로그래밍 — TCP 에코 서버/클라이언트, UDP 에코
로컬 루프백(127.0.0.1)에서만 동작합니다.
"""
import socket
import threading
import time

HOST, PORT = "127.0.0.1", 9000
UDP_PORT = 9001


# 1) TCP 에코 서버 (스레드로 백그라운드 실행)
def tcp_echo_server(stop_event):
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind((HOST, PORT))
    server.listen(1)
    server.settimeout(0.2)
    print(f"[TCP 서버] {HOST}:{PORT} 대기 중...")

    while not stop_event.is_set():
        try:
            conn, addr = server.accept()
        except socket.timeout:
            continue
        with conn:
            print(f"[TCP 서버] {addr} 연결됨")
            while True:
                data = conn.recv(1024)
                if not data:
                    break
                print(f"[TCP 서버] 수신: {data.decode()!r} -> 에코")
                conn.sendall(data)  # 받은 그대로 돌려줌
        print(f"[TCP 서버] {addr} 연결 종료")
    server.close()
    print("[TCP 서버] 종료")


# 2) UDP 에코 서버
def udp_echo_server(stop_event):
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.bind((HOST, UDP_PORT))
    sock.settimeout(0.2)
    print(f"[UDP 서버] {HOST}:{UDP_PORT} 대기 중...")

    while not stop_event.is_set():
        try:
            data, addr = sock.recvfrom(1024)
        except socket.timeout:
            continue
        print(f"[UDP 서버] {addr}로부터 {data.decode()!r} 수신 -> 에코")
        sock.sendto(data, addr)
    sock.close()
    print("[UDP 서버] 종료")


def tcp_client():
    print("--- TCP 클라이언트 ---")
    with socket.create_connection((HOST, PORT), timeout=3) as sock:
        messages = ["안녕하세요", "TCP 테스트", "bye"]
        for msg in messages:
            sock.sendall(msg.encode())
            reply = sock.recv(1024)
            print(f"  보냄: {msg!r}  / 받음: {reply.decode()!r}")
            time.sleep(0.1)


def udp_client():
    print("--- UDP 클라이언트 ---")
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.settimeout(3)
    sock.sendto("UDP 안녕".encode(), (HOST, UDP_PORT))
    data, addr = sock.recvfrom(1024)
    print(f"  받은 에코: {data.decode()!r}")
    sock.close()


if __name__ == "__main__":
    stop_event = threading.Event()

    # 서버들을 백그라운드 스레드로 시작
    tcp_thread = threading.Thread(target=tcp_echo_server, args=(stop_event,), daemon=True)
    udp_thread = threading.Thread(target=udp_echo_server, args=(stop_event,), daemon=True)
    tcp_thread.start()
    udp_thread.start()
    time.sleep(0.3)

    try:
        tcp_client()
        print()
        udp_client()
    finally:
        stop_event.set()  # 서버 종료 신호
        time.sleep(0.3)
    print("\n[메인] 모든 서버 종료 완료")
