# 26: 네트워킹 — TCP Socket Server/Client, DNS

## 개요

IoT 기기가 인터넷과 통신하는 바탕은 **소켓(Socket)** 입니다. 소켓은 프로세스 간 통신의 종점으로, TCP(신뢰성)와 UDP(속도) 두 방식이 있습니다. 이번 레슨에서는 **DNS 조회**, **TCP 클라이언트**(HTTP 요청), **TCP 서버**(에코 서버)를 직접 구현합니다.

## TCP의 기본 흐름

- **서버**: `bind()` 주소 고정 → `listen()` 대기 → `accept()` 연결 수락 → `recv()/send()`
- **클라이언트**: `connect()` 연결 → `send()/recv()`
- `socket.SOCK_STREAM`이 TCP, `socket.SOCK_DGRAM`이 UDP입니다.

```python
import socket
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.settimeout(5)                 # 블로킹 최대 5초
addr = socket.getaddrinfo("example.com", 80)[0][-1]
s.connect(addr)
```

## DNS (Domain Name System)

호스트 이름(`example.com`)을 IP 주소로 변환합니다. MicroPython의 `getaddrinfo()`가 DNS 조회를 수행합니다.

```python
info = socket.getaddrinfo("google.com", 80)
ip = info[0][4][0]   # ('8.8.8.8', 80) 형태 → IP만 추출
```

## TCP 클라이언트로 HTTP 요청

소켓에 HTTP 요청 텍스트를 직접 보내면 웹 서버에서 응답을 받을 수 있습니다. `urequests` 라이브러리도 내부적으로 이 과정을 수행합니다.

## TCP 에코 서버

같은 Wi-Fi 안의 다른 기기(스마트폰, PC)가 이 보드에 연결해 메시지를 주고받을 수 있습니다. 사물인터넷 장치가 "서버"가 되는 첫 단계입니다.

## 실행/업로드 방법

1. **Thonny IDE**: Wi-Fi 정보를 실제 값으로 바꾸고 `MP/26-networking/main.py` 실행(F5).
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/26-networking/main.py
   ampy --port COM3 run MP/26-networking/main.py
   ```
3. 시리얼 출력에서 보드의 IP를 확인한 뒤, PC에서 `telnet <IP> 8080` 또는 `nc <IP> 8080`으로 에코 서버에 접속해 메시지를 보내보세요.

## 핵심 개념 요약

- 소켓은 TCP/UDP 통신의 종점, `SOCK_STREAM`=TCP
- `getaddrinfo()`로 도메인 → IP 변환(DNS)
- 서버: bind → listen → accept 루프 / 클라이언트: connect
- HTTP도 결국 소켓 위의 텍스트 프로토콜
