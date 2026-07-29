// 20-web-server
// 간단한 HTTP 서버: TcpListener, 요청 파싱, 응답, 스레드 풀

use std::fs;
use std::io::{BufRead, BufReader, Write};
use std::net::{TcpListener, TcpStream};
use std::sync::mpsc;
use std::sync::{Arc, Mutex};
use std::thread;

// --- ThreadPool: 작업자 스레드 풀 ---
struct ThreadPool {
    workers: Vec<Worker>,
    sender: Option<mpsc::Sender<Job>>,
}

type Job = Box<dyn FnOnce() + Send + 'static>;

impl ThreadPool {
    fn new(size: usize) -> Self {
        assert!(size > 0);

        let (sender, receiver) = mpsc::channel();
        let receiver = Arc::new(Mutex::new(receiver));

        let mut workers = Vec::with_capacity(size);
        for id in 0..size {
            workers.push(Worker::new(id, Arc::clone(&receiver)));
        }

        Self {
            workers,
            sender: Some(sender),
        }
    }

    fn execute<F>(&self, f: F)
    where
        F: FnOnce() + Send + 'static,
    {
        let job = Box::new(f);
        self.sender.as_ref().unwrap().send(job).unwrap();
    }
}

impl Drop for ThreadPool {
    fn drop(&mut self) {
        drop(self.sender.take());
        for worker in &mut self.workers {
            if let Some(thread) = worker.thread.take() {
                thread.join().unwrap();
            }
        }
    }
}

struct Worker {
    _id: usize,
    thread: Option<thread::JoinHandle<()>>,
}

impl Worker {
    fn new(id: usize, receiver: Arc<Mutex<mpsc::Receiver<Job>>>) -> Self {
        let thread = thread::spawn(move || loop {
            let message = receiver.lock().unwrap().recv();
            match message {
                Ok(job) => {
                    job();
                }
                Err(_) => {
                    break;
                }
            }
        });

        Self {
            _id: id,
            thread: Some(thread),
        }
    }
}

// --- HTTP 요청 파싱 ---
#[derive(Debug)]
struct HttpRequest {
    method: String,
    path: String,
    _version: String,
    _headers: Vec<String>,
}

fn parse_request(stream: &TcpStream) -> Option<HttpRequest> {
    let reader = BufReader::new(stream);
    let mut lines = reader.lines();

    // 첫 줄: "GET /path HTTP/1.1"
    let request_line = lines.next()?.ok()?;
    let mut parts = request_line.split_whitespace();
    let method = parts.next()?.to_string();
    let path = parts.next()?.to_string();
    let version = parts.next()?.to_string();

    let mut headers = Vec::new();
    for line in lines {
        let line = line.ok()?;
        if line.is_empty() {
            break;
        }
        headers.push(line);
    }

    Some(HttpRequest {
        method,
        path,
        _version: version,
        _headers: headers,
    })
}

// --- HTTP 응답 생성 ---
fn status_line(status_code: u32) -> &'static str {
    match status_code {
        200 => "HTTP/1.1 200 OK",
        404 => "HTTP/1.1 404 NOT FOUND",
        500 => "HTTP/1.1 500 INTERNAL SERVER ERROR",
        _ => "HTTP/1.1 200 OK",
    }
}

fn send_response(stream: &mut TcpStream, status: u32, content_type: &str, body: &str) {
    let status = status_line(status);
    let response = format!(
        "{}\r\nContent-Type: {}\r\nContent-Length: {}\r\nConnection: close\r\n\r\n{}",
        status,
        content_type,
        body.len(),
        body
    );
    stream.write_all(response.as_bytes()).unwrap();
    stream.flush().unwrap();
}

fn html_page(title: &str, body: &str) -> String {
    format!(
        r#"<!DOCTYPE html>
<html lang="ko">
<head><meta charset="UTF-8"><title>{}</title></head>
<body>
<h1>{}</h1>
<p>{}</p>
</body>
</html>"#,
        title, title, body
    )
}

// --- 라우팅 ---
fn handle_connection(mut stream: TcpStream) {
    let request = match parse_request(&stream) {
        Some(req) => req,
        None => {
            send_response(&mut stream, 500, "text/plain", "Bad Request");
            return;
        }
    };

    let response = match (request.method.as_str(), request.path.as_str()) {
        ("GET", "/") => html_page("Home", "Rust 웹 서버에 오신 것을 환영합니다!"),
        ("GET", "/about") => html_page("About", "이것은 Rust로 작성된 간단한 HTTP 서버입니다."),
        ("GET", "/data") => {
            // 로컬 파일 서빙 예제
            match fs::read_to_string("data.json") {
                Ok(content) => {
                    send_response(&mut stream, 200, "application/json", &content);
                    return;
                }
                Err(_) => html_page("404", "파일을 찾을 수 없습니다"),
            }
        }
        _ => html_page("404", "페이지를 찾을 수 없습니다"),
    };

    let status = if request.path == "/" || request.path == "/about" {
        200
    } else {
        404
    };
    send_response(&mut stream, status, "text/html; charset=utf-8", &response);
}

fn main() {
    // data.json 파일 생성 (파일 서빙 예제)
    let data = r#"{"message": "Hello from Rust server", "version": 1.0}"#;
    fs::write("data.json", data).unwrap();

    let listener = TcpListener::bind("127.0.0.1:7878").unwrap();
    println!("서버가 127.0.0.1:7878 에서 실행 중입니다.");

    let pool = ThreadPool::new(4);

    for stream in listener.incoming().take(3) {
        // 3개의 요청만 처리 후 종료
        match stream {
            Ok(stream) => {
                println!("새 연결: {:?}", stream.peer_addr().unwrap());
                pool.execute(|| {
                    handle_connection(stream);
                });
            }
            Err(e) => {
                eprintln!("연결 에러: {}", e);
            }
        }
    }

    // 정리
    let _ = fs::remove_file("data.json");
    println!("서버를 종료합니다.");
}
