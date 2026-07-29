# 14 File I/O — 파일 입출력

파일 읽기/쓰기, BufReader/BufWriter, Path/PathBuf, 디렉토리 탐색.

## 주요 개념
- `std::fs`: 파일 읽기(`read_to_string`), 쓰기(`write`), 디렉토리 생성
- `BufReader`: 파일을 줄 단위로 읽기 (성능 최적화)
- `BufWriter`: 버퍼링된 파일 쓰기 / `writeln!` 매크로
- `Path` / `PathBuf`: 경로 조작 (push, parent, extension 등)
- `read_dir`: 디렉토리 내용 탐색
- `?` 연산자로 에러 전파
- `Box<dyn Error>`로 다양한 에러 타입 통합

```rust
fn read_file_content(path: &str) -> Result<String, Box<dyn Error>> {
    let content = read_to_string(path)?;
    Ok(content)
}

fn read_with_bufreader(path: &str) -> Result<Vec<String>, Box<dyn Error>> {
    let file = File::open(path)?;
    let reader = BufReader::new(file);
    reader.lines().collect()
}

fn path_demo() {
    let mut path_buf = PathBuf::from("data");
    path_buf.push("subdir");
    path_buf.push("test.txt");
    println!("extension: {:?}", path_buf.extension());
}
```

## 실행
```bash
cd RUST/14-file-io && cargo run
```

## 핵심 요점
- `fs::read_to_string`과 `fs::write`로 간단한 파일 I/O
- `BufReader`/`BufWriter`로 대용량 파일 효율적 처리
- `PathBuf`는 소유권 있는 경로, `Path`는 참조
- `?` 연산자로 에러 처리 간결하게
