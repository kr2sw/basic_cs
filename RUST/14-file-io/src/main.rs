// 14-file-io
// 파일 입출력: std::fs, std::io::BufReader/BufWriter, Path/PathBuf

use std::fs::{self, File, read_dir, read_to_string};
use std::io::{BufRead, BufReader, BufWriter, Write};
use std::path::{Path, PathBuf};
use std::error::Error;

// ? 연산자로 에러 전파
fn read_file_content(path: &str) -> Result<String, Box<dyn Error>> {
    let content = read_to_string(path)?;
    Ok(content)
}

fn write_file_content(path: &str, content: &str) -> Result<(), Box<dyn Error>> {
    fs::write(path, content)?;
    Ok(())
}

fn read_with_bufreader(path: &str) -> Result<Vec<String>, Box<dyn Error>> {
    let file = File::open(path)?;
    let reader = BufReader::new(file);
    let mut lines = Vec::new();
    for line in reader.lines() {
        lines.push(line?);
    }
    Ok(lines)
}

fn write_with_bufwriter(path: &str, lines: &[&str]) -> Result<(), Box<dyn Error>> {
    let file = File::create(path)?;
    let mut writer = BufWriter::new(file);
    for line in lines {
        writeln!(writer, "{}", line)?;
    }
    Ok(())
}

fn list_directory(path: &str) -> Result<(), Box<dyn Error>> {
    for entry in read_dir(path)? {
        let entry = entry?;
        let path = entry.path();
        let metadata = entry.metadata()?;
        let file_type = if metadata.is_dir() { "DIR" } else { "FILE" };
        let size = metadata.len();
        println!("[{}] {} ({} bytes)", file_type, path.display(), size);
    }
    Ok(())
}

fn path_demo() {
    // Path와 PathBuf
    let path = Path::new("data");
    let mut path_buf = PathBuf::from("data");
    path_buf.push("subdir");
    path_buf.push("test.txt");

    println!("Path: {:?}", path);
    println!("PathBuf: {:?}", path_buf);
    println!("exists: {}", path.exists());
    println!("is_file: {}", path.is_file());
    println!("is_dir: {}", path.is_dir());
    println!("parent: {:?}", path.parent());
    println!("file_name: {:?}", path.file_name());
    println!("extension: {:?}", path.extension());
    println!("with_extension: {:?}", path_buf.with_extension("md"));
}

fn main() -> Result<(), Box<dyn Error>> {
    // 디렉토리 생성
    let dir = "file_io_example";
    fs::create_dir_all(dir)?;

    // 파일 쓰기 (간단)
    write_file_content(&format!("{}/hello.txt", dir), "안녕하세요, Rust!")?;
    println!("파일 쓰기 완료");

    // 파일 읽기 (간단)
    let content = read_file_content(&format!("{}/hello.txt", dir))?;
    println!("파일 내용: {}", content);

    // BufWriter로 여러 줄 쓰기
    write_with_bufwriter(
        &format!("{}/lines.txt", dir),
        &["첫째 줄", "둘째 줄", "셋째 줄"],
    )?;
    println!("BufWriter 쓰기 완료");

    // BufReader로 여러 줄 읽기
    let lines = read_with_bufreader(&format!("{}/lines.txt", dir))?;
    for (i, line) in lines.iter().enumerate() {
        println!("{}: {}", i + 1, line);
    }

    // 디렉토리 목록
    list_directory(dir)?;

    // Path/PathBuf 데모
    path_demo();

    // 임시 파일 정리
    fs::remove_dir_all(dir)?;
    println!("임시 파일 정리 완료");

    Ok(())
}
