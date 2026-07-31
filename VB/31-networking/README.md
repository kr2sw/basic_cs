# 31: 네트워킹 — TcpClient/Listener, HttpClient

## 소개

.NET의 네트워킹 라이브러리를 다룹니다. 저수준 TCP 통신(`TcpListener`/`TcpClient`)으로 에코 서버/클라이언트를 만들고, 고수준 HTTP 통신(`HttpClient`)으로 REST API를 호출합니다.

## 주요 개념

### 1. TCP 서버 — TcpListener

`TcpListener`로 포트를 열고 접속을 받아들입니다. 연결된 `NetworkStream`으로 바이트를 읽고 씁니다.

```vb
Dim listener As New TcpListener(IPAddress.Loopback, port)
listener.Start()
Using client = listener.AcceptTcpClient()
    Using stream = client.GetStream()
        Dim received = stream.Read(buffer, 0, buffer.Length)
        stream.Write(buffer, 0, received)   ' 에코 응답
    End Using
End Using
```

### 2. TCP 클라이언트 — TcpClient

`TcpClient.ConnectAsync`로 서버에 접속한 뒤 스트림에 데이터를 보냅니다.

```vb
Using client As New TcpClient()
    Await client.ConnectAsync(IPAddress.Loopback, port)
    Using stream = client.GetStream()
        Await stream.WriteAsync(bytes, 0, bytes.Length)
        Dim received = Await stream.ReadAsync(buffer, 0, buffer.Length)
    End Using
End Using
```

텍스트는 `Encoding.UTF8`로 바이트 변환합니다.

### 3. HttpClient — HTTP 통신

REST API 호출의 표준입니다. `GetStringAsync` 등으로 응답을 받습니다. 연결 재사용을 위해 애플리케이션에서 재사용하는 것이 권장됩니다.

```vb
Using http As New HttpClient()
    http.Timeout = TimeSpan.FromSeconds(10)
    Dim body = Await http.GetStringAsync("https://httpbin.org/get")
End Using
```

JSON 전송은 `StringContent` + `application/json`을 사용하거나 `PostAsJsonAsync` 확장 메서드를 씁니다.

### 4. IP 주소와 포트

- `IPAddress.Loopback` (127.0.0.1): 로컬
- 포트: 0~65535, 잘 알려진 포트(80, 443 등)는 서비스에 예약

## 실행

```bash
dotnet run
```

## 정리

- TCP: `TcpListener`(서버) + `TcpClient`(클라이언트), 바이트 스트림 통신.
- HTTP: `HttpClient`로 REST API 호출, 비동기 메서드 사용.
- 모든 네트워크 호출은 예외(오프라인, 타임아웃) 처리가 필수입니다.
- 예제는 로컬 루프백으로 실행되어 별도 서버가 필요 없습니다.
