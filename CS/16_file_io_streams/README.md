# 16 파일 입출력과 스트림 (File I/O & Streams)

C#에서 파일을 읽고 쓰고, 스트림을 활용하는 방법을 학습합니다.

## 주요 개념

- `File.WriteAllText` / `File.ReadAllText` — 간편한 텍스트 파일 처리
- `File.ReadAllLines` — 줄 단위 읽기
- `StreamReader` / `StreamWriter` — 스트림 기반 텍스트 I/O
- `FileStream` — 바이너리 파일 처리
- `BinaryReader` / `BinaryWriter` — 이진 데이터 직렬화
- `Directory` / `DirectoryInfo` — 디렉터리 조작
- `File.Exists` / `Directory.Exists` — 존재 여부 확인
- `Path.Combine`, `GetExtension`, `GetFileName` — 경로 유틸리티

## 예제 코드

```csharp
string content = File.ReadAllText("sample.txt");
string[] lines = File.ReadAllLines("sample.txt");

using (StreamWriter writer = new StreamWriter(file2, false, Encoding.UTF8))
    writer.WriteLine("스트림으로 작성");

byte[] data = { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"
using (FileStream fs = new FileStream(file3, FileMode.Create))
    fs.Write(data, 0, data.Length);
```

## 실행 방법

```bash
dotnet run --project ../16_file_io_streams
```

## 핵심 요약

- `File` 클래스의 정적 메서드로 간단한 파일 I/O를 처리합니다.
- `StreamReader` / `StreamWriter`는 텍스트, `FileStream` / `BinaryReader`는 바이너리 데이터에 적합합니다.
- `using` 문으로 스트림 리소스를 안전하게 해제합니다.
