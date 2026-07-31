// 28: 웹 프레임워크 — Axum/Actix 개념, 미니 라우터 구현
//
// Axum은 tokio 위에서 동작하므로 표준 라이브러리만으로 라우터 개념을 구현합니다.
// 실행 후 브라우저에서 http://127.0.0.1:8080/ 등을 열어보세요.

use std::io::{Read, Write};
use std::net::{TcpListener, TcpStream};

// === 1. 핸들러 타입 ===
type Handler = fn(path: &str) -> Response;

// === 2. 응답 구조 ===
struct Response {
    status: &'static str,
    body: String,
    content_type: &'static str,
}

impl Response {
    fn html(status: &'static str, body: String) -> Self {
        Response { status, body, content_type: "text/html; charset=utf-8" }
    }
    fn text(status: &'static str, body: String) -> Self {
        Response { status, body, content_type: "text/plain; charset=utf-8" }
    }
}

// === 3. 미니 라우터 ===
struct Router {
    routes: Vec<Route>,
}

struct Route {
    method: &'static str,
    pattern: String,          // "/users/:id" 형태
    handler: Handler,
}

impl Router {
    fn new() -> Self {
        Router { routes: Vec::new() }
    }

    fn add(&mut self, method: &'static str, pattern: &str, handler: Handler) {
        self.routes.push(Route { method, pattern: pattern.into(), handler });
    }

    fn get(&mut self, pattern: &str, handler: Handler) {
        self.add("GET", pattern, handler);
    }

    // 경로 매칭: /users/42 vs /users/:id
    fn match_route<'a>(&'a self, method: &str, path: &str) -> Option<(&'a Route, Vec<String>)> {
        for route in &self.routes {
            if route.method != method {
                continue;
            }
            let route_parts: Vec<&str> = route.pattern.split('/').collect();
            let path_parts: Vec<&str> = path.split('/').collect();
            if route_parts.len() != path_parts.len() {
                continue;
            }
            let mut params = Vec::new();
            let mut matched = true;
            for (rp, pp) in route_parts.iter().zip(path_parts.iter()) {
                if rp.starts_with(':') {
                    params.push((*pp).to_string());
                } else if *rp != *pp {
                    matched = false;
                    break;
                }
            }
            if matched {
                return Some((route, params));
            }
        }
        None
    }
}

// === 4. 핸들러들 ===
fn home(_: &str) -> Response {
    Response::html(
        "200 OK",
        "<h1>미니 웹 프레임워크</h1><ul><li><a href='/users/42'>/users/42</a></li><li><a href='/about'>/about</a></li><li><a href='/nope'>404 테스트</a></li></ul>".into(),
    )
}

fn about(_: &str) -> Response {
    Response::html("200 OK", "<h1>소개</h1><p>Rust 라우터 학습용 서버</p>".into())
}

fn user(params: &str) -> Response {
    // params는 "42"처럼 경로 파라미터가 콤마로 연결된 문자열로 전달
    Response::html(
        "200 OK",
        format!("<h1>사용자 상세</h1><p>id: {}</p>", params),
    )
}

fn not_found(_: &str) -> Response {
    Response::html("404 Not Found", "<h1>404</h1><p>경로가 없습니다</p>".into())
}

// === 5. 서버 실행 ===
fn serve(router: &Router) -> std::io::Result<()> {
    let listener = TcpListener::bind("127.0.0.1:8080")?;
    println!("서버 실행: http://127.0.0.1:8080");
    for stream in listener.incoming() {
        if let Ok(mut stream) = stream {
            handle(&mut stream, router)?;
        }
    }
    Ok(())
}

fn handle(stream: &mut TcpStream, router: &Router) -> std::io::Result<()> {
    let mut buffer = [0u8; 2048];
    let n = stream.read(&mut buffer)?;
    if n == 0 {
        return Ok(());
    }
    let request = String::from_utf8_lossy(&buffer[..n]);
    let first = request.lines().next().unwrap_or("GET / HTTP/1.1");
    let mut parts = first.split_whitespace();
    let method = parts.next().unwrap_or("GET");
    let path = parts.next().unwrap_or("/");

    println!("[{} {}] 미들웨어 로깅", method, path);

    let (status, body, ctype) = match router.match_route(method, path) {
        Some((route, params)) => {
            let r = (route.handler)(&params.join(","));
            (r.status, r.body, r.content_type)
        }
        None => {
            let r = not_found("");
            (r.status, r.body, r.content_type)
        }
    };

    let response = format!(
        "HTTP/1.1 {}\r\nContent-Length: {}\r\nContent-Type: {}\r\nConnection: close\r\n\r\n{}",
        status,
        body.len(),
        ctype,
        body
    );
    stream.write_all(response.as_bytes())?;
    stream.flush()?;
    Ok(())
}

fn main() -> std::io::Result<()> {
    let mut router = Router::new();
    router.get("/", home);
    router.get("/about", about);
    router.get("/users/:id", user);

    serve(&router)
}
