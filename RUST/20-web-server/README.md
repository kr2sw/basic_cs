# 20 Web Server — 웹 서버

TcpListener, HTTP 요청 파싱, 응답 생성, 스레드 풀을 사용한 간단한 웹 서버.

## 주요 개념
- `TcpListener` — TCP 연결 수신 (`bind`, `incoming`)
- `BufReader`로 HTTP 요청 파싱 (메서드, 경로, 헤더)
- HTTP 응답 생성: status line, headers, body
- 라우팅: 요청 경로에 따른 응답 분기
- `ThreadPool`: 작업자 스레드 풀 (mpsc + Arc<Mutex<Receiver>>)
- `Drop` 트레이트로 정리 (스레드 종료 대기)
- 파일 서빙: `fs::read_to_string`

```rust
let listener = TcpListener::bind("127.0.0.1:7878").unwrap();
let pool = ThreadPool::new(4);

for stream in listener.incoming().take(3) {
    let stream = stream.unwrap();
    pool.execute(|| {
        handle_connection(stream);
    });
}

fn handle_connection(mut stream: TcpStream) {
    let request = parse_request(&stream);
    let response = match request.path.as_str() {
        "/" => html_page("Home", "환영합니다!"),
        _ => html_page("404", "페이지를 찾을 수 없습니다"),
    };
    send_response(&mut stream, 200, "text/html", &response);
}
```

## 실행
```bash
cd RUST/20-web-server && cargo run
```

## 핵심 요점
- `TcpListener`로 TCP 연결 수신 후 스트림 처리
- HTTP는 텍스트 기반 프로토콜로 직접 파싱 가능
- 스레드 풀로 동시 요청 처리 (mpsc 작업 큐)
- `Drop` 트레이트로 graceful shutdown 구현
