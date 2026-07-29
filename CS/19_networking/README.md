# 19 네트워킹 (Networking)

C#의 `HttpClient`, `TcpClient`, `Dns` 등을 사용한 네트워크 프로그래밍을 학습합니다.

## 주요 개념

- `HttpClient` — HTTP GET/POST 요청
- `HttpRequestMessage` / `SendAsync` — 세밀한 요청 제어
- `Dns.GetHostEntryAsync` — DNS 조회
- `TcpListener` / `TcpClient` — TCP 서버/클라이언트 (Echo 예제)
- 네트워크 예외 처리 (`HttpRequestException`, `TaskCanceledException`)

## 예제 코드

```csharp
using HttpClient client = new HttpClient();
HttpResponseMessage response = await client.GetAsync("https://httpbin.org/get");

IPHostEntry hostEntry = await Dns.GetHostEntryAsync("www.google.com");

TcpListener listener = new TcpListener(IPAddress.Loopback, port);
using TcpClient client = await listener.AcceptTcpClientAsync();
// NetworkStream으로 데이터 송수신
```

## 실행 방법

```bash
dotnet run --project ../19_networking
```

## 핵심 요약

- `HttpClient`는 HTTP 통신의 주요 클래스로, 재사용하는 것이 모범 사례입니다.
- `TcpListener` / `TcpClient`로 저수준 TCP 통신을 구현할 수 있습니다.
- 네트워크 작업은 항상 예외 처리와 타임아웃을 고려해야 합니다.
