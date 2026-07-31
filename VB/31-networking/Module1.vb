Imports System
Imports System.Net
Imports System.Net.Http
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading.Tasks

Module Program
    Sub Main()
        ' 동기 Main에서 비동기 예제 실행용 래퍼
        Task.Run(Async Function() Await RunDemoAsync()).GetAwaiter().GetResult()
    End Sub

    Async Function RunDemoAsync() As Task
        Console.WriteLine("=== 1. TCP 에코 서버/클라이언트 (루프백) ===")
        Dim port = 13031

        ' 서버를 백그라운드 스레드로 시작
        Dim serverTask = Task.Run(Sub() StartTcpServer(port))

        ' 클라이언트가 접속할 시간을 준다
        Await Task.Delay(300)

        ' 클라이언트에서 메시지 전송 및 에코 수신
        Using client As New TcpClient()
            Await client.ConnectAsync(IPAddress.Loopback, port)
            Console.WriteLine("  [클라이언트] 서버에 접속")

            Using stream = client.GetStream()
                Dim msg = "안녕하세요, TCP 서버!"
                Dim bytes = Encoding.UTF8.GetBytes(msg)
                Await stream.WriteAsync(bytes, 0, bytes.Length)

                ' 에코 응답 수신
                Dim buffer(1023) As Byte
                Dim received = Await stream.ReadAsync(buffer, 0, buffer.Length)
                Dim echo = Encoding.UTF8.GetString(buffer, 0, received)
                Console.WriteLine($"  [클라이언트] 에코 수신: {echo}")
            End Using
        End Using

        Await serverTask
        Console.WriteLine("  서버 종료됨")

        Console.WriteLine()
        Console.WriteLine("=== 2. HttpClient (REST API) ===")
        Using http As New HttpClient()
            http.Timeout = TimeSpan.FromSeconds(10)
            Try
                Dim body = Await http.GetStringAsync("https://httpbin.org/get")
                Console.WriteLine(body.Substring(0, Math.Min(200, body.Length)))
            Catch ex As Exception
                Console.WriteLine($"  HTTP 오류 (네트워크 없음): {ex.Message}")
            End Try
        End Using
    End Function

    ' TCP 서버: 접속을 받아 받은 메시지를 그대로 되돌려준다 (에코)
    Private Sub StartTcpServer(port As Integer)
        Dim listener As New TcpListener(IPAddress.Loopback, port)
        Try
            listener.Start()
            Console.WriteLine($"  [서버] 대기 중 (포트 {port})...")

            Using client = listener.AcceptTcpClient()
                Using stream = client.GetStream()
                    Dim buffer(1023) As Byte
                    Dim received = stream.Read(buffer, 0, buffer.Length)
                    Dim text = Encoding.UTF8.GetString(buffer, 0, received)
                    Console.WriteLine($"  [서버] 수신: {text}")

                    ' 에코 응답
                    stream.Write(buffer, 0, received)
                End Using
            End Using
        Finally
            listener.Stop()
        End Try
    End Sub
End Module
