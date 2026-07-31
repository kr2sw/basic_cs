# 27: 네트워킹 — TcpListener/TcpStream, HTTP 요청 개념

표준 라이브러리의 `std::net`으로 TCP 서버/클라이언트를 만듭니다.

## TCP 서버

```rust
let listener = TcpListener::bind("127.0.0.1:7878")?;
for stream in listener.incoming() {
    let stream = stream?;
    // 클라이언트 처리
}
```

## TCP 클라이언트

```rust
let mut stream = TcpStream::connect("127.0.0.1:7878")?;
stream.write_all(b"hello")?;
```

## HTTP 개념

HTTP는 TCP 위에서 동작하는 텍스트 프로토콜입니다. 요청/응답을 직접 파싱합니다.

## 실행

```bash
cd RUST/27-networking
cargo run
```

서버와 클라이언트를 각각 다른 터미널에서 실행할 수도 있습니다.
