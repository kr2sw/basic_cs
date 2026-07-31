// 27: 네트워킹 — TcpListener/TcpStream, HTTP 요청 개념
//
// 실행: cargo run [server|client]
//   server: 127.0.0.1:7878 에서 요청을 받아 HTTP 응답을 돌려줌
//   client: 서버에 간단한 HTTP 요청을 보내고 응답을 출력
// 아무 인자도 없으면 server 모드로 동작합니다.

use std::env;
use std::io::{Read, Write};
use std::net::{TcpListener, TcpStream};

// === 1. 서버 ===
fn run_server() -> std::io::Result<()> {
    let listener = TcpListener::bind("127.0.0.1:7878")?;
    println!("서버 대기 중: http://127.0.0.1:7878");

    for stream in listener.incoming() {
        match stream {
            Ok(mut stream) => {
                handle_client(&mut stream)?;
            }
            Err(e) => println!("연결 오류: {e}"),
        }
    }
    Ok(())
}

fn handle_client(stream: &mut TcpStream) -> std::io::Result<()> {
    let mut buffer = [0u8; 1024];
    let n = stream.read(&mut buffer)?;
    if n == 0 {
        return Ok(());
    }

    let request = String::from_utf8_lossy(&buffer[..n]);
    let first_line = request.lines().next().unwrap_or("").to_string();
    println!("받은 요청: {first_line}");

    // 경로 추출 (예: GET /hello HTTP/1.1)
    let path = first_line
        .split_whitespace()
        .nth(1)
        .unwrap_or("/");

    let (status, body) = match path {
        "/" => ("200 OK", "<h1>홈</h1><p>Rust 네트워킹 예제</p>"),
        "/hello" => ("200 OK", "<h1>안녕하세요!</h1>"),
        _ => ("404 Not Found", "<h1>404</h1><p>찾을 수 없습니다</p>"),
    };

    let response = format!(
        "HTTP/1.1 {}\r\nContent-Length: {}\r\nContent-Type: text/html; charset=utf-8\r\nConnection: close\r\n\r\n{}",
        status,
        body.len(),
        body
    );
    stream.write_all(response.as_bytes())?;
    stream.flush()?;
    Ok(())
}

// === 2. 클라이언트 ===
fn run_client() -> std::io::Result<()> {
    let mut stream = TcpStream::connect("127.0.0.1:7878")?;
    println!("서버에 연결됨");

    // HTTP 요청 작성
    let request = "GET /hello HTTP/1.1\r\nHost: 127.0.0.1\r\nConnection: close\r\n\r\n";
    stream.write_all(request.as_bytes())?;
    stream.flush()?;

    // 응답 읽기
    let mut response = String::new();
    stream.read_to_string(&mut response)?;
    println!("=== 서버 응답 ===");
    println!("{}", response);
    Ok(())
}

// === 3. 개념: 주소 구조 보여주기 ===
fn address_concepts() {
    use std::net::{IpAddr, Ipv4Addr, SocketAddr};
    let ip: IpAddr = Ipv4Addr::new(127, 0, 0, 1).into();
    let addr = SocketAddr::new(ip, 7878);
    println!("소켓 주소: {addr}");
}

fn main() -> std::io::Result<()> {
    address_concepts();

    let args: Vec<String> = env::args().collect();
    let mode = args.get(1).map(|s| s.as_str()).unwrap_or("server");

    match mode {
        "client" => run_client()?,
        _ => run_server()?,
    }
    Ok(())
}
