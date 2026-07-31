# 36: 소켓 프로그래밍 (Sockets) — socket, TCP/UDP 클라이언트-서버

## socket 모듈
OS의 네트워크 기능을 그대로 사용하는 저수준 인터페이스입니다. HTTP, SMTP 등 모든 프로토콜의 기반입니다.

```python
import socket
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect(("127.0.0.1", 9000))
```

## TCP vs UDP
- **TCP** (`SOCK_STREAM`): 연결 지향, 순서/신뢰성 보장. 채팅, 파일 전송 등
- **UDP** (`SOCK_DGRAM`): 비연결, 빠르지만 신뢰성 없음. 영상 스트리밍, 게임 등

## 서버 절차 (TCP)
`socket()` -> `bind()` -> `listen()` -> `accept()` 루프

## 클라이언트 절차 (TCP)
`socket()` -> `connect()` -> `sendall()`/`recv()`

## 실행

```bash
python main.py
```
