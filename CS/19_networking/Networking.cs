namespace BasicCS.Chapter19;

using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Networking
{
    static async Task Main()
    {
        Console.WriteLine("=== 네트워킹(Networking) 예제 ===\n");

        // ──────────────────────────────────────────────
        // 1. HttpClient GET 요청
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 1. HttpClient GET 요청 ───");
        await HttpClientGetExample();

        // ──────────────────────────────────────────────
        // 2. HttpClient POST 요청
        // ──────────────────────────────────────────────
        Console.WriteLine("\n─── 2. HttpClient POST 요청 ───");
        await HttpClientPostExample();

        // ──────────────────────────────────────────────
        // 3. HttpClient.SendAsync (고급 사용)
        // ──────────────────────────────────────────────
        Console.WriteLine("\n─── 3. HttpClient.SendAsync ───");
        await HttpClientSendAsyncExample();

        // ──────────────────────────────────────────────
        // 4. DNS 조회 (Dns.GetHostEntryAsync)
        // ──────────────────────────────────────────────
        Console.WriteLine("\n─── 4. DNS 조회 ───");
        await DnsLookupExample();

        // ──────────────────────────────────────────────
        // 5. TCP 클라이언트/서버 (localhost)
        // ──────────────────────────────────────────────
        Console.WriteLine("\n─── 5. TCP 클라이언트/서버 ───");
        await TcpEchoExample();

        // ──────────────────────────────────────────────
        // 6. 네트워크 오류 처리 (try-catch)
        // ──────────────────────────────────────────────
        Console.WriteLine("\n─── 6. 네트워크 오류 처리 ───");
        await NetworkErrorHandlingExample();

        Console.WriteLine("\n=== 네트워킹 예제 종료 ===");
    }

    // ──────────────────────────────────────────────
    // HTTP GET 예제
    // ──────────────────────────────────────────────
    static async Task HttpClientGetExample()
    {
        using HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(5);

        try
        {
            // HTTP GET 요청 (공개 API 사용)
            HttpResponseMessage response = await client.GetAsync("https://httpbin.org/get");
            response.EnsureSuccessStatusCode(); // 200 OK 확인

            string body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GET 응답 상태코드: {(int)response.StatusCode} {response.ReasonPhrase}");
            Console.WriteLine($"응답 본문 (처음 200자):\n{body[..Math.Min(200, body.Length)]}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GET 요청 실패 (네트워크 없을 수 있음): {ex.Message}");
        }
    }

    // ──────────────────────────────────────────────
    // HTTP POST 예제
    // ──────────────────────────────────────────────
    static async Task HttpClientPostExample()
    {
        using HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(5);

        try
        {
            // JSON 데이터 준비
            var postData = new { Name = "C# 예제", Category = "네트워킹" };
            string json = System.Text.Json.JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // POST 요청
            HttpResponseMessage response = await client.PostAsync("https://httpbin.org/post", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"POST 응답 상태코드: {(int)response.StatusCode}");
            Console.WriteLine($"응답 본문 (처음 200자):\n{responseBody[..Math.Min(200, responseBody.Length)]}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST 요청 실패: {ex.Message}");
        }
    }

    // ──────────────────────────────────────────────
    // HttpClient.SendAsync
    // ──────────────────────────────────────────────
    static async Task HttpClientSendAsyncExample()
    {
        using HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(5);

        try
        {
            // HttpRequestMessage를 직접 구성하여 세밀한 제어
            var request = new HttpRequestMessage(HttpMethod.Get, "https://httpbin.org/headers");
            request.Headers.Add("User-Agent", "BasicCS-Example/1.0");
            request.Headers.Add("Accept", "application/json");

            // SendAsync로 전송
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // HttpContent에서 본문 읽기
            HttpContent responseContent = response.Content;
            string body = await responseContent.ReadAsStringAsync();

            Console.WriteLine($"SendAsync 응답 코드: {(int)response.StatusCode}");
            Console.WriteLine($"Content-Type: {responseContent.Headers.ContentType}");
            Console.WriteLine($"Content-Length: {responseContent.Headers.ContentLength}");
            Console.WriteLine($"Headers 응답 (처음 200자):\n{body[..Math.Min(200, body.Length)]}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SendAsync 요청 실패: {ex.Message}");
        }
    }

    // ──────────────────────────────────────────────
    // DNS 조회
    // ──────────────────────────────────────────────
    static async Task DnsLookupExample()
    {
        try
        {
            string host = "www.google.com";
            Console.WriteLine($"DNS 조회 중: {host}");

            IPHostEntry hostEntry = await Dns.GetHostEntryAsync(host);

            Console.WriteLine($"호스트명: {hostEntry.HostName}");
            Console.WriteLine($"별칭: {(hostEntry.Aliases.Length > 0 ? string.Join(", ", hostEntry.Aliases) : "없음")}");
            Console.WriteLine($"IP 주소 목록:");
            foreach (IPAddress addr in hostEntry.AddressList)
            {
                Console.WriteLine($"  - {addr} (AddressFamily: {addr.AddressFamily})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DNS 조회 실패: {ex.Message}");
        }
    }

    // ──────────────────────────────────────────────
    // TCP 에코 서버/클라이언트 (localhost)
    // ──────────────────────────────────────────────
    static async Task TcpEchoExample()
    {
        int port = 51111;
        using var cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        try
        {
            // TCP 서버를 별도 태스크에서 시작
            Task serverTask = Task.Run(async () =>
            {
                var listener = new TcpListener(IPAddress.Loopback, port);
                listener.Start();
                Console.WriteLine($"TCP 서버 시작: {IPAddress.Loopback}:{port}");

                try
                {
                    using TcpClient client = await listener.AcceptTcpClientAsync(token);
                    Console.WriteLine("클라이언트 연결 수락됨");

                    using NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"서버 수신: {received}");

                    // 에코 응답 전송
                    byte[] response = Encoding.UTF8.GetBytes($"Echo: {received}");
                    await stream.WriteAsync(response, 0, response.Length, token);
                    Console.WriteLine("서버 응답 전송 완료");
                }
                finally
                {
                    listener.Stop();
                }
            }, token);

            // 서버가 시작될 시간 확보
            await Task.Delay(500);

            // TCP 클라이언트 연결
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(IPAddress.Loopback, port);
            Console.WriteLine("TCP 클라이언트 연결 성공");

            using NetworkStream clientStream = tcpClient.GetStream();
            string message = "안녕, TCP 서버!";
            byte[] sendData = Encoding.UTF8.GetBytes(message);
            await clientStream.WriteAsync(sendData, 0, sendData.Length, token);
            Console.WriteLine($"클라이언트 전송: {message}");

            // 응답 수신
            byte[] recvBuffer = new byte[1024];
            int totalRead = await clientStream.ReadAsync(recvBuffer, 0, recvBuffer.Length, token);
            string responseMsg = Encoding.UTF8.GetString(recvBuffer, 0, totalRead);
            Console.WriteLine($"클라이언트 수신: {responseMsg}");

            await serverTask;
            Console.WriteLine("TCP 에코 예제 완료\n");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("TCP 작업 취소됨");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TCP 예제 오류: {ex.Message}");
        }
    }

    // ──────────────────────────────────────────────
    // 네트워크 오류 처리 예제
    // ──────────────────────────────────────────────
    static async Task NetworkErrorHandlingExample()
    {
        using HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(3);

        // 존재하지 않는 호스트에 요청 → 예외 발생
        try
        {
            var response = await client.GetAsync("https://이상한호스트이름.example.com");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HttpRequestException: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("TaskCanceledException: 요청 시간 초과");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"일반 예외: {ex.GetType().Name} - {ex.Message}");
        }

        // 잘못된 URL 형식
        try
        {
            var response = await client.GetAsync("htp://invalid-url");
        }
        catch (UriFormatException ex)
        {
            Console.WriteLine($"UriFormatException: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"기타 오류: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
