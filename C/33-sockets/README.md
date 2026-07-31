# 33: 소켓 프로그래밍 — TCP/UDP 클라이언트-서버 (POSIX, 개념 중심)

## 소켓이란?

네트워크 통신의 끝점(endpoint)입니다. 프로세스가 OS를 통해 다른 호스트의 프로세스와 데이터를 주고받는 창구 역할을 합니다.

## TCP 서버의 흐름 (POSIX)

```c
socket()   →  bind()  →  listen()  →  accept()  →  recv()/send()  →  close()
```

```c
int sfd = socket(AF_INET, SOCK_STREAM, 0);
bind(sfd, (struct sockaddr*)&addr, sizeof(addr));
listen(sfd, 5);
int cfd = accept(sfd, NULL, NULL);
recv(cfd, buf, sizeof(buf), 0);
```

- **TCP**: 연결 지향, 신뢰성 보장 (순서, 재전송)
- **UDP**: 비연결, 빠르지만 신뢰성 없음 → `SOCK_DGRAM`

## 클라이언트의 흐름

```c
socket() → connect() → send()/recv() → close()
```

## 통신 순서 (TCP)

| 단계 | 서버 | 클라이언트 |
|------|------|-----------|
| 1 | socket, bind, listen | socket |
| 2 | accept (블로킹) | connect |
| 3 | recv | send |
| 4 | send | recv |
| 5 | close | close |

## Windows 참고

- POSIX `recv`/`send` 대신 Winsock: `WSAStartup`, `recv`, `send`
- 링크 시 `ws2_32.lib` 필요

본 강의 main.c는 표준 C만 사용합니다 (소켓 예제는 주석으로 제공).

## 실행

```bash
gcc main.c -o main && ./main
```
