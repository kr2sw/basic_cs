Imports System
Imports System.IO
Imports System.Threading

Module Program
    Sub Main()
        Console.WriteLine("=== Windows 서비스 개념 (ServiceBase) ===")
        Console.WriteLine("실제 ServiceBase 코드는 README 주석 참고 (OnStart/OnStop)")

        Dim logPath = Path.Combine(Path.GetTempPath(), "vb_service_demo.log")

        ' 서비스 라이프사이클 시뮬레이션: 시작 → 주기 작업 → 중지
        Dim service As New ServiceSimulator(logPath)
        service.OnStart(Nothing)
        Console.WriteLine("서비스 시작됨 (3초간 실행)...")
        Thread.Sleep(3000)
        service.OnStop()

        Console.WriteLine()
        Console.WriteLine("=== 로그 내용 ===")
        If File.Exists(logPath) Then
            For Each line In File.ReadAllLines(logPath)
                Console.WriteLine($"  {line}")
            Next
            File.Delete(logPath)
        Else
            Console.WriteLine("  (로그 없음)")
        End If
    End Sub
End Module

' ServiceBase 라이프사이클을 재현하는 시뮬레이터
' 실제 구현:
'   Public Class MyService
'       Inherits ServiceBase
'
'       Public Sub New()
'           Me.ServiceName = "MyService"
'       End Sub
'
'       Protected Overrides Sub OnStart(args As String())
'           StartTimer()
'       End Sub
'
'       Protected Overrides Sub OnStop()
'           StopTimer()
'       End Sub
'   End Class
Public Class ServiceSimulator
    Private ReadOnly _logPath As String
    Private _timer As Timer

    Public Sub New(logPath As String)
        _logPath = logPath
    End Sub

    ' ServiceBase.OnStart에 해당: 무거운 작업 대신 타이머 시작
    Public Sub OnStart(args() As String)
        File.AppendAllText(_logPath, $"{DateTime.Now:O} 서비스 시작{Environment.NewLine}")
        _timer = New Timer(AddressOf Tick, Nothing, 0, 1000)
    End Sub

    ' 1초마다 수행되는 주기 작업 (예: 로그 회전, 캐시 정리, 상태 점검)
    Private Sub Tick(state As Object)
        File.AppendAllText(_logPath, $"{DateTime.Now:O} 주기 작업 실행{Environment.NewLine}")
        Console.WriteLine($"  [tick] {DateTime.Now:HH:mm:ss}")
    End Sub

    ' ServiceBase.OnStop에 해당: 리소스 정리
    Public Sub OnStop()
        _timer?.Dispose()
        File.AppendAllText(_logPath, $"{DateTime.Now:O} 서비스 중지{Environment.NewLine}")
        Console.WriteLine("서비스 중지됨")
    End Sub
End Class
