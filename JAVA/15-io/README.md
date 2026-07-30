# 15: I/O — 파일 입출력

## java.io 패키지

### 바이트 스트림 (Byte Stream)
- `InputStream` / `OutputStream` (추상)
- `FileInputStream` / `FileOutputStream`
- `BufferedInputStream` / `BufferedOutputStream`

### 문자 스트림 (Char Stream)
- `Reader` / `Writer` (추상)
- `FileReader` / `FileWriter`
- `BufferedReader` / `BufferedWriter` (버퍼링, 한 줄 읽기)

### 보조 스트림
- `InputStreamReader` / `OutputStreamWriter` (바이트↔문자 변환)
- `PrintWriter` (편리한 출력)
- `DataInputStream` / `DataOutputStream` (기본형 단위)

## java.nio.file 패키지 (Java 7+)

- `Path`, `Files`, `Paths` 클래스
- `readAllLines()`, `write()`, `copy()`, `move()`, `delete()`
- `walk()`, `find()` (디렉토리 탐색)
